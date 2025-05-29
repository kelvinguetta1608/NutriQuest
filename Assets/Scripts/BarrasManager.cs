using UnityEngine;
using UnityEngine.UI; // Para el componente Button
using TMPro; // Para TextMeshProUGUI y para los paneles
using System.Collections.Generic; // Necesario para Dictionary

public class BarrasManager : MonoBehaviour
{
    [Header("Referencias a las Barras Nutricionales")]
    // Aquí arrastrarás todas las instancias de tus barras desde el Inspector
    public BarraNutricional[] barrasNutricionales;

    [Header("Paneles de Resultado")]
    public GameObject panelExito; // Panel para 7 o más barras en rango
    public GameObject panelFallo; // Panel para menos de 7 barras en rango

    [Header("Botón de Mezcla")]
    public Button botonMezclar; // Arrastra el botón de la escena aquí

    [Header("Retroalimentación de Mensajes")]
    public TextMeshProUGUI mensajeFeedbackText; // TextMeshPro donde aparecerán los mensajes
    public Image imagenColorBajo; // Imagen que cambiará de color según la barra más baja
    public GameObject objetoApagarAlMostrarMensajes; // Objeto que se apagará al iniciar los mensajes

    // Opcional: Para mostrar cuántas barras están en rango en el Editor
    [SerializeField] private int barrasEnRangoCount = 0;

    // Constantes para los rangos de validación del batido
    private const float RANGO_MIN_ACEPTADO = 0.65f; // 65%
    private const float RANGO_MAX_ACEPTADO = 0.98f; // 98%
    private const float UMBRAL_MUY_BAJO = 0.50f; // 50% para mensaje específico y color de imagen

    // --- NUEVAS VARIABLES PARA EL CONTROL DE ESTADO ---
    private bool alimentoAgregadoPorPrimeraVez = false;
    private float valorInicialTotalBarras = 0f; // Para detectar si las barras se han movido

    // Diccionario para mapear los sufijos de los nombres de GameObject a nombres legibles
    private Dictionary<string, string> nombreNutrienteMap;

    void Awake()
    {
        // Inicializa el diccionario de mapeo de nombres de nutrientes
        nombreNutrienteMap = new Dictionary<string, string>()
        {
            {"prot", "Proteínas"},
            {"omg3", "Omega-3"},
            {"mag", "Magnesio"},
            {"vit-b", "Vitamina B"},
            {"vit-c", "Vitamina C"},
            {"antiox", "Antioxidantes"},
            {"fibra", "Fibra"},
            {"cabH", "Carbohidratos"},
            {"hierro", "Hierro"}
        };

        // Calcular el valor inicial de todas las barras (debería ser 0 si todas inician en 0)
        // Esto es para detectar el "primer movimiento"
        foreach (BarraNutricional barra in barrasNutricionales)
        {
            if (barra != null)
            {
                valorInicialTotalBarras += barra.valorActual;
            }
        }
    }

    void Start()
    {
        // Asegúrate de que los paneles y el texto de feedback estén desactivados/vacíos al inicio
        if (panelExito != null) panelExito.SetActive(false);
        if (panelFallo != null) panelFallo.SetActive(false);
        if (mensajeFeedbackText != null) mensajeFeedbackText.text = "";

        // --- CONFIGURACIÓN INICIAL DE LA IMAGEN DE COLOR BAJO ---
        if (imagenColorBajo != null)
        {
            imagenColorBajo.gameObject.SetActive(true);
            imagenColorBajo.color = Color.white; // Color inicial: BLANCO
            Color tempColor = imagenColorBajo.color;
            tempColor.a = 1f; // Opacidad completa
            imagenColorBajo.color = tempColor;
        }

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

        // El objeto a apagar debería estar encendido al inicio por defecto
        if (objetoApagarAlMostrarMensajes != null)
        {
            objetoApagarAlMostrarMensajes.SetActive(true);
        }
    }

