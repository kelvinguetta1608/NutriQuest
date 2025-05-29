using UnityEngine;
using TMPro;
using System;

public class MisionDiariaManager : MonoBehaviour
{
    [System.Serializable]
    public class Mision
    {
        public string descripcion;
        public string tipo;
        public int recompensa;
    }

    public TextMeshProUGUI textoDescripcion;
    public TextMeshProUGUI textoRecompensa;

    private Mision[] misiones = new Mision[]
    {
        new Mision { descripcion = "Prepara un batido para la ansiedad", tipo = "ansiedad", recompensa = 10 },
        new Mision { descripcion = "Prepara un batido para el sueño", tipo = "sueño", recompensa = 12 },
        new Mision { descripcion = "Prepara un batido alto en fibra", tipo = "fibra", recompensa = 15 }
    };

    private Mision misionDelDia;

    void Start()
    {
        SeleccionarMisionDelDia();
        MostrarMisionEnUI();
    }

    private void SeleccionarMisionDelDia()
    {
        int dia = DateTime.Now.DayOfYear;
        int index = dia % misiones.Length;
        misionDelDia = misiones[index];

        PlayerPrefs.SetString("mision_actual", misionDelDia.tipo); // Guardamos el tipo por si se usa luego
    }

    private void MostrarMisionEnUI()
    {
        if (textoDescripcion != null)
            textoDescripcion.text = misionDelDia.descripcion;

        if (textoRecompensa != null)
            textoRecompensa.text = $"Recompensa: +{misionDelDia.recompensa} puntos";
    }
}
