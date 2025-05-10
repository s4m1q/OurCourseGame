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

    KeyValuePair<float, float>[] NewXYforAnime = new KeyValuePair<float, float>[]
    {
        new KeyValuePair<float, float>(12.15f, 9f),
        new KeyValuePair<float, float>(13.5f, 10f),
        new KeyValuePair<float, float>(14.85f, 11f),
        new KeyValuePair<float, float>(16.2f, 12f),
        new KeyValuePair<float, float>(17.55f, 13f),
        new KeyValuePair<float, float>(18.9f, 14f),
        new KeyValuePair<float, float>(20.25f, 15f),
        new KeyValuePair<float, float>(21.6f, 16f)
    };
    
    public GameObject anime;
    public float duration = 5f;
    private bool isOnCooldown = false;

    public void UseAbility()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Использована способность 3");
            ChangeScale();
            anime.SetActive(true);
            AchievementConditions.OnAbilityUsed();
            StartCoroutine(ApplyPoisonAura());
        }
        else
        {
            Debug.Log("Способность 3 на перезарядке");
        }
    }

    private void ChangeScale()
    {
        switch (CurrentLevel) {
            case 0: anime.transform.localScale = new Vector3(NewXYforAnime[0].Key, NewXYforAnime[0].Value, anime.transform.localScale.z); break;
            case 1: anime.transform.localScale = new Vector3(NewXYforAnime[1].Key, NewXYforAnime[1].Value, anime.transform.localScale.z); break;
            case 2: anime.transform.localScale = new Vector3(NewXYforAnime[2].Key, NewXYforAnime[2].Value, anime.transform.localScale.z); break;
            case 3: anime.transform.localScale = new Vector3(NewXYforAnime[3].Key, NewXYforAnime[3].Value, anime.transform.localScale.z); break;
            case 4: anime.transform.localScale = new Vector3(NewXYforAnime[4].Key, NewXYforAnime[4].Value, anime.transform.localScale.z); break;
            case 5: anime.transform.localScale = new Vector3(NewXYforAnime[5].Key, NewXYforAnime[5].Value, anime.transform.localScale.z); break;
            case 6: anime.transform.localScale = new Vector3(NewXYforAnime[6].Key, NewXYforAnime[6].Value, anime.transform.localScale.z); break;
            case 7: anime.transform.localScale = new Vector3(NewXYforAnime[7].Key, NewXYforAnime[7].Value, anime.transform.localScale.z); break;
            }
    }

    private IEnumerator ApplyPoisonAura()
    {
        isOnCooldown = true;

        AbilityThreeLevelData data = Levels[Mathf.Clamp(CurrentLevel-1, 0, Levels.Count-1)];
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
                    AchievementConditions.Ondamaged(data.Damage);
                }
            }

            yield return new WaitForSeconds(interval);
            elapsed += interval;
            
        }
        anime.SetActive(false);
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
