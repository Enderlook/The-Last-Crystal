using UnityEngine;

[System.Serializable]
public class AIAttack
{
    [Header("Setup Attack")]
    [Tooltip("Attack position")]
    public Transform attackPosition;
    [Tooltip("Range attack")]
    [Range(0f, 10f)]
    public float rangeAttack;
    [Tooltip("Player layer mask")]
    public LayerMask playerMask;
    [Tooltip("Cooldown of basic attack.")]
    public float timeBtwAttack;
    [Tooltip("Cooldown of strong attack.")]
    public float timeBtwStrongAttack;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float nextAttack;
    private float nextStrongAttack;
    private Collider2D playerToDamage;
    private float damage;
    private Transform transform;

    bool PlayerInArea() => Physics2D.OverlapCircle(attackPosition.position, rangeAttack, playerMask);
    public Collider2D[] PlayersToDamage() => Physics2D.OverlapCircleAll(attackPosition.position, rangeAttack, playerMask);

    public void Initialize(AIEnemy aiEnemy)
    {
        animator = aiEnemy.animator;
        spriteRenderer = aiEnemy.sprite;
        damage = aiEnemy.damage;
        transform = aiEnemy.transform;
    }

    public void Attack(float time)
    {
        FlipAttackPosition();

        if (PlayerInArea() && time >= nextAttack)
        {
            animator.SetTrigger(AIEnemy.ATTACK);
            nextAttack = time + timeBtwAttack;
        }
    }

    void FlipAttackPosition()
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
    }
}
