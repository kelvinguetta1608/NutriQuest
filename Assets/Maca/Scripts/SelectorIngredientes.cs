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

        [Range(0f, 1f)]
        public float posicionNormalizada;
    }

    public BotonIngrediente[] botones;
    public ScrollRect scrollRect;
    public float velocidadDesplazamiento = 5f;

    private void Start()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            int indexCapturado = i;
            botones[i].botonGO.GetComponent<Button>().onClick.AddListener(() => ActivarIngrediente(indexCapturado));
        }

        ActivarIngrediente(0);
    }

    void ActivarIngrediente(int indexSeleccionado)
    {
        // Desactiva todos los círculos
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].circuloNaranja.SetActive(false);
        }

        // Inicia desplazamiento y enciende imagen correcta después
        StartCoroutine(DesplazarScrollYActivarImg(botones[indexSeleccionado].posicionNormalizada));
    }

    IEnumerator DesplazarScrollYActivarImg(float destinoNormalizado)
    {
        float inicio = scrollRect.horizontalNormalizedPosition;
        float tiempo = 0f;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadDesplazamiento;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(inicio, destinoNormalizado, tiempo);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = destinoNormalizado;

        // Luego de desplazarse, activamos la imagen adecuada
        ActivarImagenPorPosicion(destinoNormalizado);
    }

    void ActivarImagenPorPosicion(float pos)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].circuloNaranja.SetActive(false);
        }

        int index = 0;

        if (pos >= 0f && pos < 0.32f)
            index = 0;
        else if (pos >= 0.32f && pos < 0.49f)
            index = 1;
        else if (pos >= 0.49f && pos < 0.79f)
            index = 2;
        else if (pos >= 0.79f && pos < 0.88f)
            index = 3;
        else if (pos >= 0.88f && pos <= 1f)
            index = 4;

        if (index >= 0 && index < botones.Length)
        {
            botones[index].circuloNaranja.SetActive(true);
        }
    }

    private void Update()
    {
        // Si quieres que esto se actualice también con swipe manual:
        ActivarImagenPorPosicion(scrollRect.horizontalNormalizedPosition);
    }
}
