using UnityEngine;
using CreaturesAddons;

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
    [Tooltip("Right point to check is no ground.")]
    public Transform rightPoint;
    [Tooltip("Left point to check is no ground.")]
    public Transform leftPoint;

    private Rigidbody2D thisRB2D;
    private Animator animator;
    private Transform target;
    private Transform platform;
    private GameObject player;
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
        target = GameObject.FindGameObjectWithTag(TARGET).transform;
        platform = GameObject.Find("Islands").transform;
        DistanceFunction(target, transform);
    }

    /*protected virtual void Update()
    {
        if (target != null)
            CheckEdge();
    }*/

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
        float dist = Vector2.Distance(transform.position, target.position);
        if (dist > stopDistance)
        {
            animator.SetBool(WALK, true);
            Vector2 objective = target.position - transform.position;
            thisRB2D.AddForce(Vector2.right * objective.normalized * moveForce);

            if (Mathf.Abs(thisRB2D.velocity.x) > maxSpeed)
                thisRB2D.velocity = new Vector2(Mathf.Sign(thisRB2D.velocity.x) * maxSpeed,
                    thisRB2D.velocity.y);
        }
    }
    
    void JumpPlatforms(Transform t)
    {
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
        Vector2 objective = closePlatform.position - transform.position;
        thisRB2D.AddForce(new Vector2(objective.normalized.x * (moveForce * 2), 
            Mathf.Abs(objective.normalized.y) * jumpForce));
    }

    void DistanceFunction(Transform target, Transform origin)
    {
        float pX = Mathf.Pow(target.position.x - origin.position.x, 2);
        float pY = Mathf.Pow(target.position.y - origin.position.y, 2);

        float sumXY = pX + pY;

        float distF = Mathf.Sqrt(sumXY);

        float direction = Vector2.Angle(target.position, origin.position);

        Debug.Log($"Tan {Mathf.Tan(pY / pX)}, Sin {Mathf.Sin(pY / distF)}, cos {Mathf.Cos(pX / distF)}");
        Debug.Log($"Direction: {direction}");

    }
}
