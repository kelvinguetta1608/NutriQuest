using UnityEngine;

public class ContenedorDeDatosPersistente : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
