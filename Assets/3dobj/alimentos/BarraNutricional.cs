using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarraNutricional : MonoBehaviour
{
    private Image barra;

    [Range(0f, 1f)] public float valorActual;

    private Coroutine animacionActual;

    // --- Colores base para la interpolación ---
    public Color colorRojo = new Color(1f, 0.2f, 0.2f, 1f); // Rojo vibrante
    public Color colorAmarillo = new Color(1f, 0.9f, 0.2f, 1f); // Amarillo
    public Color colorAmarilloVerdoso = new Color(0.7f, 1f, 0.2f, 1f); // Amarillo-verdoso
    public Color colorVerde = new Color(0.2f, 1f, 0.2f, 1f); // Verde

    void Awake()
    {
        barra = GetComponent<Image>();
        if (barra == null)
        {
            Debug.LogError("No se encontró el componente Image en el GameObject de la barra nutricional.");
        }
    }

    // El Update() se encargará de mantener el color correcto basado en el valor actual
    void Update()
    {
        if (barra != null)
        {
            barra.color = ObtenerColorProgresivo(barra.fillAmount); // Usa fillAmount para el color mientras anima
        }
    }

    public void ActualizarBarra(float nuevoValor)
    {
        valorActual = Mathf.Clamp01(nuevoValor); // Asegura que el valor esté entre 0 y 1

        Debug.Log($"[BarraNutricional] Valor recibido: {nuevoValor}, Fill final: {valorActual}");

        if (animacionActual != null)
            StopCoroutine(animacionActual);

        if (barra != null)
            animacionActual = StartCoroutine(AnimarCambioBarra(valorActual));
    }

    private IEnumerator AnimarCambioBarra(float valorFinal)
    {
        float duracion = 0.4f;
        float tiempo = 0f;
        float valorInicial = barra.fillAmount; // Inicia la animación desde el fillAmount actual

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float nuevoValorAnimado = Mathf.Lerp(valorInicial, valorFinal, tiempo / duracion);
            barra.fillAmount = nuevoValorAnimado;

            // El Update() se encargará de cambiar el color en cada frame de la animación.
            yield return null;
        }

        barra.fillAmount = valorFinal; // Asegura que el valor final sea exacto
        animacionActual = null;

        Debug.Log($"Relleno final: {barra.fillAmount}");
    }

    private Color ObtenerColorProgresivo(float valor)
    {
        // El valor de entrada (valor) está entre 0 y 1 (0% a 100%)

        if (valor <= 0.25f) // Menor o igual a 25% (Rojo a Amarillo)
        {
            // Interpola de Rojo (0%) a Amarillo (25%)
            return Color.Lerp(colorRojo, colorAmarillo, valor / 0.25f);
        }
        else if (valor <= 0.50f) // Entre 25% y 50% (Amarillo a Amarillo Verdoso)
        {
            // Interpola de Amarillo (25%) a Amarillo Verdoso (50%)
            // Normaliza el valor dentro de este rango (0.25 a 0.50) a un rango de 0 a 1
            return Color.Lerp(colorAmarillo, colorAmarilloVerdoso, (valor - 0.25f) / 0.25f);
        }
        else if (valor <= 0.79f) // Entre 50% y 79% (Amarillo Verdoso a Verde)
        {
            // Interpola de Amarillo Verdoso (50%) a Verde (79%)
            // Normaliza el valor dentro de este rango (0.50 a 0.79) a un rango de 0 a 1
            return Color.Lerp(colorAmarilloVerdoso, colorVerde, (valor - 0.50f) / 0.29f); // 0.79 - 0.50 = 0.29
        }
        else if (valor <= 0.95f) // Entre 80% y 95% (Verde puro)
        {
            // Para mantener el verde puro en este rango, no hay interpolación.
            // Si quisieras una transición suave de verde a rojo aquí, usarías otro Lerp.
            // Por simplicidad, se mantiene verde puro en este rango.
            return colorVerde;
        }
        else // De 96% a 100% (Rojo)
        {
            // Interpola de Verde (95%) a Rojo (100%) para indicar exceso
            // Normaliza el valor dentro de este rango (0.95 a 1.0) a un rango de 0 a 1
            return Color.Lerp(colorVerde, colorRojo, (valor - 0.95f) / 0.05f); // 1.0 - 0.95 = 0.05
        }
    }
}