using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarraNutricional : MonoBehaviour
{
    private Image barra;

    [Range(0f, 1f)] public float valorActual;

    private Coroutine animacionActual;

    // --- Colores base para la interpolación ---
    public Color colorRojo = new Color(0.635f, 0f, 0f, 1f);       // A20000 -> Convertido de Hex a RGB (R: 162/255, G: 0/255, B: 0/255)
    public Color colorAmarillo = new Color(1f, 0.533f, 0.2f, 1f); // FF8833 -> Convertido de Hex a RGB (R: 255/255, G: 136/255, B: 51/255)
    public Color colorAmarilloVerdoso = new Color(1f, 0.866f, 0.2f, 1f); // FFDD33 -> Convertido de Hex a RGB (R: 255/255, G: 221/255, B: 51/255)
    public Color colorVerde = new Color(0.2f, 1f, 0.2f, 1f);     // 33FF33 -> Ya era el mismo en tu código anterior

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

    public Color ObtenerColorProgresivo(float valor)
    {
        // El valor de entrada (valor) está entre 0 y 1 (0% a 100%)

        if (valor <= 0.25f) // Menor o igual a 25% (Rojo original A20000 a Amarillo FF8833)
        {
            return Color.Lerp(colorRojo, colorAmarillo, valor / 0.25f);
        }
        else if (valor <= 0.40f) // Entre 25% y 40% (Amarillo FF8833 a Amarillo Verdoso FFDD33)
        {
            return Color.Lerp(colorAmarillo, colorAmarilloVerdoso, (valor - 0.25f) / 0.15f); // 0.40 - 0.25 = 0.15
        }
        else if (valor <= 0.65f) // Entre 40% y 60% (Amarillo Verdoso FFDD33 a Verde 33FF33)
        {
            // Ajusté el rango para la interpolación para que 0.60 sea el punto final para alcanzar el verde.
            // Si el verde "ideal" es de 70-95, el 60% ya debería ser un verde claro.
            // La interpolación va de 0.40 a 0.60, por lo que el divisor es 0.20f
            return Color.Lerp(colorAmarilloVerdoso, colorVerde, (valor - 0.40f) / 0.20f);
        }
        else if (valor <= 0.98f) // Entre 60% y 98% (Verde puro)
        {
            // Este es tu rango "óptimo" más amplio donde la barra se mantiene verde.
            // Antes tenías 0.95f, ahora 0.98f, lo cual está bien si quieres una ventana de éxito más grande.
            return colorVerde;
        }
        else // De 98% a 100% (Verde a Rojo final A20000 para indicar exceso)
        {
            // La interpolación de verde a rojo ahora ocurre en el último 2% (0.98f a 1.0f).
            return Color.Lerp(colorVerde, colorRojo, (valor - 0.98f) / 0.02f); // 1.0 - 0.98 = 0.02
        }
    }
}