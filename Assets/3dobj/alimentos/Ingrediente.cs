using UnityEngine;

public class Ingrediente : MonoBehaviour
{
    public string nombreIngrediente; // Ejemplo: "Mango", "Manzana", etc.
    public GameObject objetoDentroLicuadora; // Prefab para mostrar dentro de la licuadora

    // Material del ingrediente
    public Material materialIngrediente;

    // Porcentajes para cada barra (de 0 a 100)
    public float Kcal;
    [Range(0, 100)] public float carbohidratos;
    [Range(0, 100)] public float proteinas;
    [Range(0, 100)] public float hierro;
    [Range(0, 100)] public float vitaminaB;
    [Range(0, 100)] public float vitaminaC;
    [Range(0, 100)] public float magnesio;
    [Range(0, 100)] public float omega3;
    [Range(0, 100)] public float fibra;
    [Range(0, 100)] public float antioxidantes;

    [Header("Audio")]
    public AudioSource audioSource;      // AudioSource desde donde se reproducirá el sonido
    public AudioClip sonidoAlSeleccionar; // Clip que se reproducirá al hacer clic

    // Este método se puede vincular al botón en el Inspector
    public void ReproducirSonido()
    {
        if (audioSource != null && sonidoAlSeleccionar != null)
        {
            audioSource.PlayOneShot(sonidoAlSeleccionar);
        }
        else
        {
            Debug.LogWarning("Faltan asignaciones de AudioSource o AudioClip en el ingrediente: " + nombreIngrediente);
        }
    }
}
