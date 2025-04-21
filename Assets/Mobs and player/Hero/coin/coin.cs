using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName; // Название предмета (например, "Coin")
    public int value = 1; // Значение предмета (например, количество очков)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что игрок коснулся объекта
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player picked up {itemName}");

            // Например, увеличиваем счет игрока
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddScore(value); // Метод для увеличения счета
            }

            // Уничтожаем объект после подбора
            Destroy(gameObject);
        }
    }
}