    /// <summary>
    /// Método público para notificar al BarrasManager cuando se ha agregado un ingrediente.
    /// Esto será llamado desde tu script de "añadir ingrediente".
    /// </summary>
    public void IngredienteAgregado()
    {
        // Calcular el valor actual total de las barras
        float valorActualTotalBarras = 0f;
        foreach (BarraNutricional barra in barrasNutricionales)
        {
            if (barra != null)
            {
                valorActualTotalBarras += barra.valorActual;
            }
        }

        // Si el valor total de las barras ha cambiado desde el inicio, significa que se ha añadido un alimento.
        if (valorActualTotalBarras > valorInicialTotalBarras)
        {
            // Solo la primera vez que se agrega un alimento, apagamos el objeto
            if (!alimentoAgregadoPorPrimeraVez)
            {
                alimentoAgregadoPorPrimeraVez = true;
                if (objetoApagarAlMostrarMensajes != null)
                {
                    objetoApagarAlMostrarMensajes.SetActive(false);
                    Debug.Log("Objeto especificado apagado al agregar el primer ingrediente: " + objetoApagarAlMostrarMensajes.name);
                }
            }
            // Generar el mensaje inicial de la barra más baja y cambiar el color de la imagen
            GenerarFeedbackEnTiempoReal();
        }
        else if (alimentoAgregadoPorPrimeraVez)
        {
            // Si ya se ha agregado al menos un alimento (pero el valor total no cambió mucho, quizás por un valor muy pequeño o redondeo),
            // aún así actualiza el feedback en tiempo real si ya está activado.
            GenerarFeedbackEnTiempoReal();
        }
    }


    /// <summary>
    /// Genera feedback de la barra más baja en tiempo real mientras se agregan ingredientes.
    /// </summary>
    private void GenerarFeedbackEnTiempoReal()
    {
        if (barrasNutricionales == null || barrasNutricionales.Length == 0) return;

        float valorMasBajo = 1.0f;
        string nombreNutrienteMasBajo = "";
        BarraNutricional barraMasBajaRef = null;

        foreach (BarraNutricional barra in barrasNutricionales)
        {
            if (barra == null) continue;

            float valor = barra.valorActual;
            // --- CÓDIGO PARA OBTENER EL NOMBRE DEL NUTRIENTE ---
            string nombreGameObject = barra.name;
            string sufijoNutriente = "";
            if (nombreGameObject.StartsWith("BA"))
            {
                sufijoNutriente = nombreGameObject.Substring(2); // Elimina "BA"
            }
            string nombreNutriente = nombreNutrienteMap.ContainsKey(sufijoNutriente) ? nombreNutrienteMap[sufijoNutriente] : sufijoNutriente;
            // --- FIN DEL CÓDIGO ---

            if (valor < valorMasBajo)
            {
                valorMasBajo = valor;
                nombreNutrienteMasBajo = nombreNutriente;
                barraMasBajaRef = barra;
            }
        }

        // Mostrar mensaje y color de la imagen de feedback
        if (barraMasBajaRef != null)
        {
            if (valorMasBajo < UMBRAL_MUY_BAJO) // Si la barra más baja está por debajo del 50%
            {
                mensajeFeedbackText.text = $"¡Ups! Estás bajo de **{nombreNutrienteMasBajo}** ({valorMasBajo:P0}).";
                if (imagenColorBajo != null)
                {
                    imagenColorBajo.color = barraMasBajaRef.ObtenerColorProgresivo(valorMasBajo);
                }
            }
            else if (valorMasBajo < RANGO_MIN_ACEPTADO) // Si está bajo pero no críticamente (entre 50% y 65%)
            {
                mensajeFeedbackText.text = $"Estás bajo de **{nombreNutrienteMasBajo}** ({valorMasBajo:P0}).";
                if (imagenColorBajo != null)
                {
                    imagenColorBajo.color = barraMasBajaRef.ObtenerColorProgresivo(valorMasBajo);
                }
            }
            else // Si la barra más baja está al menos en el 65% o más, pero el batido no es "perfecto" aún
            {
                mensajeFeedbackText.text = "¡Vas bien! Agrega más ingredientes para equilibrar.";
                if (imagenColorBajo != null)
                {
                    imagenColorBajo.color = barraMasBajaRef.ObtenerColorProgresivo(valorMasBajo);
                }
            }
        }
        else
        {
            mensajeFeedbackText.text = ""; // No hay mensaje si no hay barras válidas
            if (imagenColorBajo != null) imagenColorBajo.color = Color.white;
        }
    }


