using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityOneLevelData
{
    public float Damage;
    public float Radius;
    public float Cooldown;
}

public class AbilityOne : MonoBehaviour
{
    public List<AbilityOneLevelData> Levels = new List<AbilityOneLevelData>();
    public int CurrentLevel = 0;

    private bool isOnCooldown = false;

    public void UseAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Использована способность 1");

            // Здесь должна запускаться анимация и звук:
            // AnimationHandler.Play("AbilityOne");
            // AudioManager.Play("AbilityOneSFX");

            StartCoroutine(TryUse());
        }
        else
        {
            Debug.Log("Способность 1 на перезарядке");
        }
    }

    public IEnumerator TryUse()
    {
        isOnCooldown = true;

        AbilityOneLevelData data = Levels[Mathf.Clamp(CurrentLevel - 1, 0, Levels.Count - 1)];
        float maxRadius = data.Radius;
        float damage = data.Damage;

        Vector2 center = transform.position;
        float[] thresholds = new float[] { 0.33f, 0.66f, 1f };
        float delay = 0.5f;

        for (int i = 0; i < 3; i++)
        {
            float min = i == 0 ? 0f : thresholds[i - 1] * maxRadius;
            float max = thresholds[i] * maxRadius;

            Collider2D[] hits = Physics2D.OverlapCircleAll(center, max);

            foreach (Collider2D col in hits)
            {
                if (col.CompareTag("Enemy"))
                {
                    float distance = Vector2.Distance(center, col.transform.position);
                    if (distance >= min && distance <= max)
                    {
                        EnemyAI enemy = col.GetComponent<EnemyAI>();
                        RangedEnemyAI enemy1 = col.GetComponent<RangedEnemyAI>();
                        TankAI enemy2 = col.GetComponent<TankAI>();
                        
                        if (enemy != null)
                        {
                            enemy.TakeDamage(damage);
                            Debug.Log($"Нанесено {damage} урона врагу на расстоянии {distance:F2}");
                        }
                        if (enemy1 != null)
                        {
                            enemy1.TakeDamage(damage);
                            Debug.Log($"Нанесено {damage} урона врагу на расстоянии {distance:F2}");
                        }
                        if (enemy2 != null)
                        {
                            enemy2.TakeDamage(damage);
                            Debug.Log($"Нанесено {damage} урона врагу на расстоянии {distance:F2}");
                        }
                    }
                }
            }

            // Здесь можно добавить эффект визуального расширяющегося круга
            // VisualEffectManager.PlayPulseEffect(center, max);

            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(data.Cooldown);
        isOnCooldown = false;
    }
}
