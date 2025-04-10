using UnityEngine;

public class GameObjectActivator : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    public void ActivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un GameObject para activar.");
        }
    }

    public void DeactivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un GameObject para desactivar.");
        }
    }
}
