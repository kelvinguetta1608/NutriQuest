using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class RangoNutriente
{
    public float minimo;
    public float maximo;
}

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

    public TextMeshProUGUI mensajeResultado;

    // RANGOS (se asignan en Start según la necesidad)
    public RangoNutriente rangoHierro;
    public RangoNutriente rangoVitaminaC;
    public RangoNutriente rangoOmega3;
    public RangoNutriente rangoProteina;
    public RangoNutriente rangoMagnesio;
    public RangoNutriente rangoVitaminaB;
    public RangoNutriente rangoFibra;
    public RangoNutriente rangoAntioxidantes;
    public RangoNutriente rangoCarbohidratos;

    void Start()
    {
        if (!string.IsNullOrEmpty(DatosCompartidos.necesidadSeleccionada))
        {
            EstablecerRangosPorNecesidad(DatosCompartidos.necesidadSeleccionada);
        }
    }

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
        if (ingredientesDentro.Count == 0) return;

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

        int total = ingredientesDentro.Count;

        ActualizarBarra(barraHierro, hierro / total, rangoHierro);
        ActualizarBarra(barraVitaminaC, vitC / total, rangoVitaminaC);
        ActualizarBarra(barraOmega3, omega3 / total, rangoOmega3);
        ActualizarBarra(barraProteina, proteinas / total, rangoProteina);
        ActualizarBarra(barraMag, magnesio / total, rangoMagnesio);
        ActualizarBarra(barraVitaminaB, vitaminaB / total, rangoVitaminaB);
        ActualizarBarra(barraFibra, fibra / total, rangoFibra);
        ActualizarBarra(barraAntiOx, antioxidantes / total, rangoAntioxidantes);
        ActualizarBarra(barraCarboH, carbohidratos / total, rangoCarbohidratos);

        VerificarResultadoFinal();
    }

    private void ActualizarBarra(BarraNutricional barra, float promedio, RangoNutriente rango)
    {
        if (barra == null) return;

        float porcentaje = promedio / rango.maximo;
        barra.ActualizarBarra(porcentaje);

        barra.Estado = (promedio >= rango.minimo && promedio <= rango.maximo)
            ? BarraNutricional.EstadoBarra.Ideal
            : BarraNutricional.EstadoBarra.FueraDeRango;
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
            mensajeResultado.text = todasIdeales
                ? "¡Batido perfecto dentro del rango ideal!"
                : "Aún puedes mejorar tu batido.";

            mensajeResultado.color = todasIdeales ? Color.green : Color.yellow;
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

    public void EstablecerRangosPorNecesidad(string necesidad)
    {
        switch (necesidad.ToLower())
        {
            case "sueño":
                rangoHierro = new RangoNutriente { minimo = 30, maximo = 60 };
                rangoVitaminaC = new RangoNutriente { minimo = 20, maximo = 50 };
                rangoOmega3 = new RangoNutriente { minimo = 60, maximo = 100 };
                rangoProteina = new RangoNutriente { minimo = 20, maximo = 50 };
                rangoMagnesio = new RangoNutriente { minimo = 70, maximo = 100 };
                rangoVitaminaB = new RangoNutriente { minimo = 40, maximo = 70 };
                rangoFibra = new RangoNutriente { minimo = 40, maximo = 80 };
                rangoAntioxidantes = new RangoNutriente { minimo = 50, maximo = 90 };
                rangoCarbohidratos = new RangoNutriente { minimo = 20, maximo = 60 };
                break;

            case "estrés":
                rangoHierro = new RangoNutriente { minimo = 50, maximo = 80 };
                rangoVitaminaC = new RangoNutriente { minimo = 70, maximo = 100 };
                rangoOmega3 = new RangoNutriente { minimo = 60, maximo = 100 };
                rangoProteina = new RangoNutriente { minimo = 40, maximo = 70 };
                rangoMagnesio = new RangoNutriente { minimo = 60, maximo = 90 };
                rangoVitaminaB = new RangoNutriente { minimo = 60, maximo = 90 };
                rangoFibra = new RangoNutriente { minimo = 50, maximo = 80 };
                rangoAntioxidantes = new RangoNutriente { minimo = 80, maximo = 100 };
                rangoCarbohidratos = new RangoNutriente { minimo = 30, maximo = 70 };
                break;

            case "ansiedad":
                rangoHierro = new RangoNutriente { minimo = 40, maximo = 70 };
                rangoVitaminaC = new RangoNutriente { minimo = 60, maximo = 90 };
                rangoOmega3 = new RangoNutriente { minimo = 70, maximo = 100 };
                rangoProteina = new RangoNutriente { minimo = 30, maximo = 60 };
                rangoMagnesio = new RangoNutriente { minimo = 80, maximo = 100 };
                rangoVitaminaB = new RangoNutriente { minimo = 60, maximo = 90 };
                rangoFibra = new RangoNutriente { minimo = 50, maximo = 90 };
                rangoAntioxidantes = new RangoNutriente { minimo = 70, maximo = 100 };
                rangoCarbohidratos = new RangoNutriente { minimo = 40, maximo = 70 };
                break;

            case "actividad física":
            case "ejercicio":
                rangoHierro = new RangoNutriente { minimo = 60, maximo = 100 };
                rangoVitaminaC = new RangoNutriente { minimo = 60, maximo = 100 };
                rangoOmega3 = new RangoNutriente { minimo = 40, maximo = 80 };
                rangoProteina = new RangoNutriente { minimo = 80, maximo = 100 };
                rangoMagnesio = new RangoNutriente { minimo = 50, maximo = 80 };
                rangoVitaminaB = new RangoNutriente { minimo = 70, maximo = 100 };
                rangoFibra = new RangoNutriente { minimo = 60, maximo = 100 };
                rangoAntioxidantes = new RangoNutriente { minimo = 60, maximo = 90 };
                rangoCarbohidratos = new RangoNutriente { minimo = 70, maximo = 100 };
                break;

            default:
                Debug.LogWarning("Necesidad no reconocida: " + necesidad);
                break;
        }

        Debug.Log("Rangos nutricionales establecidos para: " + necesidad);
    }
}
