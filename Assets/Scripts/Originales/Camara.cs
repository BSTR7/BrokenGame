using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;
    public float minVerticalAngle = -20.0f; // Ángulo mínimo de rotación vertical
    public float maxVerticalAngle = 90.0f;  // Ángulo máximo de rotación vertical

    private float xRotation = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        // Limitar la rotación de la cámara entre minVerticalAngle y maxVerticalAngle
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        // Aplicar la rotación a la cámara (no al playerBody)
        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // Rotar el cuerpo del jugador solamente en el eje horizontal
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
