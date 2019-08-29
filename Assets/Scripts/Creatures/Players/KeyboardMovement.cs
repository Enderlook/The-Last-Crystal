using CreaturesAddons;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour, IMove, IBuild
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

    [Header("Setup")]
    [Tooltip("Sprite Renderer to flip.")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("Collider used to check if it's touching the floor.")]
    public Collider2D baseCollider;
    [Tooltip("Layer of the floor.")]
    public LayerMask floorLayer;

    private Rigidbody2D thisRigidbody2D;

    private float OVERLAP_CIRCLE_RADIUS = 0.2f;
    private Collider2D[] colliders;

    private void Awake() => remainingJumps = maxJumps;

    void IBuild.Build(Creature creature) => thisRigidbody2D = creature.thisRigidbody2D;

    void IMove.Move(float deltaTime, float speedMultiplier)
    {
        if (Input.GetKey(rightKey))
            MoveHorizontal(deltaTime * speedMultiplier);
        if (Input.GetKey(leftKey))
            MoveHorizontal(-deltaTime * speedMultiplier);
        if (Input.GetKeyDown(jumpKey) && remainingJumps > 0)
        {
            thisRigidbody2D.AddForce(thisRigidbody2D.transform.up * jumpStrength * thisRigidbody2D.mass);
            remainingJumps--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.IsTouchingLayers(floorLayer))
            remainingJumps = maxJumps;
    }

    private void MoveHorizontal(float distance)
    {
        thisRigidbody2D.MovePosition(thisRigidbody2D.position + (Vector2)thisRigidbody2D.transform.right * speed * distance);
        spriteRenderer.flipX = distance < 0;
    }
}
