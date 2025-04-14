using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private AbilityOne abilityOne;
    private AbilityTwo abilityTwo;
    private AbilityThree abilityThree;

    void Start()
    {
        abilityOne = GetComponent<AbilityOne>();
        abilityTwo = GetComponent<AbilityTwo>();
        abilityThree = GetComponent<AbilityThree>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilityOne.TryUse();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilityTwo.UseAbility();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            abilityThree.UseAbility();
        }
    }
}
