using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para la UI

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody2D rigidbody;
    private Vector3 velocity;
    private Vector3 knockbackVelocity;
    
    public float speed;
    private Transform player;

    // Variables de salud y daño
    public float maxHealth = 100f;  // Vida máxima del enemigo
    public float healthPoints;      // Vida actual (se inicializa con maxHealth)
    public float knockbackForceX;
    public float knockbackForceY;
    public float damageToGive;

    // Barra de vida (asigna el componente Image de la barra en el prefab)
    public Image healthBarFill;

    // Variables para el efecto de knockback
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    public float knockbackDuration = 0.5f; // Duración del knockback

    // Variables para la animación de muerte
    public float deathAnimationDuration = 1.0f; // Duración de la animación de muerte
    private bool isDying = false;

    // Prefab para spawnear nuevos enemigos
    public GameObject enemyPrefab;

    // Ruta de la carpeta en Resources para cargar Animator Controllers aleatorios.
    // Tus controladores deben estar en: Assets/Resources/Prefabs/Animators
    public string randomAnimatorFolder = "Prefabs/Animators";

    // Controlador predeterminado (se asigna en el Inspector)
    public RuntimeAnimatorController defaultAnimatorController;

    // Ruta para cargar el sprite por defecto (sin extensión)
    public string defaultSpritePath = "Sprites/DefaultEnemy";

    // Parámetros para posicionar nuevos enemigos alrededor del jugador
    public float minSpawnDistanceFromPlayer = 5f;
    public float maxSpawnDistanceFromPlayer = 10f;

    // Variable estática para incrementar la cantidad de enemigos a spawnear
    public static int spawnCount = 2;

    // Variables de sonido
    public AudioClip collisionSound;    // Sonido al colisionar con el jugador
    public AudioClip deathSound;        // Sonido al ser eliminado
    private AudioSource audioSource;    // Componente AudioSource para reproducir sonidos

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        // Inicializar la vida actual
        healthPoints = maxHealth;

        // Asignar un Animator Controller aleatorio al enemigo inicial
        AssignRandomAnimatorController(gameObject);

        // Asegurarse de que el enemigo tenga un sprite asignado
        EnsureSprite(gameObject);

        // Obtener o agregar el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Buscar al jugador
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
        else
            Debug.LogError("No se encontró el objeto con tag 'Player'");
    }

    void Update()
    {
        // Actualizar la barra de vida
        if (healthBarFill != null)
        {
            float fill = healthPoints / maxHealth;
            healthBarFill.fillAmount = fill;
            Debug.Log("HealthBar fillAmount: " + fill);
        }
        else
        {
            Debug.LogWarning("No se ha asignado la referencia a HealthBarFill");
        }
        
        if (player != null && !isDying)
        {
            if (isKnockback)
            {
                knockbackTimer -= Time.deltaTime;
                if (knockbackTimer <= 0f)
                    isKnockback = false;
            }
            else
            {
                // Calcular la dirección hacia el jugador
                Vector3 direction = (player.position - transform.position).normalized;
                velocity = direction * speed;

                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                animator.SetFloat("Speed", velocity.sqrMagnitude);

                float deltaX = player.position.x - transform.position.x;
                if (deltaX > 0)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (deltaX < 0)
                    transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            velocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (!isKnockback && !isDying)
            rigidbody.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        else if (isKnockback && !isDying)
            rigidbody.MovePosition(transform.position + knockbackVelocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Colisión con el jugador
        if (collision.CompareTag("Player"))
        {
            if (isDying)
                return;

            animator.SetTrigger("Attack");

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.currentHealth -= damageToGive;
                Debug.Log("Jugador recibió " + damageToGive + " de daño.");
            }
            
            // Reproducir sonido de colisión
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }
            
            isKnockback = true;
            knockbackTimer = knockbackDuration;
            Vector3 knockDir = (transform.position - collision.transform.position).normalized;
            knockbackVelocity = new Vector3(knockDir.x * knockbackForceX, knockDir.y * knockbackForceY, 0);
        }
        // Colisión con una bala
        else if (collision.CompareTag("Bullet"))
        {
            if (isDying)
                return;

            float damage = 1f;
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
                damage = bullet.damage;

            healthPoints -= damage;
            Debug.Log("Enemigo recibió " + damage + " de daño. Vida restante: " + healthPoints);

            if (healthPoints <= 0 && !isDying)
                Die();
        }
    }

    private void Die()
    {
        // Aumentar la puntuación
        PuntuacionUI.AumentarPuntuacion(10);

        isDying = true;
        rigidbody.velocity = Vector2.zero;

        // Reproducir sonido de muerte
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        
        animator.SetTrigger("Death");

        SpawnNewEnemies();

        StartCoroutine(WaitAndDestroy(deathAnimationDuration));
    }

    private IEnumerator WaitAndDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void SpawnNewEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("No se ha asignado un prefab para spawnear nuevos enemigos.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetRandomPositionAroundPlayer();
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.healthPoints = enemyScript.maxHealth;

            AssignRandomAnimatorController(newEnemy);
            EnsureSprite(newEnemy);
        }

        spawnCount++;
    }

    /// <summary>
    /// Genera una posición aleatoria en un anillo alrededor del jugador, evitando que sea muy cercana.
    /// </summary>
    private Vector3 GetRandomPositionAroundPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("Player no asignado. Retornando Vector3.zero.");
            return Vector3.zero;
        }

        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minSpawnDistanceFromPlayer, maxSpawnDistanceFromPlayer);
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * distance;
        return player.position + offset;
    }

    /// <summary>
    /// Carga todos los RuntimeAnimatorController de la carpeta especificada y asigna uno aleatorio al Animator.
    /// Si no se encuentran, asigna el controlador predeterminado.
    /// </summary>
    private void AssignRandomAnimatorController(GameObject enemyObj)
    {
        RuntimeAnimatorController[] controllers = Resources.LoadAll<RuntimeAnimatorController>(randomAnimatorFolder);
        Debug.Log("Cantidad de Animator Controllers cargados: " + (controllers != null ? controllers.Length : 0));

        Animator anim = enemyObj.GetComponent<Animator>();
        if (controllers != null && controllers.Length > 0)
        {
            int randomIndex = Random.Range(0, controllers.Length);
            RuntimeAnimatorController selectedController = controllers[randomIndex];
            if (anim != null)
            {
                anim.runtimeAnimatorController = selectedController;
                Debug.Log("Animator Controller asignado: " + selectedController.name);
            }
        }
        else
        {
            Debug.LogWarning("No se encontraron Animator Controllers en la carpeta: " + randomAnimatorFolder);
            if (anim != null && defaultAnimatorController != null)
            {
                anim.runtimeAnimatorController = defaultAnimatorController;
                Debug.Log("Se asignó el Animator Controller predeterminado: " + defaultAnimatorController.name);
            }
            else
            {
                Debug.LogWarning("No se asignó un Animator Controller predeterminado. El objeto usará el que ya tenga.");
            }
        }
    }

    /// <summary>
    /// Asegura que el objeto tenga un sprite asignado; de lo contrario, carga uno por defecto.
    /// </summary>
    private void EnsureSprite(GameObject enemyObj)
    {
        SpriteRenderer sr = enemyObj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (sr.sprite == null)
            {
                Sprite defaultSprite = Resources.Load<Sprite>(defaultSpritePath);
                if (defaultSprite != null)
                {
                    sr.sprite = defaultSprite;
                    Debug.Log("Se asignó el sprite por defecto: " + defaultSprite.name);
                }
                else
                {
                    Debug.LogWarning("No se encontró un sprite por defecto en la ruta: " + defaultSpritePath);
                }
            }
            else
            {
                Debug.Log("El objeto ya tiene asignado un sprite: " + sr.sprite.name);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un SpriteRenderer en el objeto: " + enemyObj.name);
        }
    }
}
