using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpForce = 5.0f;
    public Transform cameraTransform;
    private Rigidbody rb;
    private bool isGrounded;
    private Animator animator;
    private bool isAttacking = false; // Variable para controlar el estado de ataque
    private bool isDead = false; // Variable para controlar el estado de muerte
    private GameController gameController;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (isDead)
        {
            return; // No hacer nada si el personaje está muerto
        }

        // Si no está atacando ni muerto, permitir el movimiento y otros controles
        if (!isAttacking)
        {
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = cameraTransform.forward * moveZ;
            move.y = 0; // Mantener al jugador en el plano horizontal

            if (move.magnitude > 0.1f) // Verificar si hay movimiento
            {
                animator.SetBool("Run", true);
                rb.MovePosition(transform.position + move * speed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Run", false);
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                animator.SetBool("Jump", true);
                animator.SetBool("Run", false);
            }
        }

        // Controlar el ataque
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking && !isDead)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        animator.SetBool("Run", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Atacar", true);
        isAttacking = true; // Iniciar el ataque y bloquear otros controles

        // Esperar 0.8 segundos (ajustar según la duración de la animación de ataque)
        yield return new WaitForSeconds(0.4f);

        // Después de la animación de ataque, restablecer el estado de ataque
        animator.SetBool("Atacar", false);
        isAttacking = false;
    }

    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true;
        animator.SetBool("Jump", false);
    }

    if (collision.gameObject.CompareTag("Enemigo") && !isDead)
    {
        gameController.RestarVida(1); // Restar una vida
        if (gameController.GetVida() <= 0)
        {
            Die();
        }
    }

    if (collision.gameObject.CompareTag("Premio"))
    {
        Destroy(collision.gameObject);
        if (gameController != null)
        {
            gameController.SumarPuntos(1);
        }
    }
}


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Die()
    {
        if (isDead) return; // Evitar llamar Die() múltiples veces

        animator.SetBool("Run", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Atacar", false);
        isDead = true;
        animator.SetBool("Morir", true);
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // Aquí podrías agregar más lógica como desactivar el control del jugador, etc.
    }


    public void FinishAttackAnimation()
    {
        isAttacking = false; // Restablecer el estado de ataque al finalizar la animación de ataque
        animator.SetBool("Atacar", false); // Desactivar la animación de ataque
    }
}
