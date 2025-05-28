using UnityEngine;
using UnityEngine.SceneManagement;

public class SeleccionarModo : MonoBehaviour
{
    public bool esRA;

    public void ElegirModo()
    {
        ModoJuego.usarRA = esRA;
        Debug.Log("Modo RA seleccionado: " + ModoJuego.usarRA);
        SceneManager.LoadScene("EleccionBatido");
    }
}
