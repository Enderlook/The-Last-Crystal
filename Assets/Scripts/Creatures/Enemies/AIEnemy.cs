using UnityEngine;
using CreaturesAddons;

public class AIEnemy : MonoBehaviour, IAwake, IUpdate
{
    [Header("Setup")]
    [Tooltip("Target to reach")]
    // False == crystal, True == player
    public bool targetPlayer;
    [Tooltip("Enemy movement.")]
    public AIMovement AIMovement;
    [Tooltip("Enemy attack")]
    public AIAttack AIAttack;
    
    [HideInInspector]
    public Rigidbody2D thisRB2D;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Transform crystal;
    [HideInInspector]
    public Transform platform;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public GameObject[] player;
    [HideInInspector]
    public RaycastHit2D isRightEmpty;
    [HideInInspector]
    public RaycastHit2D isLeftEmpty;
    [HideInInspector]
    public SpriteRenderer sprite;
    [HideInInspector]
    public float damage;

    private const string TARGET = "Crystal";
    private const string PLAYER = "Player";

    //Key Animations
    public const string WALK = "Walk";
    public const string JUMP = "Jump";
    public const string ATTACK = "Attack";

    void IAwake.Awake(Creature creature)
    {
        thisRB2D = creature.thisRigidbody2D;
        animator = creature.animator;
        sprite = creature.sprite;
        crystal = GameObject.FindGameObjectWithTag(TARGET).transform;
        platform = GameObject.Find("Islands").transform;
        player = GameObject.FindGameObjectsWithTag("Player");

        target = targetPlayer ? player[Random.Range(0, player.Length)].transform : crystal;


        AIMovement.Initialize(this);
        AIAttack.Initialize(this);
    }

    void IUpdate.Update(float deltaTime)
    {
        AIMovement.Move();
        if (targetPlayer)
            AIAttack.Attack(Time.time);


        //var currentState = animator.GetCurrentAnimatorStateInfo(0);
        //if (currentState.IsTag(WALK))
        //{
        //    Debug.Log("Hola");
        //}
    }

    void Hit()
    {
        for (int n = 0; n < AIAttack.PlayersToDamage().Length; n++)
        {
            Vector2 direction = (AIAttack.PlayersToDamage()[n].transform.position - transform.position).normalized;
            AIAttack.PlayersToDamage()[n].GetComponent<Creature>().TakeDamage(damage, direction, 30f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(AIMovement.leftPoint.position, AIMovement.rangeCheck);
        Gizmos.DrawWireSphere(AIMovement.rightPoint.position, AIMovement.rangeCheck);
        Gizmos.DrawWireSphere(AIAttack.attackPosition.position, AIAttack.rangeAttack);
    }
}
