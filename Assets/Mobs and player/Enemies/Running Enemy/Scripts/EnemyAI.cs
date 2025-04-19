using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // Параметры врага
    public float maxHealth = 100f; // Максимальное здоровье
    public float currentHealth;   // Текущее здоровье
    public float damage = 10f;    // Урон, который наносит враг
    public float fieldOfViewRadius = 5f; // Радиус области видимости

    // Ссылка на игрока
    public Transform player;
    private bool isPlayerInSight = false; // Флаг, находится ли игрок в зоне видимости

    // Компонент NavMeshAgent для управления движением
    private NavMeshAgent navMeshAgent;

    public float attackRange = 1.5f;

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false; // Отключаем автоматическое вращение
            navMeshAgent.updateUpAxis = false; // Предотвращаем изменение вертикальной оси
        }

        // Инициализация здоровья
        currentHealth = maxHealth;
        isDead = false;

        // Находим игрока по тегу "Player"
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("EnemyAI: Player not found! Make sure the player has the 'Player' tag.");
        }

        // Получаем компонент NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("EnemyAI: NavMeshAgent component not found on the enemy!");
        }
    }

    void Update()
    {
        if (isDead) return;
        // Проверяем наличие необходимых ссылок один раз
        if (player == null || navMeshAgent == null)
            return;

        // Проверяем видимость игрока один раз
        CheckPlayerInSight();

        if (isPlayerInSight)
        {
            FollowPlayer();
            
            // Поворот без изменения размера
            if (navMeshAgent.velocity.x > 0.1f)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 
                                                transform.localScale.y, 
                                                transform.localScale.z);
            }
            else if (navMeshAgent.velocity.x < -0.1f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 
                                                transform.localScale.y, 
                                                transform.localScale.z);
            }
        }
        else
        {
            navMeshAgent.ResetPath();
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isPlayerInSight && navMeshAgent.velocity.magnitude > 0.1f);
            animator.SetBool("IsDead", isDead);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                
                // Опциональное отталкивание
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * 3f, ForceMode2D.Impulse);
            }
        }
    }

    void CheckPlayerInSight()
    {
        // Вычисляем расстояние между врагом и игроком
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Если игрок находится в пределах радиуса видимости
        if (distanceToPlayer <= fieldOfViewRadius)
        {
            isPlayerInSight = true;
        }
        else
        {
            isPlayerInSight = false;
        }
    }

    void FollowPlayer()
    {
        // Устанавливаем цель для NavMeshAgent
        navMeshAgent.SetDestination(player.position);
    }

    // Метод для получения урона
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Enemy took {damageAmount} damage. Current health: {currentHealth}");

        // Если здоровье закончилось, вызываем метод Die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод для смерти врага
    private void Die()
    {
        isDead = true;
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        
        if (animator != null)
        {
            animator.SetTrigger("Die"); // Альтернатива IsDead, если используете триггер
        }
        Destroy(gameObject, 2f);
        
        Debug.Log("Enemy has died!");
        // Здесь можно добавить логику смерти, например:
        // - Воспроизведение анимации смерти
        // - Уничтожение объекта
        gameObject.SetActive(false); // Просто отключаем объект
    }

    // Визуализация области видимости в редакторе Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfViewRadius);
    }
}