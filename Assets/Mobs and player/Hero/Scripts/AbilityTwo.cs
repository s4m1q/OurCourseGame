using UnityEngine;

public class AbilityTwo : MonoBehaviour
{
    public float healthRestore = 40f;
    public float staminaRestore = 35f;
    public float cooldown = 12f;

    private float lastUseTime = -Mathf.Infinity;

    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void UseAbility()
    {
        if (Time.time - lastUseTime < cooldown)
        {
            Debug.Log("Ability Two is on cooldown.");
            return;
        }

        lastUseTime = Time.time;

        player.currentHealth += healthRestore;
        player.currentHealth = Mathf.Clamp(player.currentHealth, 0, player.maxHealth);

        player.currentStamina += staminaRestore;
        player.currentStamina = Mathf.Clamp(player.currentStamina, 0, player.maxStamina);

        Debug.Log($"Used Ability Two: +{healthRestore} HP, +{staminaRestore} stamina.");
    }
}
