using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Параметры движения
    public float moveSpeed = 5f; // Обычная скорость
    public float runSpeed = 8f; // Скорость бега
    private Rigidbody2D rb;

    // Параметры здоровья и стамины
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f; // Скорость восстановления стамины
    public float staminaDrainRate = 20f; // Скорость расхода стамины

    // Задержка перед восстановлением стамины
    public float staminaRegenDelay = 2f; // Время ожидания перед восстановлением
    private float staminaRegenTimer = 0f; // Таймер для отсчета задержки

    // Флаги состояния
    private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Инициализация здоровья и стамины
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Отключаем вращение через физику
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Получаем входные данные от клавиш WASD или стрелок
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Проверяем, зажата ли клавиша Shift для бега
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Если игрок хочет бежать и стамина больше 0, разрешаем бег
        if (wantsToRun && currentStamina > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false; // Запрещаем бег, если стамина закончилась или Shift не зажат
        }

        // Вычисляем текущую скорость
        float speed = isRunning ? runSpeed : moveSpeed;

        // Создаем вектор направления движения
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        // Применяем движение к Rigidbody2D
        if (movement != Vector2.zero)
        {
            rb.linearVelocity = movement * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Управление стаминой
        HandleStamina();
    }

    void HandleStamina()
    {
        // Если бежим и стамина больше 0, расходуем её
        if (isRunning && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Ограничиваем минимальное значение

            // Сбрасываем таймер задержки, если бежим
            staminaRegenTimer = staminaRegenDelay;
        }

        // Если стамина равна 0, отсчитываем задержку
        if (currentStamina <= 0)
        {
            isRunning = false; // Запрещаем бег, если стамина закончилась
            staminaRegenTimer -= Time.deltaTime;

            // Если задержка истекла, начинаем восстановление
            if (staminaRegenTimer <= 0)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
        // Если не бежим и стамина не пустая, восстанавливаем её
        else if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    public void TakeDamage(float damage)
    {
        // Уменьшаем здоровье при получении урона
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Проверяем, если здоровье закончилось
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Логика смерти персонажа
        Debug.Log("Player has died!");
        // Например, можно отключить объект игрока
        gameObject.SetActive(false);
    }
}