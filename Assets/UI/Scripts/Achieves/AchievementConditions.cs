using UnityEngine.UI;

public static class AchievementConditions
{
    public static int enemiesKilled = 0;
    public static int coinsCollected = 0;
    public static int abilitiesUsed = 0;
    public static float summaryDamage = 0;
    public static int combatsWere = 0;
    public static void OnEnemyKilled() //работает
    {
        enemiesKilled++;

        if (enemiesKilled == 30)
            AchievementManager.Instance.UnlockAchievement("kill 30 enemies");
        if (enemiesKilled == 50)
            AchievementManager.Instance.UnlockAchievement("kill 50 enemies");
        if (enemiesKilled == 100)
            AchievementManager.Instance.UnlockAchievement("kill 100 enemies");
        if (enemiesKilled == 200)
            AchievementManager.Instance.UnlockAchievement("kill 200 enemies");
        if (enemiesKilled == 500)
            AchievementManager.Instance.UnlockAchievement("kill 500 enemies");
    }

    static bool fl1 = true;
    static bool fl2 = true;
    public static void OnCoinCollected(int value) //работает
    {
        coinsCollected = value;
        if (coinsCollected >= 500 && fl1 == true) {
            AchievementManager.Instance.UnlockAchievement("Collect 500 coins");
            fl1 = false;
            }
        if (coinsCollected >= 1500 && fl2 == true) {
            AchievementManager.Instance.UnlockAchievement("Collect 1500 coins");
            fl2 = false;
            }
    }

    public static void OnCombatsWere() // работает
    {
        combatsWere++;
        if (combatsWere == 50)
        {
            AchievementManager.Instance.UnlockAchievement("50 attacks commited");
        }
        if (combatsWere == 150)
        {
            AchievementManager.Instance.UnlockAchievement("150 attacks commited");
        }
    }

    public static void OnAbilityUsed() //работает
    {
        abilitiesUsed++;
        if (abilitiesUsed == 20)
            AchievementManager.Instance.UnlockAchievement("Use abilities 20 times");
        if (abilitiesUsed == 50)
            AchievementManager.Instance.UnlockAchievement("Use abilities 50 times");
    }

    static bool WasThousanddam = false;
    static bool WasFiveThousanddam = false;
    public static void Ondamaged(float newdamage) // работает
    {
        summaryDamage+=newdamage;
        if (summaryDamage >= 1000 && WasThousanddam == false)
        {
            AchievementManager.Instance.UnlockAchievement("Deal 1000 damage");
            WasThousanddam = true;
        }
        if (summaryDamage >= 5000 && WasFiveThousanddam == false)
        {
            AchievementManager.Instance.UnlockAchievement("Deal 5000 damage");
            WasFiveThousanddam = true;
        }
    }

    public static void OnUpgradeAbilityToMaxLevel(string abilityName) // работает
    {
        AchievementManager.Instance.UnlockAchievement(abilityName);
    }

    static bool flMaxHP = false;
    public static void OnHealthIncreased(float newHealth) // работает
    {
        if (newHealth == 340 && flMaxHP == false) {
            AchievementManager.Instance.UnlockAchievement("Got Max HP");
            flMaxHP = true;
        }

    }

    static bool flMaxStamina = false;
    public static void OnStaminaIncreased(float newStamina) // работает
    {
        if (newStamina == 160 && flMaxStamina == false) {
            AchievementManager.Instance.UnlockAchievement("Got Max Stamina");
            flMaxStamina = true;
        }
    }
}
