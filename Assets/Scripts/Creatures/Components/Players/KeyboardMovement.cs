using CreaturesAddons;
using UnityEngine;

namespace PlayerAddons
{
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
        private Rigidbody2D thisRigidbody2D;
        private Animator thisAnimator;
        private SpriteRenderer thisSprite;

        private GroundChecker groundChecker;

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
            thisSprite = creature.sprite;
            groundChecker = creature.groundChecker;
            remainingJumps = maxJumps;
        }

        void IMove.Move(float deltaTime, float speedMultiplier)
        {
            if (groundChecker.IsGrounded())
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

        private void MoveHorizontal(float distance)
        {
            thisSprite.flipX = distance < 0;
            thisRigidbody2D.AddForce(thisRigidbody2D.transform.right * distance * moveForce);
            if (Mathf.Abs(thisRigidbody2D.velocity.x) > speed)
                thisRigidbody2D.velocity = new Vector2(Mathf.Sign(thisRigidbody2D.velocity.x) * speed, thisRigidbody2D.velocity.y);
        }
    }
}