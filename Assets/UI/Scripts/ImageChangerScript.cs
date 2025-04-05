using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageChangerScript : MonoBehaviour
{
    public Image firstImage;
    public GameObject menuPanel;
    public Text promptText;
    public CanvasGroup firstCanvasGroup;
    public CanvasGroup menuPanelCanvasGroup;
    public float fadeDuration = 1f;
    public float blinkDuration = 1f;

    private bool hasTransitioned = false;

    void Start()
    {
        // Инициализация значений
        firstCanvasGroup.alpha = 1;
        menuPanelCanvasGroup.alpha = 0;
        menuPanel.SetActive(false);
        promptText.text = "Press to continue";

        // Начало мерцания текста
        StartCoroutine(BlinkText());
    }

    void Update()
    {
        // Проверка нажатия мыши
        if (Input.GetMouseButtonDown(0) && !hasTransitioned)
        {
            StartCoroutine(TransitionToMenuPanel());
        }
    }

    IEnumerator TransitionToMenuPanel()
    {
        // Переход от первого изображения к панели меню
        yield return StartCoroutine(FadeOut(firstCanvasGroup));
        firstImage.gameObject.SetActive(false);
        menuPanel.SetActive(true);
        yield return StartCoroutine(FadeIn(menuPanelCanvasGroup));
        promptText.gameObject.SetActive(false);

        hasTransitioned = true; // Обозначение того, что переход был выполнен
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    IEnumerator BlinkText()
    {
        while (!hasTransitioned)
        {
            // Плавное появление текста
            for (float t = 0.01f; t < blinkDuration; t += Time.deltaTime)
            {
                promptText.color = new Color(promptText.color.r, promptText.color.g, promptText.color.b, Mathf.Lerp(0, 1, t / blinkDuration));
                yield return null;
            }

            // Задержка перед началом исчезания
            yield return new WaitForSeconds(0.5f);

            // Плавное исчезновение текста
            for (float t = 0.01f; t < blinkDuration; t += Time.deltaTime)
            {
                promptText.color = new Color(promptText.color.r, promptText.color.g, promptText.color.b, Mathf.Lerp(1, 0, t / blinkDuration));
                yield return null;
            }

            // Задержка перед началом появления
            yield return new WaitForSeconds(0.5f);
        }
    }
}
