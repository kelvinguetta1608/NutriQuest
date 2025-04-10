using UnityEngine;
using TMPro; // Importar TextMeshPro

public class DataPersistenceTMP : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public TMP_InputField inputEdad;
    public TMP_InputField inputEstatura;
    public TMP_InputField inputPeso;
    public TMP_InputField inputGenero;

    void Start()
    {
        // Cargar datos al iniciar
        inputNombre.text = PlayerPrefs.GetString("Nombre", "");
        inputEdad.text = PlayerPrefs.GetInt("Edad", 0).ToString();
        inputEstatura.text = PlayerPrefs.GetFloat("Estatura", 0).ToString();
        inputPeso.text = PlayerPrefs.GetFloat("Peso", 0).ToString();
        inputGenero.text = PlayerPrefs.GetString("Genero", "");
    }

    public void GuardarDatos()
    {
        // Validar entradas vacías antes de guardar
        if (string.IsNullOrEmpty(inputEdad.text) || string.IsNullOrEmpty(inputEstatura.text) || string.IsNullOrEmpty(inputPeso.text))
        {
            Debug.LogWarning("Algunos campos numéricos están vacíos. No se guardarán.");
            return;
        }

        // Guardar datos en PlayerPrefs
        PlayerPrefs.SetString("Nombre", inputNombre.text);
        PlayerPrefs.SetInt("Edad", int.TryParse(inputEdad.text, out int edad) ? edad : 0);
        PlayerPrefs.SetFloat("Estatura", float.TryParse(inputEstatura.text, out float estatura) ? estatura : 0f);
        PlayerPrefs.SetFloat("Peso", float.TryParse(inputPeso.text, out float peso) ? peso : 0f);
        PlayerPrefs.SetString("Genero", inputGenero.text);

        PlayerPrefs.Save();
        Debug.Log("Datos guardados correctamente");
    }
}
