using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // Asigna en el Inspector el objeto que contiene el script PlayerHealth
    public PlayerHealth playerHealth;
    // Este script se asume que está en el mismo objeto que el componente Text
    private Text healthText;

    void Start()
    {
        // Obtener el componente Text desde el mismo GameObject
        healthText = GetComponent<Text>();
    }

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            // Muestra la vida actual del jugador. Puedes ajustar el formato si lo deseas.
            healthText.text = "Vida: " + playerHealth.currentHealth.ToString("F0");
        }
    }
}
