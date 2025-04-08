using UnityEngine;

public class InventarioUIManager : MonoBehaviour
{
    [Header("Panel principal de frutas (PanelB/PanelC/frut)")]
    public GameObject panelFrutas;

    [Header("Panel del menú de categorías (PanelB/PanelCat)")]
    public GameObject panelCategorias;

    public GameObject contenedorFrutas; // arrástralo desde el inspector


    // MÉTODO para guardar la categoría actual (llamar desde cada fruta)
    public void GuardarCategoria(GameObject categoriaPanel)
    {
        UIStateManager.lastOpenedCategory = categoriaPanel;
        Debug.Log("Guardada categoría actual: " + categoriaPanel.name);
    }

    // MÉTODO para regresar desde frut/
    public void VolverDesdeFrutas()
    {
        // Apagar cada fruta individual
        foreach (Transform fruta in contenedorFrutas.transform)
        {
            fruta.gameObject.SetActive(false);
        }

        // Apagar panel general de frutas
        panelFrutas.SetActive(false);

        // Volver a la categoría anterior
        if (UIStateManager.lastOpenedCategory != null)
        {
            UIStateManager.lastOpenedCategory.SetActive(true);
        }
        else
        {
            panelCategorias.SetActive(true);
        }
    }

}
