using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarraNutricional : MonoBehaviour
{
    public Image barra; // Imagen de la barra
    [Range(0f, 1f)]
    public float valorActual; // Valor objetivo (entre 0.0 y 1.0)

    private Coroutine animacionActual;

    void Update()
    {
        barra.color = ObtenerColorSegunValor(barra.fillAmount); // Color según el valor visible, no el objetivo
    }

    Color ObtenerColorSegunValor(float valor)
    {
        if (valor < 0.3f)
            return Color.red;
        else if (valor >= 0.3f && valor <= 0.5f)
            return new Color(1f, 0.5f, 0f); // Naranja
        else if (valor > 0.5f && valor < 0.8f)
            return new Color(0.8f, 1f, 0f); // Amarillo verdoso
        else if (valor >= 0.8f && valor <= 0.95f)
            return Color.green;
        else
            return new Color(0.5f, 0f, 0f); // Marrón oscuro para "exceso"
    }

    public void ActualizarBarra(float nuevoValor)
    {
        valorActual = Mathf.Clamp(nuevoValor, 0f, 1f);
        if (animacionActual != null)
            StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarCambioBarra(valorActual));
    }

    private IEnumerator AnimarCambioBarra(float valorFinal)
    {
        float duracion = 0.4f; // Duración total de la animación
        float tiempo = 0f;
        float valorInicial = barra.fillAmount;

        while (tiempo < duracion)
        {
            barra.fillAmount = Mathf.Lerp(valorInicial, valorFinal, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        barra.fillAmount = valorFinal;
        animacionActual = null;
    }
}
