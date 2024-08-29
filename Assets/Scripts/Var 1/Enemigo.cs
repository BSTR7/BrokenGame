using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform[] puntosPatrulla; // Array de puntos de patrulla
    public float velocidad = 2f; // Velocidad de movimiento del enemigo
    public float velocidadPersecucion = 4f; // Velocidad de persecución del enemigo
    public Transform jugador; // Referencia al jugador
    public float rangoPersecucion = 5f; // Rango en el cual el enemigo empieza a seguir al jugador

    private int indicePuntoActual = 0; // Índice del punto de patrulla actual
    private bool avanzando = true; // Dirección de la patrulla
    private Animator jugadorAnimator; // Referencia al Animator del jugador

    private Animator AEnemigo; // Referencia al Animator del enemigo

    void Start()
    {
        AEnemigo = GetComponent<Animator>(); // Obtenemos la referencia al Animator del enemigo

        if (jugador != null)
        {
            jugadorAnimator = jugador.GetComponent<Animator>(); // Obtenemos la referencia al Animator del jugador
        }
    }

    void Update()
    {
        // Si no hay puntos de patrulla definidos, no hacemos nada
        if (puntosPatrulla.Length == 0 || jugadorAnimator == null)
            return;

        // Si el jugador está muerto, el enemigo no se mueve
        if (jugadorAnimator.GetBool("Morir"))
        {
            AEnemigo.SetBool("Correr", false); // Detenemos la animación de correr
            AEnemigo.SetBool("Caminar", true); // Activamos la animación de caminar
            Patrullar(); // Continuamos patrullando
            return;
        }

        // Comprobar la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoPersecucion)
        {
            // Perseguir al jugador
            PerseguirJugador();
        }
        else
        {
            // Continuar con la patrulla
            Patrullar();
            AEnemigo.SetBool("Correr", false); // Detenemos la animación de correr
            AEnemigo.SetBool("Caminar", true); // Activamos la animación de caminar
        }
    }

    void Patrullar()
    {
        AEnemigo.SetBool("Caminar", true); // Activamos la animación de caminar

        // Movemos el enemigo hacia el punto de patrulla actual
        Transform puntoObjetivo = puntosPatrulla[indicePuntoActual];
        Vector3 direccion = puntoObjetivo.position - transform.position;
        direccion.y = 0; // Mantener la dirección en el plano horizontal
        Vector3 movimiento = direccion.normalized * velocidad * Time.deltaTime;

        // Aplicar el movimiento
        transform.position += movimiento;

        // Comprobamos si hemos llegado al punto de patrulla actual
        if (Vector3.Distance(transform.position, puntoObjetivo.position) < 0.1f)
        {
            // Actualizamos el índice del punto de patrulla según la dirección de patrulla
            if (avanzando)
            {
                indicePuntoActual++;
                if (indicePuntoActual >= puntosPatrulla.Length)
                {
                    indicePuntoActual = puntosPatrulla.Length - 2;
                    avanzando = false;
                }
            }
            else
            {
                indicePuntoActual--;
                if (indicePuntoActual < 0)
                {
                    indicePuntoActual = 1;
                    avanzando = true;
                }
            }

            // Giramos el enemigo para que mire inmediatamente hacia el siguiente punto de patrulla
            direccion = puntosPatrulla[indicePuntoActual].position - transform.position;
            direccion.y = 0; // Mantener la dirección en el plano horizontal
            transform.rotation = Quaternion.LookRotation(direccion);
        }
    }

    void PerseguirJugador()
    {
        AEnemigo.SetBool("Correr", true); // Activamos la animación de correr
        // Movemos el enemigo hacia el jugador
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0; // Mantener la dirección en el plano horizontal
        Vector3 movimiento = direccion.normalized * velocidadPersecucion * Time.deltaTime; // Usamos la velocidad de persecución

        // Aplicar el movimiento
        transform.position += movimiento;

        // Girar para mirar al jugador
        if (direccion != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direccion);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Comprobar si la colisión es con el jugador
        {

            AEnemigo.SetBool("Correr", false);
            AEnemigo.SetBool("Caminar", false);
            AEnemigo.SetBool("Atacar", true); // Activar la animación de ataque
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Comprobar si el jugador deja de colisionar
        {
            AEnemigo.SetBool("Atacar", false); // Desactivar la animación de ataque
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < puntosPatrulla.Length - 1; i++)
        {
            Gizmos.DrawLine(puntosPatrulla[i].position, puntosPatrulla[i + 1].position);
        }

        // Dibujar el rango de persecución
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoPersecucion);
    }
}
