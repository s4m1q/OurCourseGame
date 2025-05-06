using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AbilityThreeLevelData
{
    public float Damage;
    public float Radius;
    public float Cooldown;
}

public class AbilityThree : MonoBehaviour
{
    public List<AbilityThreeLevelData> Levels = new List<AbilityThreeLevelData>();
    public int CurrentLevel = 0;

    public float duration = 5f;
    private bool isOnCooldown = false;

    public void UseAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Использована способность 3");
            AchievementConditions.OnAbilityUsed();
            StartCoroutine(ApplyPoisonAura());
        }
        else
        {
            Debug.Log("Способность 3 на перезарядке");
        }
    }

    private IEnumerator ApplyPoisonAura()
    {
        isOnCooldown = true;

        AbilityThreeLevelData data = Levels[Mathf.Clamp(CurrentLevel - 1, 0, Levels.Count - 1)];
        float elapsed = 0f;
        float interval = 1f;

        while (elapsed < duration)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, data.Radius);

            foreach (var col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<EnemyAI>()?.TakeDamage(data.Damage);
                    col.GetComponent<RangedEnemyAI>()?.TakeDamage(data.Damage);
                    col.GetComponent<TankAI>()?.TakeDamage(data.Damage);
                    col.GetComponent<RangeZonaEnemyAI>()?.TakeDamage(data.Damage);
                }
            }

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        yield return new WaitForSeconds(data.Cooldown);
        isOnCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (Levels.Count > 0)
        {
            AbilityThreeLevelData data = Levels[Mathf.Clamp(CurrentLevel - 1, 0, Levels.Count - 1)];
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, data.Radius);
        }
    }
}
