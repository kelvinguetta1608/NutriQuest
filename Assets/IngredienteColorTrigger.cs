using UnityEngine;
using System.Collections.Generic;

public class IngredienteColorTrigger : MonoBehaviour
{
    public Renderer liquidoRenderer; // El Renderer del jugo en la licuadora
    private List<Color> coloresIngredientes = new List<Color>();

    private Dictionary<string, Color> coloresPorTag = new Dictionary<string, Color>()
{
    { "Fresa", new Color(0.87f, 0.23f, 0.36f, 1f) },        // #DE3B5C
    { "Mango", new Color(1f, 0.69f, 0.13f, 1f) },           // #FFA022
    { "Banano", new Color(1f, 0.95f, 0.55f, 1f) },          // #FFF28C
    { "Manzana", new Color(0.85f, 0.15f, 0.2f, 1f) },       // #D92633
    { "Aguacate", new Color(0.42f, 0.65f, 0.32f, 1f) },     // #6BA652
    { "Piña", new Color(1f, 0.85f, 0.3f, 1f) },              // #FFD94D
    { "Uva morada", new Color(0.45f, 0.18f, 0.47f, 1f) },   // #742E78
    { "Naranja", new Color(1f, 0.5f, 0f, 1f) },              // #FF8000
    { "Guayaba", new Color(1f, 0.68f, 0.75f, 1f) },          // #FFAEBD
    { "Maracuya", new Color(1f, 0.94f, 0.34f, 1f) },         // #FFF054
    { "Papaya", new Color(1f, 0.6f, 0.2f, 1f) },             // #FF9933
    { "Apio", new Color(0.56f, 0.74f, 0.31f, 1f) },          // #8FB852
    { "Pimenton", new Color(1f, 0.25f, 0.25f, 1f) },         // #FF4040
    { "Pepino", new Color(0.6f, 0.8f, 0.5f, 1f) },           // #99CC80
    { "Zanahoria", new Color(1f, 0.55f, 0f, 1f) },           // #FF8C00
    { "Brocoli", new Color(0.31f, 0.59f, 0.27f, 1f) },       // #4F963B
    { "Avena", new Color(0.91f, 0.83f, 0.67f, 1f) },         // #E8D4AB
    { "Canela", new Color(0.58f, 0.29f, 0.05f, 1f) },        // #944A0B
    { "Mani", new Color(0.76f, 0.60f, 0.42f, 1f) },          // #C2996D
    { "Miel", new Color(1f, 0.8f, 0.2f, 1f) },               // #FFCC33
    { "Chia", new Color(0.11f, 0.11f, 0.11f, 1f) },          // #1C1C1C (casi negro)
    { "Linaza", new Color(0.67f, 0.5f, 0.33f, 1f) },         // #AA8053
    { "Nueces", new Color(0.55f, 0.36f, 0.14f, 1f) },        // #8C5E24
    { "Almendras", new Color(0.75f, 0.57f, 0.42f, 1f) },     // #BF916A
    { "Chocolate", new Color(0.36f, 0.22f, 0.12f, 1f) },      // #5C381A
        { "Agua", new Color(0.8f, 0.9f, 1f, 0.6f) },             // Azul muy claro y semitransparente
    { "Leche", new Color(1f, 1f, 0.95f, 1f) },               // Blanco ligeramente cremoso
    { "Leche de almendras", new Color(0.97f, 0.93f, 0.85f, 1f) },  // Blanco beige claro
    { "Yogurt", new Color(1f, 1f, 0.9f, 1f) },               // Blanco con toque amarillento
    { "Yogurt griego", new Color(1f, 1f, 0.87f, 1f) },       // Blanco con más crema
    { "Leche de coco", new Color(0.98f, 0.97f, 0.9f, 1f) }   // Blanco con un toque muy suave beige
};


    // Suscribirse al evento cuando el script está activo
    void OnEnable()
    {
        LicuadoraManager.OnIngredienteAgregado += HandleIngredienteAgregado;
    }

    // Desuscribirse del evento cuando el script se desactiva para evitar fugas de memoria
    void OnDisable()
    {
        LicuadoraManager.OnIngredienteAgregado -= HandleIngredienteAgregado;
    }

    // Este método se llamará automáticamente cuando LicuadoraManager.OnIngredienteAgregado se dispare
    private void HandleIngredienteAgregado(GameObject ingredienteGameObject)
    {
        // Asegúrate de que el ingrediente no sea nulo antes de procesarlo
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
                // Si tienes un material compartido, esto cambiará el color para todos los objetos que lo usen.
                // Si quieres que cada instancia de licuadora tenga su propio material único, podrías hacer:
                // liquidoRenderer.material = new Material(liquidoRenderer.material);
                liquidoRenderer.material.color = mezcla;
            }

            Debug.Log($"Ingrediente {tagDetectado} añadido. Color actual mezclado: {mezcla}");
        }
    }

    // ¡REMOVER EL OnTriggerEnter DE ESTE SCRIPT. YA NO ES NECESARIO AQUÍ!
    /*
    private void OnTriggerEnter(Collider other)
    {
        // Elimina o comenta este método por completo.
        // La lógica de cambio de color ahora se manejará a través del evento.
    }
    */

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