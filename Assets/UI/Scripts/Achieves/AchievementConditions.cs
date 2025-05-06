using UnityEngine;

public static class AchievementConditions
{
    public static int enemiesKilled = 0;
    public static int coinsCollected = 0;
    public static int abilitiesUsed = 0;
    public static void OnEnemyKilled() //работает
    {
        enemiesKilled++;

        if (enemiesKilled == 30)
            AchievementManager.Instance.UnlockAchievement("kill 30 enemies");
        if (enemiesKilled == 50)
            AchievementManager.Instance.UnlockAchievement("kill 50 enemies");
        if (enemiesKilled == 100)
            AchievementManager.Instance.UnlockAchievement("kill 100 enemies");
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

    public static void OnAbilityUsed() //работает
    {
        abilitiesUsed++;
        if (abilitiesUsed == 20)
            AchievementManager.Instance.UnlockAchievement("Use abilities 20 times");
        if (abilitiesUsed == 50)
            AchievementManager.Instance.UnlockAchievement("Use abilities 50 times");
    }

    public static void OnUpgradeAbilityToMaxLevel(string abilityName) //работает
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
