using Additions.Components.ScriptableSound;
using Additions.Utils;

using Creatures;

using System;

using UnityEngine;

namespace PlayerAddons
{
    public class KeyboardMovement : MonoBehaviour, IMove, IInitialize<Creature>
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Move right key.")]
        private KeyCode rightKey;

        [SerializeField, Tooltip("Move left key.")]
        private KeyCode leftKey;

        [SerializeField, Tooltip("Movement speed.")]
        private float speed;

        [SerializeField, Tooltip("Jump key.")]
        private KeyCode jumpKey;

        [SerializeField, Tooltip("Maximum number of jumps.")]
        private int maxJumps;

        private int remainingJumps;

        [SerializeField, Tooltip("Jump strength.")]
        private float jumpStrength;

        [SerializeField, Tooltip("Move force horizontal.")]
        private float moveForce;

        [Header("Setup")]
        [SerializeField, Tooltip("Sound played when jump.")]
        private SoundPlay jumpSound;
#pragma warning restore CS0649

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

        void IInitialize<Creature>.Initialize(Creature creature)
        {
            thisRigidbody2D = creature.ThisRigidbody2D;
            thisAnimator = creature.Animator;
            thisSprite = creature.Sprite;
            groundChecker = creature.GroundChecker;
            remainingJumps = maxJumps;
            jumpSound.Init();
        }

        void IMove.Move(float deltaTime, float speedMultiplier)
        {
            jumpSound.UpdateBehaviour(deltaTime);
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
                jumpSound.Play();
            }
        }

        private void MoveHorizontal(float distance)
        {
            thisSprite.flipX = distance < 0;
            Vector3 impulse = thisRigidbody2D.transform.right * distance * moveForce;
            Vector3 acceleration = impulse * thisRigidbody2D.mass;
            if (Math.Abs(thisRigidbody2D.velocity.x + acceleration.x) <= speed)
                thisRigidbody2D.AddForce(impulse);
            else if (Math.Abs(thisRigidbody2D.velocity.x) < speed)
                thisRigidbody2D.AddForce((acceleration - new Vector3(speed, 0)) / thisRigidbody2D.mass);
        }
    }
}