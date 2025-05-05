using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    public GameObject notificationPrefab;
    public Transform notificationParent;
    public Canvas canvas;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UnlockAchievement(string title)
    {
        if (notificationPrefab == null || notificationParent == null)
        {
            Debug.LogWarning("Не назначены префаб и родитель.");
            return;
        }

        GameObject notif = Instantiate(notificationPrefab, notificationParent);
        notif.GetComponent<AchievementNotification>().Show(title);
    }

    public void ShowAchievement(string title)
    {
        Instantiate(notificationPrefab, canvas.transform).GetComponent<AchievementNotification>().Show(title);
    }

}
