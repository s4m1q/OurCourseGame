using UnityEngine;
using System.Collections;

public class AbilityThree : MonoBehaviour
{
    public float[] damagePerLevel = { 10f, 15f, 20f, 25f, 30f };
    public float[] radiusPerLevel = { 2f, 2.5f, 3f, 3.5f, 4f };
    public float[] cooldownPerLevel = { 16f, 14f, 12f, 10f, 8f };

    public int abilityLevel = 0; // Уровень способности от 0 до 4
    private bool isOnCooldown = false;
    public float duration = 5f; // Длительность действия ауры

    public void UseAbility()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ApplyPoisonAura());
            Debug.Log("Использована 3 способность");
        }
        else 
        {
            Debug.Log("перезарядка 3 способность");
        }
    }

    private IEnumerator ApplyPoisonAura()
    {
        isOnCooldown = true;
        float elapsed = 0f;
        float interval = 1f;

        while (elapsed < duration)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radiusPerLevel[abilityLevel]);

            foreach (var col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<EnemyAI>()?.TakeDamage(damagePerLevel[abilityLevel]);
                    col.GetComponent<RangedEnemyAI>()?.TakeDamage(damagePerLevel[abilityLevel]);
                    col.GetComponent<TankAI>()?.TakeDamage(damagePerLevel[abilityLevel]);
                }
            }

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        yield return new WaitForSeconds(cooldownPerLevel[abilityLevel]);
        isOnCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Показываем радиус в редакторе
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusPerLevel[abilityLevel]);
    }
}
