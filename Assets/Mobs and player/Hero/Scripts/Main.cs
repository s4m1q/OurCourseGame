using UnityEngine;
using System.Collections; //Поебота добавленная Тимуром

public class PlayerController : MonoBehaviour
{
    // Параметры движения
    public float moveSpeed = 5f; // Обычная скорость
    public float runSpeed = 8f; // Скорость бега
    private Rigidbody2D rb;
    private float defaultMoveSpeed;
    private float defaultRunSpeed;

    public Animator anime;
    private SpriteRenderer spriteRenderer; // Компонент для управления спрайтом

    // Параметры здоровья и стамины
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxStamina = 100f;
    public int Coins = 0;
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
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer

        // Инициализация здоровья и стамины
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Отключаем вращение через физику
        rb.freezeRotation = true;

        defaultMoveSpeed = moveSpeed;
        defaultRunSpeed = runSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            moveSpeed *= 0.7f;
            runSpeed *= 0.7f;
        }
    }


    public void AddStamina(int value)
    {
        if(currentStamina+value<maxStamina){
            currentStamina+=value;
        }else{
            currentStamina=maxStamina;
        }
    }


    public void AddHP(int value)
    {
        if(currentHealth+value<maxHealth){
            currentHealth+=value;
        }else{
            currentHealth=maxHealth;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            moveSpeed = defaultMoveSpeed;
            runSpeed = defaultRunSpeed;
        }
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
            anime.SetBool("Run", true);
        }
        else
        {
            isRunning = false; // Запрещаем бег, если стамина закончилась или Shift не зажат
            anime.SetBool("Run", false);
        }

        // Вычисляем текущую скорость
        float speed = isRunning ? runSpeed : moveSpeed;

        // Создаем вектор направления движения
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        // Применяем движение к Rigidbody2D
        if (movement != Vector2.zero)
        {
            rb.linearVelocity = movement * speed;

            // Поворот персонажа в зависимости от направления движения
            if (moveX > 0) // Движение вправо
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Угол 0 градусов
            }
            else if (moveX < 0) // Движение влево
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Поворот на 180 градусов по оси Y
            }

            if (!isRunning)
            {
                anime.SetBool("Walk", true);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anime.SetBool("Walk", false);
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
    int fl = 0;
    public void AddScore(int value) {
        Coins+=value;
        if (Coins >= 100 && fl == 0){
            AchievementManager.Instance.UnlockAchievement("100 монет");
            fl = 1;
            }
    }
    public void TakeDamage(float damage)
    {
        // Уменьшаем здоровье при получении урона
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //Поебота добавленная Тимуром
        StartCoroutine(DamageFlash());
        //Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        // Проверяем, если здоровье закончилось
        if (currentHealth <= 0)
        {
            Die();
            anime.SetBool("Death", true);
        }
    }

    //Поебота добавленная Тимуром
    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        // Логика смерти персонажа
        Debug.Log("Player has died!");
        // Например, можно отключить объект игрока
        gameObject.SetActive(false);
    }
}