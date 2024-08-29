using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Iniciar()
    {
        SceneManager.LoadScene("EscenaLucas");
    }
    
    public void EscenaLucas()
    {
        SceneManager.LoadScene("EscenaLucas");
    }
    public void Cerrar()
    {
        Application.Quit();
    }
}
