using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNecesidadUI : MonoBehaviour
{
    public void SeleccionarNecesidad(string necesidad)
    {
        DatosCompartidos.necesidadSeleccionada = necesidad;
        SceneManager.LoadScene("Licuadora");
    }
}
