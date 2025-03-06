using UnityEngine;
using UnityEngine.UI;

public class PuntuacionUI : MonoBehaviour
{
    // Asigna este componente en el Inspector al objeto Text que muestra la puntuación.
    public Text puntuacionText;

    // Instancia estática para poder acceder al método de forma global.
    private static PuntuacionUI instance;

    // Puntuación interna
    private int puntuacion = 0;

    void Awake()
    {
        // Guarda la instancia actual para que sea accesible globalmente
        instance = this;
    }

    void Start()
    {
        ActualizarUI();
    }

    /// <summary>
    /// Llama a este método para aumentar la puntuación en la cantidad indicada.
    /// Por ejemplo, desde el script Enemy: PuntuacionUI.AumentarPuntuacion(10);
    /// </summary>
    /// <param name="puntos">Cantidad de puntos a sumar.</param>
    public static void AumentarPuntuacion(int puntos)
    {
        if (instance != null)
        {
            instance.puntuacion += puntos;
            instance.ActualizarUI();
        }
        else
        {
            Debug.LogWarning("No se encontró la instancia de PuntuacionUI.");
        }
    }

    /// <summary>
    /// Actualiza el texto de la UI con la puntuación actual.
    /// </summary>
    private void ActualizarUI()
    {
        if (puntuacionText != null)
        {
            puntuacionText.text = "Puntuación: " + puntuacion.ToString();
        }
    }
}
