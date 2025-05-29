using UnityEngine;
using System.Collections.Generic;

public class IngredienteColorTrigger : MonoBehaviour
{
    public Renderer liquidoRenderer; // El Renderer del jugo en la licuadora
    public AudioSource audioSource; // AudioSource asignado desde el Inspector
    public AudioClip sonidoIngrediente; // Sonido a reproducir cuando se agrega un ingrediente

    private List<Color> coloresIngredientes = new List<Color>();

    private Dictionary<string, Color> coloresPorTag = new Dictionary<string, Color>()
    {
        { "Fresa", new Color(0.87f, 0.23f, 0.36f, 1f) },
        { "Mango", new Color(1f, 0.69f, 0.13f, 1f) },
        { "Banano", new Color(1f, 0.95f, 0.55f, 1f) },
        { "Manzana", new Color(0.85f, 0.15f, 0.2f, 1f) },
        { "Aguacate", new Color(0.42f, 0.65f, 0.32f, 1f) },
        { "Piña", new Color(1f, 0.85f, 0.3f, 1f) },
        { "Uva morada", new Color(0.45f, 0.18f, 0.47f, 1f) },
        { "Naranja", new Color(1f, 0.5f, 0f, 1f) },
        { "Guayaba", new Color(1f, 0.68f, 0.75f, 1f) },
        { "Maracuya", new Color(1f, 0.94f, 0.34f, 1f) },
        { "Papaya", new Color(1f, 0.6f, 0.2f, 1f) },
        { "Apio", new Color(0.56f, 0.74f, 0.31f, 1f) },
        { "Pimenton", new Color(1f, 0.25f, 0.25f, 1f) },
        { "Pepino", new Color(0.6f, 0.8f, 0.5f, 1f) },
        { "Zanahoria", new Color(1f, 0.55f, 0f, 1f) },
        { "Brocoli", new Color(0.31f, 0.59f, 0.27f, 1f) },
        { "Avena", new Color(0.91f, 0.83f, 0.67f, 1f) },
        { "Canela", new Color(0.58f, 0.29f, 0.05f, 1f) },
        { "Mani", new Color(0.76f, 0.60f, 0.42f, 1f) },
        { "Miel", new Color(1f, 0.8f, 0.2f, 1f) },
        { "Chia", new Color(0.11f, 0.11f, 0.11f, 1f) },
        { "Linaza", new Color(0.67f, 0.5f, 0.33f, 1f) },
        { "Nueces", new Color(0.55f, 0.36f, 0.14f, 1f) },
        { "Almendras", new Color(0.75f, 0.57f, 0.42f, 1f) },
        { "Chocolate", new Color(0.36f, 0.22f, 0.12f, 1f) },
        { "Agua", new Color(0.8f, 0.9f, 1f, 0.6f) },
        { "Leche", new Color(1f, 1f, 0.95f, 1f) },
        { "Leche de almendras", new Color(0.97f, 0.93f, 0.85f, 1f) },
        { "Yogurt", new Color(1f, 1f, 0.9f, 1f) },
        { "Yogurt griego", new Color(1f, 1f, 0.87f, 1f) },
        { "Leche de coco", new Color(0.98f, 0.97f, 0.9f, 1f) }
    };

    void OnEnable()
    {
        LicuadoraManager.OnIngredienteAgregado += HandleIngredienteAgregado;
    }

    void OnDisable()
    {
        LicuadoraManager.OnIngredienteAgregado -= HandleIngredienteAgregado;
    }

    private void HandleIngredienteAgregado(GameObject ingredienteGameObject)
    {
        if (ingredienteGameObject == null)
        {
            Debug.LogWarning("Evento de ingrediente agregado recibido, pero el GameObject es nulo.");
            return;
        }

        string tagDetectado = ingredienteGameObject.tag;

        if (coloresPorTag.ContainsKey(tagDetectado))
        {
            Color color = coloresPorTag[tagDetectado];
            coloresIngredientes.Add(color);

            // Mezclar colores actuales
            Color mezcla = CalcularColorPromedio(coloresIngredientes);

            // Asignar al material de la licuadora
            if (liquidoRenderer != null)
            {
                liquidoRenderer.material.color = mezcla;
            }

            // Reproducir sonido al agregar ingrediente
            if (audioSource != null && sonidoIngrediente != null)
            {
                audioSource.PlayOneShot(sonidoIngrediente);
            }

            // ? Vibrar el dispositivo
            Handheld.Vibrate();

            Debug.Log($"Ingrediente {tagDetectado} añadido. Color actual mezclado: {mezcla}");
        }
    }

    private Color CalcularColorPromedio(List<Color> colores)
    {
        if (colores.Count == 0) return Color.clear;

        float r = 0f, g = 0f, b = 0f;
        foreach (Color c in colores)
        {
            r += c.r;
            g += c.g;
            b += c.b;
        }

        return new Color(r / colores.Count, g / colores.Count, b / colores.Count, 1f);
    }

    public void ReiniciarMezcla()
    {
        coloresIngredientes.Clear();
        if (liquidoRenderer != null)
        {
            liquidoRenderer.material.color = Color.clear;
        }
    }
}
