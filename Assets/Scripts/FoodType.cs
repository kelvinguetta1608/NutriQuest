using UnityEngine;

[System.Serializable]
public enum FoodCategory
{
    Ansiedad,
    Sueño,
    ActividadFisica,
    Estres
}

public class FoodType : MonoBehaviour
{
    public FoodCategory category;
}