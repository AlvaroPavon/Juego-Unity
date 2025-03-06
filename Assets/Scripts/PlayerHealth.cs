using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;  // Salud actual del jugador
    public float maxHealth;      // Salud máxima del jugador

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Evita que la salud supere el máximo
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Si la salud llega a 0 y aún no se ha ejecutado la muerte
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        if (collision.CompareTag("Enemy"))
        {
            // Intenta obtener el componente Enemy (puede estar en el padre si no está en el mismo GameObject)
            Enemy enemy = collision.GetComponent<Enemy>(); // o collision.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Enemy component encontrado. damageToGive: " + enemy.damageToGive);
                currentHealth -= enemy.damageToGive;
                Debug.Log("El jugador recibió " + enemy.damageToGive + " de daño. Salud actual: " + currentHealth);
            }
            else
            {
                Debug.Log("No se encontró el componente Enemy en el objeto colisionado.");
            }
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("El jugador ha muerto");
        // Cargar inmediatamente la escena "Fin"
        SceneManager.LoadScene("Fin");
    }
}
