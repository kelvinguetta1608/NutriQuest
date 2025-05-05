using UnityEngine;
using System.Collections.Generic;

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

    // Llamado desde objetos arrastrables al soltarse dentro de la licuadora
    public void AgregarIngrediente(Ingrediente nuevo)
    {
        if (nuevo == null)
        {
            Debug.LogWarning("Ingrediente nulo recibido.");
            return;
        }

        ingredientesDentro.Add(nuevo);
        Debug.Log("Ingrediente agregado: " + nuevo.nombreIngrediente);

        CalcularYActualizarBarras();
    }

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
}
