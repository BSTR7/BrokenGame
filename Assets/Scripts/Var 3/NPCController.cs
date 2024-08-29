using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Transform jugador; // Referencia al jugador
    public float rangoInteraccion = 5f; // Rango en el cual el NPC puede interactuar con el jugador

    private GameController gameController; // Referencia al GameController
    private bool isPlayerInRange = false; // Si el jugador está en rango
    private bool puntosDados = false; // Si los puntos ya fueron dados

    void Start()
    {
        gameController = FindObjectOfType<GameController>(); // Obtenemos la referencia al GameController
    }

    void Update()
    {
        // Comprobar la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoInteraccion)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }

        // Ejecutar acción si el jugador está en rango, presiona la tecla G y no se han dado puntos aún
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.G) && !puntosDados)
        {
            gameController.SumarPuntos(3);
            puntosDados = true; // Marcar que los puntos ya fueron dados
        }
    }

    void OnDrawGizmos()
    {
        // Dibujar el rango de interacción
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoInteraccion);
    }
}
