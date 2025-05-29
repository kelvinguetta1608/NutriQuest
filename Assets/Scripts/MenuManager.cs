using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button ansiedadButton;
    [SerializeField] private Button sueñoButton;
    [SerializeField] private Button actividadFisicaButton;
    [SerializeField] private Button estresButton;

    void Start()
    {
        UpdateButtonStates();
        
        ansiedadButton?.onClick.AddListener(() => SelectCategory("Ansiedad"));
        sueñoButton?.onClick.AddListener(() => SelectCategory("Sueño"));
        actividadFisicaButton?.onClick.AddListener(() => SelectCategory("ActividadFisica"));
        estresButton?.onClick.AddListener(() => SelectCategory("Estres"));
    }

    private void UpdateButtonStates()
    {
        if (GameManager.Instance != null)
        {
            ansiedadButton.interactable = GameManager.Instance.IsCategoryUnlocked("Ansiedad");
            sueñoButton.interactable = GameManager.Instance.IsCategoryUnlocked("Sueño");
            actividadFisicaButton.interactable = GameManager.Instance.IsCategoryUnlocked("ActividadFisica");
            estresButton.interactable = GameManager.Instance.IsCategoryUnlocked("Estres");
        }
    }

    public void SelectCategory(string category)
    {
        if (GameManager.Instance.IsCategoryUnlocked(category))
        {
            GameManager.Instance.SetTargetCategory(category);
            SceneManager.LoadScene("GAMEF");
        }
    }
}