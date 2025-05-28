using UnityEngine;

[System.Serializable]
public class RangoNutriente
{
    public float minimo;
    public float maximo;
}

[CreateAssetMenu(fileName = "NuevaNecesidad", menuName = "NutriQuest/NecesidadNutricional", order = 1)]
public class NecesidadNutricional : ScriptableObject
{
    public string nombre;
    [TextArea]
    public string descripcion;

    public RangoNutriente hierro;
    public RangoNutriente vitaminaC;
    public RangoNutriente omega3;
    public RangoNutriente proteinas;
    public RangoNutriente magnesio;
    public RangoNutriente vitaminaB;
    public RangoNutriente fibra;
    public RangoNutriente antioxidantes;
    public RangoNutriente carbohidratos;
}