    /// <summary>
    /// Valida el estado actual de todas las barras nutricionales.
    /// Esta función será llamada por el botón de mezclar.
    /// </summary>
    public void ValidarEstadoBarras()
    {
        barrasEnRangoCount = 0; // Reinicia el contador
        mensajeFeedbackText.text = ""; // Limpia mensajes anteriores, ya que ahora se mostrará el resultado final

        // Ya el objeto se apagó con el primer ingrediente. No necesita apagarse de nuevo aquí.
        // if (objetoApagarAlMostrarMensajes != null) { objetoApagarAlMostrarMensajes.SetActive(false); }

        if (barrasNutricionales == null || barrasNutricionales.Length == 0)
        {
            Debug.LogError("No hay barras nutricionales asignadas para validar.");
            return;
        }

        // Variables para encontrar la barra más alta (para mensajes de exceso si aplica)
        float valorMasAlto = 0.0f;
        string nombreNutrienteMasAlto = "";

        // Primer paso: Recopilar información de todas las barras
        foreach (BarraNutricional barra in barrasNutricionales)
        {
            if (barra == null)
            {
                Debug.LogWarning("Una referencia de barra es nula en BarrasManager. Revisa tus asignaciones en el Inspector.");
                continue;
            }

            float valor = barra.valorActual;
            // --- CÓDIGO PARA OBTENER EL NOMBRE DEL NUTRIENTE ---
            string nombreGameObject = barra.name;
            string sufijoNutriente = "";
            if (nombreGameObject.StartsWith("BA"))
            {
                sufijoNutriente = nombreGameObject.Substring(2); // Elimina "BA"
            }
            string nombreNutriente = nombreNutrienteMap.ContainsKey(sufijoNutriente) ? nombreNutrienteMap[sufijoNutriente] : sufijoNutriente;
            // --- FIN DEL CÓDIGO ---

            // Contar barras en rango aceptado para la validación final del batido
            if (valor >= RANGO_MIN_ACEPTADO && valor <= RANGO_MAX_ACEPTADO)
            {
                barrasEnRangoCount++;
            }

            // Encontrar la barra con el valor más alto (potencialmente para exceso)
            if (valor > valorMasAlto)
            {
                valorMasAlto = valor;
                nombreNutrienteMasAlto = nombreNutriente;
            }
        }

        Debug.Log($"Total de barras en rango: {barrasEnRangoCount} de {barrasNutricionales.Length}");

        // --- Lógica de Retroalimentación de Mensajes Final y Activación de Paneles ---

        if (barrasEnRangoCount >= 7) // Si 7 o más barras están en rango
        {
            mensajeFeedbackText.text = "¡Felicidades! Has creado un batido nutricionalmente equilibrado. ¡Perfecto!";
            if (imagenColorBajo != null)
            {
                imagenColorBajo.color = barrasNutricionales[0].colorVerde; // Pone la imagen de color verde fijo
            }
            panelExito?.SetActive(true);
            panelFallo?.SetActive(false);
            Debug.Log("¡Batido Perfecto! Se activó el Panel de Éxito.");
        }
        else // Menos de 7 barras en rango, es un fallo o necesita mejoras
        {
            mensajeFeedbackText.text = "";

            // Encuentra la barra más baja para el feedback de fallo
            float valorMasBajoParaFallo = 1.0f;
            string nombreNutrienteMasBajoParaFallo = "";
            BarraNutricional barraMasBajaRefParaFallo = null;

            foreach (BarraNutricional barra in barrasNutricionales)
            {
                if (barra == null) continue;
                if (barra.valorActual < valorMasBajoParaFallo)
                {
                    valorMasBajoParaFallo = barra.valorActual;
                    // --- CÓDIGO PARA OBTENER EL NOMBRE DEL NUTRIENTE ---
                    string nombreGameObject = barra.name;
                    string sufijoNutriente = "";
                    if (nombreGameObject.StartsWith("BA"))
                    {
                        sufijoNutriente = nombreGameObject.Substring(2); // Elimina "BA"
                    }
                    nombreNutrienteMasBajoParaFallo = nombreNutrienteMap.ContainsKey(sufijoNutriente) ? nombreNutrienteMap[sufijoNutriente] : sufijoNutriente;
                    // --- FIN DEL CÓDIGO ---
                    barraMasBajaRefParaFallo = barra;
                }
            }

            // Mensaje principal de fallo
            if (barraMasBajaRefParaFallo != null)
            {
                mensajeFeedbackText.text = $"Tu batido no está en el punto óptimo. Todavía te falta **{nombreNutrienteMasBajoParaFallo}** ({valorMasBajoParaFallo:P0}).";
                if (imagenColorBajo != null)
                {
                    imagenColorBajo.color = barraMasBajaRefParaFallo.ObtenerColorProgresivo(valorMasBajoParaFallo);
                }
            }
            else
            {
                mensajeFeedbackText.text = "Tu batido no es óptimo. Asegúrate de añadir más ingredientes.";
                if (imagenColorBajo != null) imagenColorBajo.color = Color.white;
            }

            // Adicional: Mensaje si alguna barra está en exceso (solo para el mensaje de fallo final)
            if (valorMasAlto > RANGO_MAX_ACEPTADO)
            {
                mensajeFeedbackText.text += $"\n¡Cuidado! Te excediste en **{nombreNutrienteMasAlto}** ({valorMasAlto:P0}). Un batido balanceado es clave.";
            }

            panelExito?.SetActive(false);
            panelFallo?.SetActive(true);
            Debug.Log("Batido Regular. Se activó el Panel de Fallo.");
        }
    }

