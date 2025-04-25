using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityTwoLevelData
{
    public float healthRestore;
    public float staminaRestore;
    public float cooldown;
}

public class AbilityTwo : MonoBehaviour
{
    public List<AbilityTwoLevelData> Levels = new List<AbilityTwoLevelData>();
    public int CurrentLevel = 0;

    private float lastUseTime = -Mathf.Infinity;
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void UseAbility()
    {
        if (Levels.Count == 0 || CurrentLevel <= 0 || CurrentLevel > Levels.Count)
        {
            Debug.LogWarning("Некорректный уровень способности 2 или отсутствуют данные уровней.");
            return;
        }

        AbilityTwoLevelData data = Levels[CurrentLevel - 1];

        if (Time.time - lastUseTime < data.cooldown)
        {
            Debug.Log("Способность 2 на перезарядке");
            return;
        }

        lastUseTime = Time.time;

        player.currentHealth += data.healthRestore;
        player.currentHealth = Mathf.Clamp(player.currentHealth, 0, player.maxHealth);

        player.currentStamina += data.staminaRestore;
        player.currentStamina = Mathf.Clamp(player.currentStamina, 0, player.maxStamina);

        Debug.Log($"Использована способность 2: +{data.healthRestore} HP, +{data.staminaRestore} стамина.");
    }
}
