using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CambiarEscenaSegunPuntos();
        }
    }

    private void CambiarEscenaSegunPuntos()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        GameData.TotalPuntos += 5;
        if (escenaActual == "EscenaLucas" && GameData.Puntos == 5)
        {
            SceneManager.LoadScene("EscenaJorge");
        }
        else if (escenaActual == "EscenaJorge" && GameData.Puntos == 10)
        {
            SceneManager.LoadScene("EscenaBonito");
        }
        else if (escenaActual == "EscenaBonito" && GameData.Puntos == 15)
        {
            SceneManager.LoadScene("EscenaSebastian");
        }
        else if (escenaActual == "EscenaSebastian" && GameData.Puntos == 20)
        {
            SceneManager.LoadScene("EscenaSoto");
        }
        else if (escenaActual == "EscenaSoto" && GameData.Puntos == 25)
        {
            SceneManager.LoadScene("Final");
        }
    }
}
