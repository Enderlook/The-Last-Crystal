using CreaturesAddons;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour, IMove, IAwake
{
    [Header("Configuration")]
    [Tooltip("Move right key.")]
    public KeyCode rightKey;
    [Tooltip("Move left key.")]
    public KeyCode leftKey;
    [Tooltip("Movement speed.")]
    public float speed;

    [Tooltip("Jump key.")]
    public KeyCode jumpKey;
    [Tooltip("Maximum number of jumps.")]
    public int maxJumps;
    private int remainingJumps;
    [Tooltip("Jump strength.")]
    public float jumpStrength;
    [Tooltip("Move force horizontal")]
    public float moveForce;

    [Header("Setup")]
    [Tooltip("Sprite Renderer to flip.")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("Collider used to check if it's touching the floor.")]
    public Collider2D baseCollider;
    [Tooltip("Layer of the floor.")]
    public LayerMask floorLayer;
    [Tooltip("Animator for set animation")]
    public Animator animator;
    [Tooltip("Position attack")]
    public Transform attackPosition;

    private Rigidbody2D thisRigidbody2D;

    //Const key for animation
    private const string WALK = "Walk"; 
    private const string JUMP = "Jump"; 

    void IAwake.Awake(Creature creature)
    {
        thisRigidbody2D = creature.thisRigidbody2D;
        remainingJumps = maxJumps;
    }

    void IMove.Move(float deltaTime, float speedMultiplier)
    {
        animator.SetFloat(WALK, Mathf.Abs(thisRigidbody2D.velocity.x));

        if (Input.GetKey(rightKey))
            MoveHorizontal(deltaTime * speedMultiplier);
        if (Input.GetKey(leftKey))
            MoveHorizontal(-deltaTime * speedMultiplier);
        
        if (Input.GetKeyDown(jumpKey) && remainingJumps > 0)
        {
            thisRigidbody2D.AddForce(thisRigidbody2D.transform.up * jumpStrength * thisRigidbody2D.mass);
            remainingJumps--;
            animator.SetBool(JUMP, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.IsTouchingLayers(floorLayer))
        {
            remainingJumps = maxJumps;
            animator.SetBool(JUMP, false);
        }
    }

    private void MoveHorizontal(float distance)
    {
        thisRigidbody2D.AddForce(thisRigidbody2D.transform.right * distance * moveForce);
        if (Mathf.Abs(thisRigidbody2D.velocity.x) > speed)
            thisRigidbody2D.velocity = new Vector2(Mathf.Sign(thisRigidbody2D.velocity.x) * speed, thisRigidbody2D.velocity.y);

        spriteRenderer.flipX = distance < 0;

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
