using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class SkillAnimatiionsController : MonoBehaviour
{
    public GameObject skillEffectObject;
    private Animator skillAnimator;

    private void Awake()
    {
        skillAnimator = skillEffectObject.GetComponent<Animator>();
        skillEffectObject.SetActive(false);
    }

    
    public void On()
    {
        
        skillEffectObject.SetActive(true);
        
        
    }

    public void Off()
    {
        skillEffectObject.SetActive(false);
    }
}
