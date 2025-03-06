using UnityEngine;
using UnityEngine.SceneManagement;

public class FinMenu : MonoBehaviour
{
    // Método para volver a jugar (cargar la escena "SampleScene")
    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Método para volver al menú (cargar la escena "MainMenu")
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
