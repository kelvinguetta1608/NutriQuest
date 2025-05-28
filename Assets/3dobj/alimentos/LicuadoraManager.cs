using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LicuadoraManager : MonoBehaviour
{
    public List<Ingrediente> ingredientesDentro = new List<Ingrediente>();

    public BarraNutricional barraHierro;
    public BarraNutricional barraVitaminaC;
    public BarraNutricional barraOmega3;
    public BarraNutricional barraProteina;
    public BarraNutricional barraVitaminaB;
    public BarraNutricional barraFibra;
    public BarraNutricional barraAntiOx;
    public BarraNutricional barraCarboH;
    public BarraNutricional barraMag;

    public NecesidadNutricional necesidad; // Asignada desde otro script
    public TextMeshProUGUI mensajeResultado; // Mensaje de retroalimentación al usuario

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

    private void CalcularYActualizarBarras()
    {
        float hierro = 0, vitC = 0, omega3 = 0, proteinas = 0, magnesio = 0;
        float vitaminaB = 0, fibra = 0, antioxidantes = 0, carbohidratos = 0;

        foreach (Ingrediente ing in ingredientesDentro)
        {
            hierro += ing.hierro;
            vitC += ing.vitaminaC;
            omega3 += ing.omega3;
            proteinas += ing.proteinas;
            magnesio += ing.magnesio;
            vitaminaB += ing.vitaminaB;
            fibra += ing.fibra;
            antioxidantes += ing.antioxidantes;
            carbohidratos += ing.carbohidratos;
        }

        ActualizarBarra(barraHierro, hierro, necesidad.hierro);
        ActualizarBarra(barraVitaminaC, vitC, necesidad.vitaminaC);
        ActualizarBarra(barraOmega3, omega3, necesidad.omega3);
        ActualizarBarra(barraProteina, proteinas, necesidad.proteinas);
        ActualizarBarra(barraMag, magnesio, necesidad.magnesio);
        ActualizarBarra(barraVitaminaB, vitaminaB, necesidad.vitaminaB);
        ActualizarBarra(barraFibra, fibra, necesidad.fibra);
        ActualizarBarra(barraAntiOx, antioxidantes, necesidad.antioxidantes);
        ActualizarBarra(barraCarboH, carbohidratos, necesidad.carbohidratos);

        VerificarResultadoFinal();
    }

    private void ActualizarBarra(BarraNutricional barra, float valorTotal, RangoNutriente rango)
    {
        if (barra == null) return;

        float porcentaje = valorTotal / rango.maximo;
        barra.ActualizarBarra(porcentaje);

        if (valorTotal < rango.minimo)
            barra.Estado = BarraNutricional.EstadoBarra.Bajo;
        else if (valorTotal > rango.maximo)
            barra.Estado = BarraNutricional.EstadoBarra.Alto;
        else
            barra.Estado = BarraNutricional.EstadoBarra.Ideal;
    }

    private void VerificarResultadoFinal()
    {
        bool todasIdeales =
            barraHierro.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraVitaminaC.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraOmega3.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraProteina.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraMag.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraVitaminaB.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraFibra.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraAntiOx.Estado == BarraNutricional.EstadoBarra.Ideal &&
            barraCarboH.Estado == BarraNutricional.EstadoBarra.Ideal;

        if (mensajeResultado != null)
        {
            if (todasIdeales)
            {
                mensajeResultado.text = "¡Excelente! Tu batido cumple con los requisitos para " + necesidad.nombre;
                mensajeResultado.color = Color.green;
            }
            else
            {
                mensajeResultado.text = "Ajusta tu batido para adaptarlo mejor a " + necesidad.nombre;
                mensajeResultado.color = Color.yellow;
            }
        }
    }

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

        if (mensajeResultado != null)
            mensajeResultado.text = "";

        Debug.Log("Licuadora vaciada.");
    }
}
