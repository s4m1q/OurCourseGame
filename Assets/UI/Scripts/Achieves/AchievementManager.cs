using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [Header("UI")]
    public GameObject notificationPrefab;
    public Transform notificationParent;

    private HashSet<string> unlockedAchievements = new HashSet<string>();

    public delegate void AchievementEvent();
    public event AchievementEvent OnAchievementsUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // сохраняем при переходе между сценами
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        TryFindNotificationParent();
    }

    private void TryFindNotificationParent()
    {
        if (notificationParent == null)
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                notificationParent = canvas.transform;
            }
        }
    }

    public void UnlockAchievement(string title)
    {
        if (unlockedAchievements.Contains(title))
            return;

        unlockedAchievements.Add(title);
        ShowNotification(title);

        OnAchievementsUpdated?.Invoke(); // для панели достижений
    }

    public bool IsUnlocked(string title)
    {
        return unlockedAchievements.Contains(title);
    }

    private void ShowNotification(string title)
    {
        TryFindNotificationParent();

        if (notificationPrefab == null || notificationParent == null)
        {
            Debug.LogWarning("Notification prefab или parent не назначен.");
            return;
        }

        GameObject notif = Instantiate(notificationPrefab, notificationParent);
        AchievementNotification notification = notif.GetComponent<AchievementNotification>();
        if (notification != null)
        {
            notification.Show(title);
        }
    }
}
