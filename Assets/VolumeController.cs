using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Referencia al AudioSource")]
    public AudioSource audioSource;

    [Header("Slider de Volumen (0 a 0.4)")]
    public Slider volumeSlider;

    private const string VolumePrefKey = "VolumeLevel";

    void Start()
    {
        if (audioSource != null && volumeSlider != null)
        {
            // Configura el rango del slider
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 0.4f;

            // Carga el volumen guardado o usa el valor predeterminado (0.2f)
            float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.2f);
            audioSource.volume = savedVolume;
            volumeSlider.value = savedVolume;

            // Escucha los cambios en el slider
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
    }

    void ChangeVolume(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
            PlayerPrefs.SetFloat(VolumePrefKey, value); // Guarda el volumen
        }
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
    }
}
