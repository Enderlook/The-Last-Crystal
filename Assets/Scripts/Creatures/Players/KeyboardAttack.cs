using CreaturesAddons;
using UnityEngine;

public class KeyboardAttack : MonoBehaviour, IAttack, IAwake
{
    [Header("Configuration")]
    [Tooltip("Basic attack key.")]
    public KeyCode keyAttack;
    [Tooltip("Cooldown of basic attack.")]
    public float timeBtwAttack;
    [Tooltip("Range melee attack.")]
    [Range(0f, 10f)]
    public float rangeAttack;
    [Tooltip("Position attack")]
    public Transform attackPosition;
    [Tooltip("Enemy layer.")]
    public LayerMask enemyLayer;

    private Animator thisAnimator;
    private float nextAttack;
    private float damage;

    // Constants
    private const string BASIC_ATTACK = "Attack";

    void IAwake.Awake(Creature creature)
    {
        thisAnimator = creature.animator;
        damage = creature.damage;
    }

    void IAttack.Attack(float time)
    {
        if (Input.GetKey(keyAttack) && time >= nextAttack)
        {
            thisAnimator.SetTrigger(BASIC_ATTACK);
            nextAttack = time + timeBtwAttack;
        }
    }

    void BasicAttackWarrior()
    {
        Collider2D[] toDamage = Physics2D.OverlapCircleAll(attackPosition.position, rangeAttack,
            enemyLayer);
        for (int n = 0; n < toDamage.Length; n++)
        {
            Vector2 direction = (toDamage[n].transform.position - transform.position).normalized;
            toDamage[n].GetComponent<Creature>().TakeDamage(damage, direction, 30f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, rangeAttack);
    }
}
