using UnityEngine;
using UnityEngine.UI;

public class OpenLink : MonoBehaviour
{
    [SerializeField] private Button googleButton;

    // URL que quieres abrir
    public string googleUrl = "https://www.funcionpublica.gov.co/eva/gestornormativo/norma.php?i=53646#0";

    void Start()
    {
        googleButton.onClick.AddListener(OpenGoogle);
    }

    private void OpenGoogle()
    {
        Application.OpenURL(googleUrl);
    }
}
