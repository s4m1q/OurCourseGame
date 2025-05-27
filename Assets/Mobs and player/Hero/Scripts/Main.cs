using UnityEngine;
using System.Collections; //Часть Тимура

public class PlayerController : MonoBehaviour
{
    // Параметры движения
    public float moveSpeed = 5f; // Обычная скорость
    public float runSpeed = 8f; // Скорость бега
    private Rigidbody2D rb;
    private float defaultMoveSpeed;
    private float defaultRunSpeed;

    public Animator anime;
    private SpriteRenderer spriteRenderer; 
    public AudioSource stepSoundPlayer;
    public float stepInterval = 0.2f;
    private float lastStepTime = 0f;

    public float maxHealth = 250;
    public float currentHealth;
    public float maxStamina = 100;
    public int Coins = 0;
    public float currentStamina;
    public float staminaRegenRate = 10f; // Скорость восстановления стамины
    public float staminaDrainRate = 20f; // Скорость расхода стамины

    public float staminaRegenDelay = 2f; // Время ожидания перед восстановлением
    private float staminaRegenTimer = 0f; // Таймер для отсчета задержки

    // Флаги состояния
    private bool isRunning = false;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 

        currentHealth = maxHealth;
        currentStamina = maxStamina;

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
        }
        else{
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

        if (movement != Vector2.zero)
        {
            if (Time.time - lastStepTime >= stepInterval)
            {
                if (stepSoundPlayer != null && stepSoundPlayer.clip != null)
                {
                    stepSoundPlayer.PlayOneShot(stepSoundPlayer.clip);
                    lastStepTime = Time.time;
                }
                else
                {
                    Debug.LogWarning("AudioSource or AudioClip is not assigned!");
                }
            }

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
            if (stepSoundPlayer != null && stepSoundPlayer.isPlaying)
            {
                stepSoundPlayer.Stop();
                lastStepTime=stepInterval;
            }
            rb.linearVelocity = Vector2.zero;
            anime.SetBool("Walk", false);
        }

        // Управление стаминой
        HandleStamina();
    }

    void HandleStamina()
    {
        if (isRunning && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            staminaRegenTimer = staminaRegenDelay;
        }

        if (currentStamina <= 0)
        {
            isRunning = false; 
            staminaRegenTimer -= Time.deltaTime;

            if (staminaRegenTimer <= 0)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
        else if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }
    
    public void AddScore(int value) {
        Coins+=value;
        AchievementConditions.OnCoinCollected(Coins);
    }
    public void TakeDamage(float damage)
    {
        // Уменьшаем здоровье при получении урона
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
            anime.SetBool("Death", true);
        }
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        Debug.Log("Player has died!");
        gameObject.SetActive(false);
    }


}