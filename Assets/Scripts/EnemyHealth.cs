using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // Asigna en el Inspector la imagen que actúa como barra de llenado.
    public Image healthBarFill;

    // Referencia al script Enemy para obtener la vida actual y la máxima.
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>(); // Suponiendo que la barra es un hijo del objeto enemigo
        if (enemy == null)
        {
            Debug.LogError("No se encontró el componente Enemy en el padre del health bar.");
        }
    }

    void Update()
    {
        if (enemy != null && healthBarFill != null)
        {
            // Se actualiza la barra de vida en función del porcentaje de vida restante
            healthBarFill.fillAmount = enemy.healthPoints / enemy.maxHealth;
        }
    }
}
