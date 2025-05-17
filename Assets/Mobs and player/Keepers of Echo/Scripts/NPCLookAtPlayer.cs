using UnityEngine;

public class NPCLookAtPlayer : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private bool flipX = true; // Использовать отражение по X
    [SerializeField] private float detectionRange = 10f; // Дистанция обнаружения игрока
    
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    private void Start()
    {
        // Находим игрока по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Игрок с тегом 'Player' не найден в сцене!");
        }

        // Получаем компонент SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Компонент SpriteRenderer не найден на NPC!");
        }

        // Сохраняем оригинальный масштаб
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (playerTransform == null || spriteRenderer == null) return;

        // Проверяем дистанцию до игрока
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance > detectionRange) return;

        // Определяем направление к игроку
        Vector3 direction = playerTransform.position - transform.position;
        
        // Поворачиваем NPC в сторону игрока
        if (flipX)
        {
            // Используем отражение по X
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            // Или меняем масштаб по X (альтернативный способ)
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
        }
    }

    // Опционально: рисуем зону обнаружения в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}