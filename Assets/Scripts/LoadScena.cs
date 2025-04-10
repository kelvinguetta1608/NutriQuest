using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    // Esta función será llamada desde el botón
    public void CambiarAEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void ChangeSceneHome()
    {
        SceneManager.LoadScene("inicioMenu");
    }

    public void ChangeSceneAvatar()
    {
        SceneManager.LoadScene("Avatar");
    }
}
