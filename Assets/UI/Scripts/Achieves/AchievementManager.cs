using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{

    public List<string> allAchievements = new List<string>
    {
        "kill 30 enemies", "kill 50 enemies", "kill 100 enemies", "kill 200 enemies", "kill 500 enemies", "Collect 500 coins",
        "Collect 1500 coins", "50 attacks commited", "150 attacks commited", "Use abilities 20 times", "Use abilities 50 time", "Deal 1000 damage",
    "Deal 5000 damage", "Got Max HP", "Got Max Stamina", "Upgrade Ability 1", "Upgrade Ability 2", "Upgrade Ability 3"
    };

    public static AchievementManager Instance;

    public GameObject notificationPrefab;
    public Transform notificationParent;
    public Canvas canvas;

    // Храним все разблокированные достижения
    private HashSet<string> unlockedAchievements = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UnlockAchievement(string title)
    {
        // Проверяем, получено ли уже
        if (unlockedAchievements.Contains(title)) return;

        unlockedAchievements.Add(title);

        if (notificationPrefab == null || notificationParent == null)
        {
            Debug.LogWarning("Не назначены префаб и родитель.");
            return;
        }

        GameObject notif = Instantiate(notificationPrefab, notificationParent);
        notif.GetComponent<AchievementNotification>().Show(title);
    }

    public bool IsUnlocked(string title)
    {
        return unlockedAchievements.Contains(title);
    }


    public void ShowAchievement(string title)
    {
        Instantiate(notificationPrefab, canvas.transform).GetComponent<AchievementNotification>().Show(title);
    }
}
