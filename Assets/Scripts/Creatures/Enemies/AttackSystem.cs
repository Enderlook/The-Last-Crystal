using UnityEngine;
using CreaturesAddons;

public class AttackSystem : MonoBehaviour, IAwake, IAttack
{
    [Header("Setup")]
    [Tooltip("Attack position")]
    public Transform attackPosition;
    [Tooltip("Range attack")]
    [Range(0f, 10f)]
    public float rangeAttack;
    [Tooltip("Player layer mask")]
    public LayerMask playerMask;
    [Tooltip("Cooldown of basic attack.")]
    public float timeBtwAttack;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float nextAttack;
    private Collider2D playerToDamage;
    private float damage;

    private const string ATTACK = "Attack";
    
    void IAwake.Awake(Creature creature)
    {
        animator = creature.animator;
        spriteRenderer = creature.sprite;
        damage = creature.damage;
    }

    void IAttack.Attack(float time)
    {
        if (!spriteRenderer.flipX)
        {
            Vector3 thePositon = attackPosition.transform.localPosition;
            if (attackPosition.transform.localPosition.x < 0)
                thePositon.x = -attackPosition.transform.localPosition.x;
            attackPosition.transform.localPosition = thePositon;
        }
        else if (spriteRenderer.flipX)
        {
            Vector3 thePositon = attackPosition.transform.localPosition;
            if (attackPosition.transform.localPosition.x > 0)
                thePositon.x = -attackPosition.transform.localPosition.x;
            attackPosition.transform.localPosition = thePositon;
        }

        playerToDamage = Physics2D.OverlapCircle(attackPosition.position,
                rangeAttack, playerMask);

        if (playerToDamage && time >= nextAttack)
        {
            animator.SetTrigger(ATTACK);
            nextAttack = time + timeBtwAttack;
        }
    }

    void Hit()
    {
        Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPosition.position, rangeAttack, 
            playerMask);
        for (int n = 0; n < playersToDamage.Length; n++)
        {
            Vector2 direction = (playersToDamage[n].transform.position - transform.position).normalized;
            playersToDamage[n].GetComponent<Creature>().TakeDamage(damage, direction, 30f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, rangeAttack);
    }
}
