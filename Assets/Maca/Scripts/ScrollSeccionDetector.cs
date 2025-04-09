using UnityEngine;
using UnityEngine.UI;

public class ScrollSeccionDetector : MonoBehaviour
{
    public RectTransform content; // Asigna el Content del ScrollRect
    public GameObject[] circulosNaranjas; // Los circulitos naranjas de cada botón

    // Estas son las posiciones en X que tomaste de cada sección
    private float[] posicionesSecciones = new float[] { -199.99f, -400f, -629f, -862f, -1097f };
    private int seccionActual = -1;

    void Update()
    {
        float posX = content.anchoredPosition.x;
        int seccionMasCercana = 0;
        float distanciaMenor = Mathf.Infinity;

        for (int i = 0; i < posicionesSecciones.Length; i++)
        {
            float distancia = Mathf.Abs(posX - posicionesSecciones[i]);
            if (distancia < distanciaMenor)
            {
                distanciaMenor = distancia;
                seccionMasCercana = i;
            }
        }

        if (seccionMasCercana != seccionActual)
        {
            seccionActual = seccionMasCercana;
            ActualizarCirculos(seccionActual);
        }
    }

    void ActualizarCirculos(int indexActivo)
    {
        for (int i = 0; i < circulosNaranjas.Length; i++)
        {
            circulosNaranjas[i].SetActive(i == indexActivo);
        }
    }
}
