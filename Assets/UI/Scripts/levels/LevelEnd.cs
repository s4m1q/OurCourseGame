using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [Header("Настройки финала уровня")]
    public EnemySpawner enemySpawner;      // Ссылка на EnemySpawner
    public GameObject echoKeeperPrefab;    // Префаб Хранителя Эха
    public Transform echoKeeperSpawnPoint; // Точка появления Хранителя
    public GameObject portalPrefab;        // Префаб портала
    public Transform portalSpawnPoint;     // Точка появления портала
    public GameObject interactUI;          // UI "Нажмите E, чтобы продолжить"
    public Text dialogueText;              // UI-текст для диалога
    public float interactionDistance = 3f; // Дистанция взаимодействия
    public string nextLevelName;            // Имя следующей сцены

    [Header("Реплики Хранителя")]
    [TextArea(2, 5)]
    public string[] dialogueLines;          // Реплики

    private GameObject echoKeeperInstance;
    private GameObject portalInstance;
    private bool echoKeeperSpawned = false;
    private bool portalActivated = false;
    private bool dialogueStarted = false;
    private int currentLineIndex = 0;
    private Transform player;
    private GameObject cursor;

    void Start()
    {
        interactUI.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cursor = player.Find("Cursor").gameObject;
        
    }

    void Update()
    {
        if (!echoKeeperSpawned)
        {
            if (AllWavesCompletedAndNoEnemiesLeft())
            {
                SpawnEchoKeeper();
                cursor.GetComponent<CursorLogick>().ShowCursor();
            }
        }
        else if (!portalActivated && echoKeeperInstance != null)
        {
            float distanceToKeeper = Vector3.Distance(player.position, echoKeeperInstance.transform.position);

            if (distanceToKeeper <= interactionDistance)
            {
                if (!dialogueStarted)
                {
                    interactUI.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartDialogue();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        NextDialogueLine();
                    }
                }
            }
            else
            {
                interactUI.SetActive(false);
                dialogueText.gameObject.SetActive(false);
            }
        }
    }

    bool AllWavesCompletedAndNoEnemiesLeft()
    {
        if (enemySpawner == null) return false;
        
        return enemySpawner.IsAllWavesFinished() && enemySpawner.TotalEnemiesRemaining() == 0;
    }

    void SpawnEchoKeeper()
    {
        echoKeeperInstance = Instantiate(echoKeeperPrefab, echoKeeperSpawnPoint.position, Quaternion.identity);
        echoKeeperSpawned = true;
    }

    void StartDialogue()
    {
        dialogueStarted = true;
        interactUI.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        currentLineIndex = 0;
        dialogueText.text = dialogueLines[currentLineIndex];
    }

    void NextDialogueLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueStarted = false;
        dialogueText.gameObject.SetActive(false);
        SpawnPortal();
    }

    void SpawnPortal()
    {
        portalInstance = Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);
        portalActivated = true;
    }

    
}
