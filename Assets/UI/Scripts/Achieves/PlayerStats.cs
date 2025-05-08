using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Уровни способностей")]
    public int abilityOneLevel = 0;
    public int abilityTwoLevel = 0;
    public int abilityThreeLevel = 0;

    [Header("Здоровье и стамина")]
    public float maxHealth = 250;
    public int HealthLevel = 0;
    public float maxStamina = 100;
    public int StaminaLevel = 0;


    [Header("Уровень удара с руки")]
    public int meleeLevel = 0;

    [Header("Деньги")]
    public int coins = 7000;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // сохраняем при смене сцены
        }
        else
        {
            Destroy(gameObject); // не дублируем
        }
    }
}
