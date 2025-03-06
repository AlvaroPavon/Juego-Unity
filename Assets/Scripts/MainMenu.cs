using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Método que se llamará desde el botón PlayButton para iniciar el juego.
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Método que se llamará desde el botón QuitButton para salir del juego.
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
