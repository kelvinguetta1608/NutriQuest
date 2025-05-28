using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarraNutricional : MonoBehaviour
{
    public enum EstadoBarra { Bajo, Ideal, Alto }
    public EstadoBarra Estado = EstadoBarra.Bajo;

    public Image barra;
    [Range(0f, 1f)]
    public float valorActual;

    private Coroutine animacionActual;

    void Update()
    {
        switch (Estado)
        {
            case EstadoBarra.Bajo:
                barra.color = Color.red;
                break;
            case EstadoBarra.Ideal:
                barra.color = Color.green;
                break;
            case EstadoBarra.Alto:
                barra.color = new Color(0.5f, 0f, 0f); // Marrón oscuro
                break;
        }
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
        float duracion = 0.4f;
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
