using UnityEngine;
using UnityEngine.UI; // Para el componente Button
using TMPro; // Si tus paneles son TextMeshPro o quieres usarlo en Debug.Log

public class BarrasManager : MonoBehaviour
{
    [Header("Referencias a las Barras Nutricionales")]
    // Aquí arrastrarás todas las instancias de tus barras desde el Inspector
    public BarraNutricional[] barrasNutricionales;

    [Header("Paneles de Resultado")]
    public GameObject panelExito; // Panel para 8/9 o 9/9 barras en rango
    public GameObject panelFallo; // Panel para menos de 8 barras en rango

    [Header("Botón de Mezcla")]
    public Button botonMezclar; // Arrastra el botón de la escena aquí

    // Opcional: Para mostrar cuántas barras están en rango en el Editor
    [SerializeField] private int barrasEnRangoCount = 0;

    void Start()
    {
        // Asegúrate de que los paneles estén desactivados al inicio
        if (panelExito != null) panelExito.SetActive(false);
        if (panelFallo != null) panelFallo.SetActive(false);

        // Asigna la función ValidarEstadoBarras al botón de mezcla
        if (botonMezclar != null)
        {
            botonMezclar.onClick.AddListener(ValidarEstadoBarras);
        }
        else
        {
            Debug.LogError("Botón de Mezclar no asignado en BarrasManager.");
        }

        // Validación inicial para asegurarnos de que las barras estén asignadas
        if (barrasNutricionales == null || barrasNutricionales.Length == 0)
        {
            Debug.LogWarning("No se han asignado barras nutricionales a BarrasManager. Asegúrate de arrastrarlas en el Inspector.");
        }
    }

    /// <summary>
    /// Valida el estado actual de todas las barras nutricionales.
    /// Esta función será llamada por el botón de mezclar.
    /// </summary>
    public void ValidarEstadoBarras()
    {
        barrasEnRangoCount = 0; // Reinicia el contador

        if (barrasNutricionales == null || barrasNutricionales.Length == 0)
        {
            Debug.LogError("No hay barras nutricionales asignadas para validar.");
            return;
        }

        foreach (BarraNutricional barra in barrasNutricionales)
        {
            if (barra == null)
            {
                Debug.LogWarning("Una referencia de barra es nula en BarrasManager. Revisa tus asignaciones en el Inspector.");
                continue;
            }

            // Aquí es donde necesitamos saber el 'fillAmount' o 'valorActual' de la barra
            // y si está dentro de tu rango aceptado (70-95%)
            float valor = barra.valorActual; // Usamos valorActual directamente del script BarraNutricional

            // --- LÓGICA DE VALIDACIÓN DEL RANGO ---
            // Puedes ajustar estos umbrales según tu diseño final
            float rangoMinAceptado = 0.65f; // 60%
            float rangoMaxAceptado = 0.98f; // 95%

            if (valor >= rangoMinAceptado && valor <= rangoMaxAceptado)
            {
                barrasEnRangoCount++;
                Debug.Log($"Barra {barra.name} está en rango. Valor: {valor:F2}");
            }
            else
            {
                Debug.Log($"Barra {barra.name} FUERA DE RANGO. Valor: {valor:F2}. Rango ideal: {rangoMinAceptado:P0}-{rangoMaxAceptado:P0}");
            }
        }

        Debug.Log($"Total de barras en rango: {barrasEnRangoCount} de {barrasNutricionales.Length}");

        // Lógica para encender los paneles
        if (barrasEnRangoCount >= 7) // Si 8 o 9 barras están en rango
        {
            panelExito?.SetActive(true);
            panelFallo?.SetActive(false);
            Debug.Log("¡Batido Perfecto! Se activó el Panel de Éxito.");
        }
        else // Menos de 8 barras en rango
        {
            panelExito?.SetActive(false);
            panelFallo?.SetActive(true);
            Debug.Log("Batido Regular. Se activó el Panel de Fallo.");
        }
    }

    /// <summary>
    /// Método para resetear los paneles (útil si tienes un botón de "volver a intentar" o "vaciar licuadora")
    /// </summary>
    public void ResetPanels()
    {
        if (panelExito != null) panelExito.SetActive(false);
        if (panelFallo != null) panelFallo.SetActive(false);
        barrasEnRangoCount = 0; // Resetear el contador también
    }
}