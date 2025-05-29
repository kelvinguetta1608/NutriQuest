using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static BarraNutricional;

public class BarraNutricional : MonoBehaviour
{
    public enum EstadoBarra { Ideal, FueraDeRango }

    private Image barra;

    [Range(0f, 1f)] public float valorActual;

    public EstadoBarra Estado { get; set; } = EstadoBarra.FueraDeRango;

    private Coroutine animacionActual;

    void Awake()
    {
        barra = GetComponent<Image>();
        if (barra == null)
        {
            Debug.LogError("No se encontró el componente Image en el GameObject de la barra nutricional.");
        }
    }

    void Update()
    {
        if (barra != null)
        {
            barra.color = (Estado == EstadoBarra.Ideal) ? Color.green : Color.red;
        }
    }

    public void ActualizarBarra(float nuevoValor)
    {
        valorActual = Mathf.Clamp01(nuevoValor);

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
        float valorInicial = barra.fillAmount;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float nuevoValor = Mathf.Lerp(valorInicial, valorFinal, tiempo / duracion);
            barra.fillAmount = nuevoValor;

            Debug.Log($"Barra rellenándose: {nuevoValor}");
            yield return null;
        }

        barra.fillAmount = valorFinal;
        animacionActual = null;

        Debug.Log($"Relleno final: {barra.fillAmount}");
    }
}

