using ReadyPlayerMe.Samples.QuickStart;
using TMPro;
using UnityEngine;

public class DataPersistenceTMP : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public TMP_InputField inputEdad;
    public TMP_InputField inputEstatura;
    public TMP_InputField inputPeso;
    public TMP_InputField inputGenero;

    private PersonalAvatarLoader avatarLoader;

    void Start()
    {
        avatarLoader = FindObjectOfType<PersonalAvatarLoader>();

        inputNombre.text = PlayerPrefs.GetString("Nombre", "");
        inputEdad.text = PlayerPrefs.GetInt("Edad", 0).ToString();
        inputEstatura.text = PlayerPrefs.GetFloat("Estatura", 0).ToString();
        inputPeso.text = PlayerPrefs.GetFloat("Peso", 0).ToString();
        inputGenero.text = PlayerPrefs.GetString("Genero", "");
    }

    public void GuardarDatos()
    {
        if (string.IsNullOrEmpty(inputEdad.text) || string.IsNullOrEmpty(inputEstatura.text) || string.IsNullOrEmpty(inputPeso.text))
        {
            Debug.LogWarning("Algunos campos numéricos están vacíos. No se guardarán.");
            return;
        }

        PlayerPrefs.SetString("Nombre", inputNombre.text);
        PlayerPrefs.SetInt("Edad", int.TryParse(inputEdad.text, out int edad) ? edad : 0);
        PlayerPrefs.SetFloat("Estatura", float.TryParse(inputEstatura.text, out float estatura) ? estatura : 0f);
        PlayerPrefs.SetFloat("Peso", float.TryParse(inputPeso.text, out float peso) ? peso : 0f);
        PlayerPrefs.SetString("Genero", inputGenero.text);

        // ?? Guardar la URL del avatar desde PersonalAvatarLoader
        if (avatarLoader != null)
        {
            PlayerPrefs.SetString("AvatarUrl", avatarLoader.CurrentAvatarUrl);
        }

        PlayerPrefs.Save();
        Debug.Log("Datos y avatar guardados correctamente");
    }
}
