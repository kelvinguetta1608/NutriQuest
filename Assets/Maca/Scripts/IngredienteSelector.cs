using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IngredienteSelector : MonoBehaviour
{
    public List<Button> botonesIngredientes;

    private Button botonSeleccionado;

    public Color colorNormal = Color.white;
    public Color colorSeleccionado = new Color32(255, 100, 4, 255); // #ff6404

    void Start()
    {
        foreach (Button boton in botonesIngredientes)
        {
            boton.onClick.AddListener(() => SeleccionarBoton(boton));
            // Asegúrate de que todos inicien con color normal
            SetColor(boton, colorNormal);
        }
    }

    void SeleccionarBoton(Button boton)
    {
        if (botonSeleccionado != null)
        {
            SetColor(botonSeleccionado, colorNormal); // deselecciona el anterior
        }

        botonSeleccionado = boton;
        SetColor(botonSeleccionado, colorSeleccionado); // selecciona el nuevo
    }

    void SetColor(Button boton, Color color)
    {
        var colors = boton.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        colors.highlightedColor = color;
        boton.colors = colors;
    }
}
