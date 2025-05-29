using UnityEngine;
using System.Collections.Generic;
using TMPro; // ¡Importante! Necesitas este namespace para TextMeshPro

public class LicuadoraManager : MonoBehaviour
{
    // Lista de ingredientes que están dentro de la licuadora
    public List<Ingrediente> ingredientesDentro = new List<Ingrediente>();

    // Referencias a las barras nutricionales en la UI
    public BarraNutricional barraHierro;
    public BarraNutricional barraVitaminaC;
    public BarraNutricional barraOmega3;
    public BarraNutricional barraProteina;
    public BarraNutricional barraVitaminaB;
    public BarraNutricional barraFibra;
    public BarraNutricional barraAntiOx;
    public BarraNutricional barraCarboH;
    public BarraNutricional barraMag;

    // --- NUEVO: REFERENCIA A TEXTMESHPRO Y CALORÍAS TOTALES ---
    public TextMeshProUGUI caloriasTotalesText; // Asigna esto en el Inspector
    private float totalCalorias = 0f;
    // -----------------------------------------------------------

    public static event System.Action<GameObject> OnIngredienteAgregado;

    // Llamado desde objetos arrastrables al soltarse dentro de la licuadora
    public void AgregarIngrediente(Ingrediente nuevo)
    {
        if (nuevo == null)
        {
            Debug.LogWarning("Ingrediente nulo recibido en LicuadoraManager.");
            return;
        }

        ingredientesDentro.Add(nuevo);
        Debug.Log("Ingrediente agregado al inventario de la licuadora: " + nuevo.name);

        // --- NUEVO: Sumar Kcal y actualizar texto ---
        totalCalorias += nuevo.Kcal; // Suma las calorías del nuevo ingrediente
        ActualizarCaloriasEnUI();    // Llama al método para actualizar el TextMeshPro
        // ---------------------------------------------

        // Llama al evento, pasando el GameObject del ingrediente que se acaba de añadir.
        if (OnIngredienteAgregado != null)
        {
            OnIngredienteAgregado(nuevo.gameObject);
        }

        CalcularYActualizarBarras();
    }

    // --- NUEVO MÉTODO: Para actualizar el texto de calorías ---
    private void ActualizarCaloriasEnUI()
    {
        if (caloriasTotalesText != null)
        {
            // Formatea el texto para mostrar las calorías. Puedes ajustar el formato si quieres.
            caloriasTotalesText.text = totalCalorias.ToString("F0");
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI 'caloriasTotalesText' no asignado en LicuadoraManager.");
        }
    }
    // ---------------------------------------------------------

    // Calcula los totales de nutrientes y actualiza las barras de progreso
    private void CalcularYActualizarBarras()
    {
        float totalHierro = 0f;
        float totalVitC = 0f;
        float totalOmega3 = 0f;
        float totalProteina = 0f;
        float totalMag = 0f;
        float totalVitaminaB = 0f;
        float totalFibra = 0f;
        float totalAntiOx = 0f;
        float totalCarboH = 0f;

        foreach (Ingrediente ing in ingredientesDentro)
        {
            totalHierro += ing.hierro;
            totalVitC += ing.vitaminaC;
            totalOmega3 += ing.omega3;
            totalProteina += ing.proteinas;
            totalMag += ing.magnesio;
            totalVitaminaB += ing.vitaminaB;
            totalFibra += ing.fibra;
            totalAntiOx += ing.antioxidantes;
            totalCarboH += ing.carbohidratos;
        }

        // Suponiendo que 100 es el valor ideal para llenar la barra (100%)
        barraHierro?.ActualizarBarra(totalHierro / 100f);
        barraVitaminaC?.ActualizarBarra(totalVitC / 100f);
        barraOmega3?.ActualizarBarra(totalOmega3 / 100f);
        barraProteina?.ActualizarBarra(totalProteina / 100f);
        barraMag?.ActualizarBarra(totalMag / 100f);
        barraVitaminaB?.ActualizarBarra(totalVitaminaB / 100f);
        barraFibra?.ActualizarBarra(totalFibra / 100f);
        barraAntiOx?.ActualizarBarra(totalAntiOx / 100f);
        barraCarboH?.ActualizarBarra(totalCarboH / 100f);
    }

    // Vaciar la licuadora y reiniciar barras
    public void VaciarLicuadora()
    {
        ingredientesDentro.Clear();

        // --- NUEVO: Reiniciar calorías al vaciar ---
        totalCalorias = 0f;
        ActualizarCaloriasEnUI(); // Actualiza el texto a 0
        // ---------------------------------------------

        barraHierro?.ActualizarBarra(0f);
        barraVitaminaC?.ActualizarBarra(0f);
        barraOmega3?.ActualizarBarra(0f);
        barraProteina?.ActualizarBarra(0f);
        barraMag?.ActualizarBarra(0f);
        barraVitaminaB?.ActualizarBarra(0f);
        barraFibra?.ActualizarBarra(0f);
        barraAntiOx?.ActualizarBarra(0f);
        barraCarboH?.ActualizarBarra(0f);

        Debug.Log("Licuadora vaciada.");
    }

    // Asegúrate de llamar a esto una vez al inicio para mostrar "0 Kcal"
    void Start()
    {
        ActualizarCaloriasEnUI();
    }
}