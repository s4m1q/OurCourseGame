using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndController : MonoBehaviour
{
    public RectTransform introText; // Текст с историей
    public float scrollSpeed = 20f;
    public float maxYPosition = 200f; // координата, после которой авто-запуск сцены

    private bool skipped = false;


    void Start()
    {
        introText.transform.rotation = Quaternion.Euler(25, 0, 0);
    }
    void Update()
    {
        if (!skipped)
        {
            // Перемещаем текст вверх
            introText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            
            // Пропуск по любой клавише
            if (Input.anyKeyDown)
            {
                SkipIntro();
            }

            // Автоматический переход, если текст полностью прокручен
            if (introText.anchoredPosition.y >= maxYPosition)
            {
                LoadLevel1();
            }
        }
    }

    void SkipIntro()
    {
        skipped = true;
        LoadLevel1();
    }

    void LoadLevel1()
    {
        SceneManager.LoadScene("Epilog");
    }
}


