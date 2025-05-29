using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverEscenaJuego : MonoBehaviour
{
    // public string nombreEscena;

    public void CargarNuevaEscena()
    {
        SceneManager.LoadScene("inicioMenu");
    }
}
