using UnityEngine;

public class ReferenciaDatos : MonoBehaviour
{
    public NecesidadNutricional necesidadSeleccionada;

    public static ReferenciaDatos instancia;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AsignarNecesidad(NecesidadNutricional necesidad)
    {
        necesidadSeleccionada = necesidad;
    }

    public NecesidadNutricional ObtenerNecesidad()
    {
        return necesidadSeleccionada;
    }
}
