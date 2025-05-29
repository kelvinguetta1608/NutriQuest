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
        public float posicionNormalizada; // Valor de 0 a 1 para desplazamiento
    }

    public BotonIngrediente[] botones;
    public ScrollRect scrollRect;
    public float velocidadDesplazamiento = 5f;
    public bool calcularPosicionesAutomaticamente = false; // ✅ Nuevo: evita sobrescribir en Start

    private void Start()
    {
        if (calcularPosicionesAutomaticamente)
        {
            float paso = botones.Length > 1 ? 1f / (botones.Length - 1) : 0f;
            for (int i = 0; i < botones.Length; i++)
            {
                botones[i].posicionNormalizada = paso * i;
            }
        }

        for (int i = 0; i < botones.Length; i++)
        {
            int indexCapturado = i; // ✅ Corrección del índice
            botones[i].botonGO.GetComponent<Button>().onClick.AddListener(() => ActivarIngrediente(indexCapturado));
        }

        ActivarIngrediente(0);
    }

    void ActivarIngrediente(int indexSeleccionado)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].circuloNaranja.SetActive(i == indexSeleccionado);
        }

        StartCoroutine(DesplazarScrollX(botones[indexSeleccionado].posicionNormalizada));
    }

    IEnumerator DesplazarScrollX(float destinoNormalizado)
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
    }
}
