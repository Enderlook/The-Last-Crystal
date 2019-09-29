using UnityEngine;
using CreaturesAddons;

public class EnemyAI : MonoBehaviour, IAwake
{
    [Header("Enemy Stats")]
    [Tooltip("Movement force.")]
    public float moveForce;
    [Tooltip("Distance that enemy stop when reach target.")]
    [Range(0f, 50f)]
    public float stopDistance;
    [Tooltip("Max speed that enemy can reach.")]
    [Range(0f, 10f)]
    public float maxSpeed;

    [Header("Setup")]
    [Tooltip("Right point to check is no ground.")]
    public Transform rightPoint;
    [Tooltip("Left point to check is no ground.")]
    public Transform leftPoint;
    [Tooltip("Target to reach")]
    // False == crystal, True == player
    public bool targetPlayer;

    private Rigidbody2D thisRB2D;
    private Animator animator;
    private Transform crystal;
    private Transform platform;
    private Transform target;
    private GameObject[] player;
    private RaycastHit2D isRightEmpty;
    private RaycastHit2D isLeftEmpty;
    private SpriteRenderer spriteRenderer;

    private const string WALK = "Walk";
    private const string JUMP = "Jump";
    private const string TARGET = "Crystal";
    private const string PLAYER = "Player";

    void IAwake.Awake(Creature creature)
    {
        thisRB2D = creature.thisRigidbody2D;
        animator = creature.animator;
        spriteRenderer = creature.sprite;
        crystal = GameObject.FindGameObjectWithTag(TARGET).transform;
        platform = GameObject.Find("Islands").transform;
        player = GameObject.FindGameObjectsWithTag("Player");

        target = targetPlayer ? player[Random.Range(0, player.Length)].transform : crystal;
    }

    protected virtual void Update()
    {
        if (target != null)
            CheckEdge();
    }

    void CheckEdge()
    {
        isRightEmpty = Physics2D.Linecast(transform.position, rightPoint.position,
            1 << LayerMask.NameToLayer("Ground"));
        isLeftEmpty = Physics2D.Linecast(transform.position, leftPoint.position,
            1 << LayerMask.NameToLayer("Ground"));

        if (!isRightEmpty && !isLeftEmpty)
            animator.SetBool(WALK, false);

        if (isRightEmpty && isLeftEmpty)
            ReachTarget();

        if (!isRightEmpty && isLeftEmpty)
            JumpPlatforms(isLeftEmpty.transform);

        if (isRightEmpty && !isLeftEmpty)
            JumpPlatforms(isRightEmpty.transform);
    }

    void ReachTarget()
    {
        animator.SetBool(JUMP, false);
        //if (!targetPlayer)
        //{
            
        //    float distPlayerOne = Vector2.Distance(transform.position, player[0].transform.position);
        //    float distPlayerTwo = Vector2.Distance(transform.position, player[1].transform.position);
        //    if (distPlayerOne < 2f)
        //    {
        //        MoveToTarget(player[0].transform);
        //    } else if (distPlayerTwo < 2f)
        //    {
        //        MoveToTarget(player[1].transform);
        //    }
        //}

        float dist = Vector2.Distance(transform.position, target.position);
        spriteRenderer.flipX = target.position.x < transform.position.x;
        if (dist > stopDistance)
        {
            MoveToTarget(target);
        }
    }

    void MoveToTarget(Transform target)
    {
        animator.SetBool(WALK, true);
        if (Mathf.Abs(thisRB2D.velocity.x) > maxSpeed)
            thisRB2D.velocity = new Vector2(Mathf.Sign(thisRB2D.velocity.x) * maxSpeed,
                thisRB2D.velocity.y);

        Vector2 objective = target.position - transform.position;
        thisRB2D.AddForce(Vector2.right * objective.normalized * moveForce);
    }
    
    void JumpPlatforms(Transform t)
    {
        thisRB2D.velocity = new Vector2(0f, thisRB2D.velocity.y);
        float closePlatformDist = 999f;
        Transform closePlatform = null;
        foreach (Transform child in platform)
        {
            if (child.name != t.name)
            {
                float dist = Vector2.Distance(transform.position, child.position);
                if (dist < closePlatformDist)
                {
                    closePlatformDist = dist;
                    closePlatform = child;
                }
            }

            
        }

        animator.SetBool(JUMP, true);
        Vector2 v0 = ProjectileMotion(closePlatform, transform);
        thisRB2D.velocity = v0;
    }

    float GetTg(Transform target, Transform origin)
    {
        float Atg(float tg) => Mathf.Atan(tg) * 180 / Mathf.PI;

        Vector2 tO = target.position - origin.position;
        float magnitude = tO.magnitude;

        float tan = tO.y / tO.x;

        return Mathf.Round(Atg(tan));

    }

    float GetSin(Transform target, Transform origin)
    {
        float Asin(float s) => Mathf.Asin(s) * 180 / Mathf.PI;

        Vector2 tO = target.position - origin.position;
        float magnitude = tO.magnitude;

        float sin = tO.y / magnitude;

        float result = Asin(sin);

        return result;

    }

    float GetCos(Transform target, Transform origin)
    {
        float Acos(float c) => Mathf.Acos(c) * 180 / Mathf.PI;

        Vector2 tO = target.position - origin.position;
        float magnitude = tO.magnitude;
        var dir = tO / magnitude;
        float cos = tO.x / magnitude;
        float result = dir.x >= 0 ? Mathf.Round(Acos(cos)) : Mathf.Round(Acos(-cos));
        return result;

    }

    Vector2 ProjectileMotion(Transform target, Transform origin)
    {
        float Vx(float x) => x / Mathf.Cos(GetCos(target, origin) / 180 * Mathf.PI);
        float Vy(float y) => y / Mathf.Abs(Mathf.Sin(GetSin(target, origin) / 180 * Mathf.PI)) + .5f * Mathf.Abs(Physics2D.gravity.y);

        Vector2 magnitude = target.position - origin.position;
        Vector2 distX = magnitude;
        distX.y = 0;

        float hY = magnitude.y;
        float wX = distX.magnitude;

        Vector2 v0 = distX.normalized;
        v0 *= Vx(wX);
        v0.y = Vy(hY);

        return v0;
    }
}
