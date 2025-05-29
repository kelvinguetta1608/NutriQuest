using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorIngredientes : MonoBehaviour
{
    [System.Serializable]
    public class BotonIngrediente
    {
        public GameObject botonGO;
        public GameObject circuloNaranja;
        public float posicionXObjetivo; // Posición X deseada del Content
    }

    public BotonIngrediente[] botones;
    public ScrollRect scrollRect;
    public float velocidadDesplazamiento = 5f;

    private void Start()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            int index = i;
            botones[i].botonGO.GetComponent<Button>().onClick.AddListener(() => ActivarIngrediente(index));
        }

        ActivarIngrediente(0);
    }

    void ActivarIngrediente(int indexSeleccionado)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].circuloNaranja.SetActive(i == indexSeleccionado);
        }

        StartCoroutine(DesplazarScrollX(botones[indexSeleccionado].posicionXObjetivo));
    }

    IEnumerator DesplazarScrollX(float destinoX)
    {
        RectTransform content = scrollRect.content;
        Vector2 posicionInicial = content.anchoredPosition;
        Vector2 destino = new Vector2(destinoX, posicionInicial.y);
        float tiempo = 0f;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadDesplazamiento;
            content.anchoredPosition = Vector2.Lerp(posicionInicial, destino, tiempo);
            yield return null;
        }

        content.anchoredPosition = destino;
    }
}