    /// <summary>
    /// Método para resetear los paneles y la retroalimentación (útil si tienes un botón de "volver a intentar" o "vaciar licuadora")
    /// </summary>
    public void ResetPanelsAndFeedback()
    {
        if (panelExito != null) panelExito.SetActive(false);
        if (panelFallo != null) panelFallo.SetActive(false);
        if (mensajeFeedbackText != null) mensajeFeedbackText.text = "";

        // Resetear la imagen a blanco al reiniciar
        if (imagenColorBajo != null)
        {
            imagenColorBajo.color = Color.white;
            Color tempColor = imagenColorBajo.color;
            tempColor.a = 1f;
            imagenColorBajo.color = tempColor;
        }

        // Asegurarse de volver a encender el objeto apagado
        if (objetoApagarAlMostrarMensajes != null)
        {
            objetoApagarAlMostrarMensajes.SetActive(true);
        }

        barrasEnRangoCount = 0; // Resetear el contador también
        alimentoAgregadoPorPrimeraVez = false; // Resetear para la próxima ronda

        // Recalcular valor inicial de las barras (debería ser 0 si tu sistema de juego las resetea)
        valorInicialTotalBarras = 0f;
        foreach (BarraNutricional barra in barrasNutricionales)
        {
            if (barra != null)
            {
                // Si tienes un método en BarraNutricional para resetear su valor, llámalo aquí.
                // Por ejemplo: barra.ResetValor();
                valorInicialTotalBarras += barra.valorActual;
            }
        }
    }
}