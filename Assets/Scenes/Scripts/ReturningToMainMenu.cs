using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturningToMainMenu : MonoBehaviour
{

    private bool skipped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!skipped)
        {


            // Пропуск по любой клавише
            if (Input.anyKeyDown)
            {
                SkipIntro();
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
        SceneManager.LoadScene("Main Menu");
    }
    
}
