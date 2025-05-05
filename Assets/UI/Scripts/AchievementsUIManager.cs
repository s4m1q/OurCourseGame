using UnityEngine;
using UnityEngine.UI;


public class AchievementsUIManager : MonoBehaviour
{   
    public GameObject MainMenu;
    public GameObject Progress;
    public GameObject[] pages; // Панели с достижениями
    public Button nextButton;
    public Button prevButton;
    public Button exitButton;

    private int currentPageIndex = 0;

    void Start()
    {
        ShowPage(0);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    void ShowPage(int index)
    {
        // Скрываем все страницы
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        // Обновляем кнопки
        prevButton.gameObject.SetActive(index > 0);
        nextButton.gameObject.SetActive(index < pages.Length - 1);
    }

    void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
    }

    void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
        else
        {
            ExitToMainMenu();
        }
    }

    void ExitToMainMenu()
    {
        // Просто отключаем панель достижений
        Progress.SetActive(false);
        MainMenu.SetActive(true);
    }
}
