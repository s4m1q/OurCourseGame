using UnityEngine;

public class vase : MonoBehaviour
{
    public float dropChance = 0.5f;
    public GameObject dropPrefab;
    private bool isDead = false;
    public float maxHealth = 10f; // Максимальное здоровье
    public float currentHealth;   // Текущее здоровье
    void Start()
    {
        currentHealth=maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth<=0){
            Die();
        }
    }
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

    private void Die(){
        isDead = true;
        // Проверяем вероятность выпадения
        if (dropPrefab != null)
        {
            if(Random.value < dropChance){
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            }
            // Создаем выпадающий объект на месте смерти врага
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 2f);
        
        Debug.Log("Vase has died!");
        // Здесь можно добавить логику смерти, например:
        // - Воспроизведение анимации смерти
        Destroy(gameObject);// - Уничтожение объекта
        // Просто отключаем объект
    }
}
