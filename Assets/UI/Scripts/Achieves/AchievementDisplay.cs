using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{
    [Header("Achievements (Page 1)")]
    public TextMeshProUGUI[] page1Achievements = new TextMeshProUGUI[6];

    [Header("Achievements (Page 2)")]
    public TextMeshProUGUI[] page2Achievements = new TextMeshProUGUI[6];

    [Header("Achievements (Page 3)")]
    public TextMeshProUGUI[] page3Achievements = new TextMeshProUGUI[6];

    private string[] allAchievementNames = new string[]
    {
        "kill 30 enemies",
        "kill 50 enemies",
        "kill 100 enemies",
        "kill 200 enemies",
        "kill 500 enemies",
        "Collect 500 coins",

        "Collect 1500 coins",
        "50 attacks commited",
        "150 attacks commited",
        "Use abilities 20 times",
        "Use abilities 50 times",
        "Deal 1000 damage",

        "Deal 5000 damage",
        "Got Max HP",
        "Got Max Stamina",
        "Upgrade Ability 1",
        "Upgrade Ability 2",
        "Upgrade Ability 3"
    };

    private void Start()
    {
        StartCoroutine(DelayedRefresh());
    }

    private System.Collections.IEnumerator DelayedRefresh()
    {
        yield return new WaitForSeconds(0.1f); // Ждём один кадр для инициализации AchievementManager
        RefreshDisplay();
    }


    public void RefreshDisplay()
    {
        for (int i = 0; i < 6; i++)
        {
            SetAchievementText(page1Achievements[i], allAchievementNames[i]);
            SetAchievementText(page2Achievements[i], allAchievementNames[i + 6]);
            SetAchievementText(page3Achievements[i], allAchievementNames[i + 12]);
        }
    }

    private void SetAchievementText(TextMeshProUGUI textField, string achievementKey)
{
    if (textField == null)
    {
        Debug.LogWarning($"Text field для достижения '{achievementKey}' не назначен.");
        return;
    }

    bool unlocked = AchievementManager.Instance != null && AchievementManager.Instance.IsUnlocked(achievementKey);
    textField.text = unlocked ? $"<s>{achievementKey}</s>" : achievementKey;
    textField.color = unlocked ? Color.green : Color.gray;
}

}
