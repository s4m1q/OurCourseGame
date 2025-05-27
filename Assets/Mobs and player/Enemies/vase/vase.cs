using UnityEngine;

public class vase : MonoBehaviour
{
    public float dropChance = 0.5f;
    public GameObject dropPrefab;
    private bool isDead = false;
    public float maxHealth = 10f; 
    public float currentHealth;   
    void Start()
    {
        currentHealth=maxHealth;
    }

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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die(){
        isDead = true;

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
        Destroy(gameObject);
    }
}
