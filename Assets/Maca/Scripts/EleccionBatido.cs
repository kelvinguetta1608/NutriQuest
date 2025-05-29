using UnityEngine;
using UnityEngine.SceneManagement;

public class EleccionBatido : MonoBehaviour
{
    public void SeleccionarNecesidad(string necesidad)
    {
        DatosCompartidos.necesidadSeleccionada = necesidad;
        SceneManager.LoadScene("Licuadora");
    }
}
