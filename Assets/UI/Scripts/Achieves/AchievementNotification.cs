using TMPro; 
using UnityEngine;

public class AchievementNotification : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public float showDuration = 2f;
    public float moveDistance = 100f;
    public float moveDuration = 0.5f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        initialPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0;
    }

    public void Show(string title)
    {
        titleText.text = $"Достижение получено: {title}";
        StartCoroutine(AnimateRoutine());
    }
    private System.Collections.IEnumerator AnimateRoutine()
    {
        // Появление снизу + затухание
        Vector2 start = initialPosition - new Vector2(0, moveDistance);
        Vector2 end = initialPosition;

        rectTransform.anchoredPosition = start;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            canvasGroup.alpha = t;
            yield return null;
        }

        // Пауза
        yield return new WaitForSeconds(showDuration);

        // Скрытие вверх + затухание
        Vector2 hideTarget = initialPosition + new Vector2(0, moveDistance);
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(end, hideTarget, t);
            canvasGroup.alpha = 1 - t;
            yield return null;
        }

        Destroy(gameObject);
    }
}