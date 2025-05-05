using System.Collections.Generic;
using UnityEngine;

public class InventarioLicuadora : MonoBehaviour
{
    public List<string> ingredientes = new List<string>();

    public void AgregarIngrediente(string nombre)
    {
        ingredientes.Add(nombre);
        Debug.Log("Ingrediente agregado: " + nombre);
    }
}
