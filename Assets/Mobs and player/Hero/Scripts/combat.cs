using UnityEngine;

public class combat : MonoBehaviour
{
    public int Attack_damage;
    public float Attack_range;
    public LayerMask Enemy_layers;
    public Animator animator;
    public Transform Attack_point;
    // Update is called once per frame
    void Update()
{
    if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("HeroKnight_Attack"))
    {
        animator.SetTrigger("Attack");
        Collider2D[] Hit_enemies = Physics2D.OverlapCircleAll(Attack_point.position, Attack_range, Enemy_layers);

        foreach (Collider2D enemy in Hit_enemies)
        {
            // Проверяем наличие компонента EnemyAI
            var enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(Attack_damage);
            }

            var RangedEnemyAI = enemy.GetComponent<RangedEnemyAI>();
            if (RangedEnemyAI != null)
            {
                RangedEnemyAI.TakeDamage(Attack_damage);
            }

            var TankAI = enemy.GetComponent<TankAI>();
            if (TankAI != null)
            {
                TankAI.TakeDamage(Attack_damage);
            }
            // Проверяем наличие компонента vase
            var vaseComponent = enemy.GetComponent<vase>();
            if (vaseComponent != null)
            {
                vaseComponent.TakeDamage(Attack_damage);
            }
        }
    }
}
void OnDrawGizmos(){
    Gizmos.DrawWireSphere(Attack_point.position,Attack_range);
}

}

