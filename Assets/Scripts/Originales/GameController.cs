using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    public Text txtVida;
    public Text txtPuntos;
    public GameObject portal;
    public Text portalMessage;
    public Text mensajeTemporal;

    private const int vidaMaxima = 3;

    void Start()
    {
        ActualizarTxtVida();
        ActualizarTxtPuntos();

        if (portal != null)
        {
            portal.SetActive(false);
        }

        if (portalMessage != null)
        {
            portalMessage.gameObject.SetActive(false);
        }

        if (mensajeTemporal != null)
        {
            mensajeTemporal.gameObject.SetActive(false);
        }
    }

    public void SumarVida(int cantidad)
    {
        GameData.Vida += cantidad;
        if (GameData.Vida > vidaMaxima)
        {
            GameData.Vida = vidaMaxima;
        }
        ActualizarTxtVida();
    }

    public int GetVida()
    {
        return GameData.Vida;
    }

    public int GetPuntos()
        {
            return GameData.Puntos;
        }

    public void RestarVida(int cantidad)
    {
        GameData.Vida -= cantidad;
        if (GameData.Vida < 1)
        {
            GameData.Vida = 0;
            MostrarMensajeTemporal("Has Muerto.. Volviendo al menu en");

            StartCoroutine(ContadorRegresivo(5, () =>
            {
                GameData.ReiniciarDatos(); // Reiniciar datos del juego aquí
                SceneManager.LoadScene("Menu");
            }));

            Debug.Log("Has Muerto");
        }
        ActualizarTxtVida();
    }

    private IEnumerator ContadorRegresivo(int segundos, Action callback)
    {
        int tiempoRestante = segundos;
        while (tiempoRestante > 0)
        {
            MostrarMensajeTemporal($"Has Muerto.. Volviendo al menu en {tiempoRestante}");
            yield return new WaitForSeconds(1);
            tiempoRestante--;
        }

        callback?.Invoke();
    }

    public void SumarPuntos(int cantidad)
    {
        GameData.Puntos += cantidad;
        ActualizarTxtPuntos();
        Debug.Log(GameData.Puntos);
        string escenaActual = SceneManager.GetActiveScene().name;

        if (GameData.Puntos >= 5)
        {
            if (escenaActual == "EscenaLucas" && GameData.Puntos == 5)
            {
                ActivarPortal("¡Portal activado! Ve al portal para cambiar de escena.");
            }
            else if (escenaActual == "EscenaJorge" && GameData.Puntos == 10)
            {
                ActivarPortal("¡Portal activado! Ve al portal para cambiar de escena.");
            }
            else if (escenaActual == "EscenaBonito" && GameData.Puntos == 15)
            {
                ActivarPortal("¡Portal activado! Ve al portal para cambiar de escena.");
            }
            else if (escenaActual == "EscenaSebastian" && GameData.Puntos == 20)
            {
                ActivarPortal("¡Portal activado! Ve al portal para cambiar de escena.");
            }
            else if (escenaActual == "EscenaSoto" && GameData.Puntos == 25)
            {
                ActivarPortal("¡Portal activado! Ve al portal para cambiar de escena.");
            }
        }
    }

    private void ActualizarTxtVida()
    {
        txtVida.text = "Vida: " + GameData.Vida.ToString();
    }

    private void ActualizarTxtPuntos()
    {
        txtPuntos.text = "Puntos: " + GameData.Puntos.ToString() + " / " + GameData.TotalPuntos.ToString();
    }

    private void MostrarMensajeTemporal(string mensaje)
    {
        if (mensajeTemporal != null)
        {
            mensajeTemporal.text = mensaje;
            mensajeTemporal.gameObject.SetActive(true);
            StartCoroutine(DesvanecerMensajeMuerte());
        }
    }

    private void MostrarMensajePortal(string mensaje)
    {
        if (portalMessage != null)
        {
            portalMessage.text = mensaje;
            portalMessage.gameObject.SetActive(true);
            StartCoroutine(DesvanecerMensajePortal());
        }
    }

    private IEnumerator DesvanecerMensajePortal()
    {
        yield return new WaitForSeconds(2);

        Color originalColor = portalMessage.color;
        float fadeDuration = 1.0f;
        float fadeSpeed = 1.0f / fadeDuration;

        for (float t = 0; t < 1; t += Time.deltaTime * fadeSpeed)
        {
            portalMessage.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        portalMessage.gameObject.SetActive(false);
        portalMessage.color = originalColor;
    }

    private IEnumerator DesvanecerMensajeMuerte()
    {
        yield return new WaitForSeconds(1);

        Color originalColor = mensajeTemporal.color;
        float fadeDuration = 1.0f;
        float fadeSpeed = 1.0f / fadeDuration;

        for (float t = 0; t < 1; t += Time.deltaTime * fadeSpeed)
        {
            mensajeTemporal.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        mensajeTemporal.gameObject.SetActive(false);
        mensajeTemporal.color = originalColor;
    }

    public void CambiarEscena(string nombreEscena)
    {
        // Incrementa el total de puntos en cada cambio de escena
        GameData.TotalPuntos += 5;
        SceneManager.LoadScene(nombreEscena);
    }

    private void ActivarPortal(string mensaje)
    {
        if (portal != null)
        {
            portal.SetActive(true);
        }
        MostrarMensajePortal(mensaje);
    }
}
