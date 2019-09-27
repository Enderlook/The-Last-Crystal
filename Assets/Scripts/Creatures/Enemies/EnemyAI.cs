using UnityEngine;
using CreaturesAddons;
using System;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour, IAwake
{
    [Header("Enemy Stats")]
    [Tooltip("Jump force.")]
    public float jumpForce;
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
    [Tooltip("Sprite Renderer component.")]
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D thisRB2D;
    private Animator animator;
    private Transform crystal;
    private Transform platform;
    private Transform target;
    private GameObject[] player;
    private RaycastHit2D isRightEmpty;
    private RaycastHit2D isLeftEmpty;

    private const string WALK = "Walk";
    private const string JUMP = "Jump";
    private const string TARGET = "Crystal";
    private const string PLAYER = "Player";

    void IAwake.Awake(Creature creature)
    {
        thisRB2D = creature.thisRigidbody2D;
        animator = creature.animator;
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
        if (!targetPlayer)
        {
            float distEnemy = Vector2.Distance(transform.position, target.position);
            if (distEnemy > 1.5f)
            {
                MoveToTarget(target);
            }
        }

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
        Vector2 objective = target.position - transform.position;
        thisRB2D.AddForce(Vector2.right * objective.normalized * moveForce);

        if (Mathf.Abs(thisRB2D.velocity.x) > maxSpeed)
            thisRB2D.velocity = new Vector2(Mathf.Sign(thisRB2D.velocity.x) * maxSpeed,
                thisRB2D.velocity.y);
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
        Vector2 v0 = ProjectileMotion(closePlatform, transform, 1f);
        thisRB2D.velocity = v0;
    }

    float GetTg(Transform target, Transform origin)
    {
        Func<float, float> atg = tg => Mathf.Atan(tg) * 180 / Mathf.PI;

        Vector2 tO = target.position - origin.position;
        float magnitude = tO.magnitude;

        float tan = tO.y / tO.x;

        return Mathf.Round(atg(tan));

    }

    float GetSin(Transform target, Transform origin)
    {
        Func<float, float> asin = s => Mathf.Asin(s) * 180 / Mathf.PI;

        Vector2 tO = target.position - origin.position;
        float magnitude = tO.magnitude;

        float sin = tO.y / magnitude;

        float result = asin(sin);

        return result;

    }

    float GetCos(Transform target, Transform origin)
    {
        Func<float, float> acos = c => Mathf.Acos(c) * 180 / Mathf.PI;

        Vector2 tO = target.position - origin.position;
        float magnitude = tO.magnitude;
        var dir = tO / magnitude;
        float cos = tO.x / magnitude;
        float result = dir.x >= 0 ? Mathf.Round(acos(cos)) : Mathf.Round(acos(-cos));
        return result;

    }

    Vector2 ProjectileMotion(Transform target, Transform origin, float t)
    {
        Func<float, float> vX = x => x / Mathf.Cos(GetCos(target, origin) / 180 * Mathf.PI) * t;
        Func<float, float> vY = y => y / Mathf.Abs(Mathf.Sin(GetSin(target, origin) / 180 * Mathf.PI)) * t + .5f * Mathf.Abs(Physics2D.gravity.y) * t;

        Vector2 magnitude = target.position - origin.position;
        Vector2 distX = magnitude;
        distX.y = 0;

        float hY = magnitude.y;
        float wX = distX.magnitude;

        Vector2 v0 = distX.normalized;
        v0 *= vX(wX);
        v0.y = vY(hY);

        return v0;
    }
}
