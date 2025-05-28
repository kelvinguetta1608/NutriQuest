using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNecesidad : MonoBehaviour
{
    public NecesidadNutricional[] opcionesNecesidad;

    public void SeleccionarNecesidad(int index)
    {
        var necesidad = opcionesNecesidad[index];
        Debug.Log("🚀 Necesidad seleccionada desde botón: " + necesidad.nombre);

        if (ReferenciaDatos.instancia == null)
        {
            Debug.LogError("💥 ReferenciaDatos.instancia es NULL. No se asignó correctamente.");
        }
        else
        {
            ReferenciaDatos.instancia.AsignarNecesidad(necesidad);
            Debug.Log("✅ Necesidad asignada en persistente: " + ReferenciaDatos.instancia.necesidadSeleccionada.nombre);
        }

        Debug.Log("Modo RA: " + ModoJuego.usarRA);

        if (ModoJuego.usarRA)
            SceneManager.LoadScene("AR");
        else
            SceneManager.LoadScene("Licuadora");
    }
}
