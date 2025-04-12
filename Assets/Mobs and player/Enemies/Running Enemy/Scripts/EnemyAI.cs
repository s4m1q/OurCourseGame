using UnityEngine;
using UnityEngine.AI;

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

    void Start()
    {
        // Инициализация здоровья
        currentHealth = maxHealth;

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
        if (player != null && navMeshAgent != null)
        {
            // Проверяем, находится ли игрок в зоне видимости
            CheckPlayerInSight();

            // Если игрок в зоне видимости, двигаемся к нему
            if (isPlayerInSight)
            {
                FollowPlayer();
            }
            else
            {
                // Останавливаем движение, если игрок вне зоны видимости
                navMeshAgent.ResetPath();
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