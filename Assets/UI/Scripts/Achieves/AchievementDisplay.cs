using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class AchievementDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] page1Achievements;
    public TextMeshProUGUI[] page2Achievements;
    public TextMeshProUGUI[] page3Achievements;

    private string[] allAchievementNames = new string[]
    {
        "kill 30 enemies", "kill 50 enemies", "kill 100 enemies",
        "kill 200 enemies", "kill 500 enemies", "Collect 500 coins",

        "Collect 1500 coins", "50 attacks commited", "150 attacks commited",
        "Use abilities 20 times", "Use abilities 50 times", "Deal 1000 damage",

        "Deal 5000 damage", "Got Max HP", "Got Max Stamina",
        "Max Level of ability One", "Max Level of ability Two", "Max Level of ability Three"
    };

    private Dictionary<string, TextMeshProUGUI> achievementTextMap = new();

    private void Awake()
    {
        // Привязка текстов к достижениям
        for (int i = 0; i < 6; i++)
        {
            achievementTextMap[allAchievementNames[i]] = page1Achievements[i];
            achievementTextMap[allAchievementNames[i + 6]] = page2Achievements[i];
            achievementTextMap[allAchievementNames[i + 12]] = page3Achievements[i];
        }
    }

    private void Start()
    {
        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.OnAchievementsUpdated += RefreshDisplay;
        }

        RefreshDisplay(); // отобразить всё сразу при старте
    }

    private void OnDestroy()
    {
        if (AchievementManager.Instance != null)
            AchievementManager.Instance.OnAchievementsUpdated -= RefreshDisplay;
    }

    public void RefreshDisplay()
    {
        foreach (var pair in achievementTextMap)
        {
            bool unlocked = AchievementManager.Instance != null && AchievementManager.Instance.IsUnlocked(pair.Key);
            pair.Value.text = unlocked ? $"<s>{pair.Key}</s>" : pair.Key;
            pair.Value.color = unlocked ? Color.green : Color.gray;
        }
    }
}
