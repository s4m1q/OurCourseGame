using UnityEngine;

public class combat : MonoBehaviour
{
    [Header("Уровень способности (от 1 до 6)")]
    [Range(1, 6)]
    public int attackLevel = 1;

    [Header("Урон по уровням")]
    public int[] attackDamageByLevel; // Заполняется в инспекторе

    [Header("Радиус по уровням")]
    public float[] attackRangeByLevel; // Заполняется в инспекторе

    [Header("Прочее")]
    public LayerMask Enemy_layers;
    public Animator animator;
    public Transform Attack_point;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("HeroKnight_Attack"))
        {
            animator.SetTrigger("Attack");
            AchievementConditions.OnCombatsWere();
            int index = Mathf.Clamp(attackLevel - 1, 0, Mathf.Min(attackDamageByLevel.Length, attackRangeByLevel.Length) - 1);
            int currentDamage = attackDamageByLevel[index];
            float currentRange = attackRangeByLevel[index];

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Attack_point.position, currentRange, Enemy_layers);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyAI enemyAI)) {
                    AchievementConditions.Ondamaged(currentDamage);
                    enemyAI.TakeDamage(currentDamage);
                    }

                if (enemy.TryGetComponent(out RangedEnemyAI rangedEnemy)) {
                    AchievementConditions.Ondamaged(currentDamage);
                    rangedEnemy.TakeDamage(currentDamage);
                }

                if (enemy.TryGetComponent(out TankAI tank)) {
                    AchievementConditions.Ondamaged(currentDamage);
                    tank.TakeDamage(currentDamage);
                }

                if (enemy.TryGetComponent(out vase vaseComponent))
                    vaseComponent.TakeDamage(currentDamage);

                if (enemy.TryGetComponent(out RangeZonaEnemyAI rangezonaEnemy)) {
                    AchievementConditions.Ondamaged(currentDamage);
                    rangezonaEnemy.TakeDamage(currentDamage);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (Attack_point != null && attackRangeByLevel != null && attackLevel - 1 < attackRangeByLevel.Length)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Attack_point.position, attackRangeByLevel[attackLevel - 1]);
        }
    }
}
