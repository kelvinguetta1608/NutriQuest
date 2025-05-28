

using UnityEngine;
using System.Collections.Generic;

public class IngredienteColorTrigger : MonoBehaviour
{
    public Renderer liquidoRenderer; // El Renderer del jugo en la licuadora
    private List<Color> coloresIngredientes = new List<Color>();

    private Dictionary<string, Color> coloresPorTag = new Dictionary<string, Color>()
    {
        { "Fresa", new Color(0.87f, 0.23f, 0.36f, 1f) },     // #DE3B5C
        { "Mango", new Color(1f, 0.69f, 0.13f, 1f) },         // #FFA022
        { "Banano", new Color(1f, 0.95f, 0.55f, 1f) },        // #FFF28C
        { "Manzana", new Color(0.85f, 0.15f, 0.2f, 1f) }      // #D92633
    };

    private void OnTriggerEnter(Collider other)
    {
        string tagDetectado = other.tag;

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
            liquidoRenderer.material.color = Color.clear;
    }
}
