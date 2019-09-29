using CreaturesAddons;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour, IMove, IInit
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
    [Tooltip("Move force horizontal.")]
    public float moveForce;

    [Header("Setup")]
    [Tooltip("Sprite Renderer to flip.")]
    public SpriteRenderer spriteRenderer;
#pragma warning disable CS0649
    [SerializeField, Tooltip("Layer to check for ground.")]
    private LayerMask ground;
    [SerializeField, Tooltip("Used to check if it's touching ground.")]
    private Transform groundCheck;
#pragma warning restore CS0649
    private const float CHECK_GROUND_DISTANCE = 0.1f;

    [Tooltip("Position attack.")]
    public Transform attackPosition;

    private Rigidbody2D thisRigidbody2D;
    private Animator thisAnimator;

    private static class ANIMATION_STATES
    {
        public const string
            WALK = "Walk",
            JUMP = "Jump";
    }

    void IInit.Init(Creature creature)
    {
        thisRigidbody2D = creature.thisRigidbody2D;
        thisAnimator = creature.animator;
        remainingJumps = maxJumps;
    }

    void IMove.Move(float deltaTime, float speedMultiplier)
    {
        if (IsGrounded())
        {
            remainingJumps = maxJumps;
            thisAnimator.SetBool(ANIMATION_STATES.JUMP, false);
        }

        thisAnimator.SetFloat(ANIMATION_STATES.WALK, Mathf.Abs(thisRigidbody2D.velocity.x));

        if (Input.GetKey(rightKey))
            MoveHorizontal(deltaTime * speedMultiplier);
        if (Input.GetKey(leftKey))
            MoveHorizontal(-deltaTime * speedMultiplier);

        if (Input.GetKeyDown(jumpKey) && remainingJumps > 0)
        {
            thisRigidbody2D.AddForce(thisRigidbody2D.transform.up * jumpStrength * thisRigidbody2D.mass);
            remainingJumps--;
            thisAnimator.SetBool(ANIMATION_STATES.JUMP, true);
        }
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, CHECK_GROUND_DISTANCE, ground);

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