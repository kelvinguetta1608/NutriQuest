
using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : MonoBehaviour
{
    [SerializeField]
    private Button BotonplayEncvennder;


    private void OnEnable()
    {
        BotonplayEncvennder.onClick.AddListener(defaultVibration);
    }

    private void OnDisable()
    {
        BotonplayEncvennder.onClick.RemoveListener(defaultVibration);
    }

    private void defaultVibration() 
    {
        Debug.Log("Vibracion esta funcionando");
        Handheld.Vibrate();
    }
}
