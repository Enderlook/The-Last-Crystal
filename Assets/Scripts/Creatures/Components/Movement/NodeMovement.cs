using System.Collections.Generic;
using Navigation;
using UnityEngine;
using Utils;
using AdditionalExtensions;

namespace CreaturesAddons.Movement.NodeMovement
{
    [RequireComponent(typeof(TargetAndPathGetter)), RequireComponent(typeof(NavigationAgent))]
    public class NodeMovement : MonoBehaviour, IInit, IMove
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Maximum speed movement.")]
        private float speed = 1;
#pragma warning restore CS0649

        private const float MARGIN_ERROR_DISTANCE = 0.1f;
        private const float MARGIN_CLOSE_DISTANCE = 0.25f;

        private Rigidbody2D thisRigidbody2D;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private bool isAirbone;
        private BasicClockwork clockWork;
        private const float LANDING_STUN_TIME = 0.4f;

        private TargetAndPathGetter targetAndPathGetter;

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
            spriteRenderer = creature.sprite;
            animator = creature.animator;
            groundChecker = creature.groundChecker;
            targetAndPathGetter = GetComponent<TargetAndPathGetter>();
        }

        void IMove.Move(float deltaTime, float speedMultiplier)
        {
            // Don't move while stunned
            if (clockWork != null)
            {
                if (clockWork.Recharge(deltaTime))
                    clockWork = null;
                else
                    return;
            }
            // Don't move while airbone
            if (!groundChecker.IsGrounded())
            {
                isAirbone = true;
                return;
            }
            else if (isAirbone)
            {
                isAirbone = false;
                clockWork = new BasicClockwork(LANDING_STUN_TIME);
            }

            animator.SetBool(ANIMATION_STATES.JUMP, false);

            List<Connection> path = targetAndPathGetter.GetPath(out Transform targetTransform);

            // Just to be sure that we really have a target
            if (targetTransform == null)
                return;

            float distanceToMove = speed * deltaTime * speedMultiplier;

            if (path.Count > 0)
            {
                // If we aren't already there
                Connection connection = path[0];
                Vector2 target = connection.end.position;

                float distanceToTarget = thisRigidbody2D.position.XDistance(target);

                if (distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget) && path.Count > 1)
                {
                    // We are too close to this target, better get a new one in order to not get stuck
                    connection = path[1];
                    target = connection.end.position;

                    distanceToTarget = thisRigidbody2D.position.XDistance(target);
                    distanceToMove = distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget) ? distanceToTarget : distanceToMove * Mathf.Sign(distanceToTarget);
                }

                if (connection.IsExtreme)
                    JumpTo(connection.end.position);
                else
                    Translate(distanceToMove * Mathf.Sign(distanceToTarget));
            }
            else
            // If we are less than one node of distance from it
            {
                float distanceToTarget = thisRigidbody2D.position.XDistance(targetTransform.position);
                if (Mathf.Abs(distanceToTarget) > MARGIN_CLOSE_DISTANCE)
                {
                    if (distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget))
                        Translate(distanceToTarget);
                    else
                        Translate(distanceToMove * Mathf.Sign(distanceToTarget));
                }
            }
        }

        private void Translate(float distance)
        {
            spriteRenderer.flipX = Mathf.Sign(distance) < 0;
            // Is our next position still on ground?
            if (groundChecker.IsGrounded(Vector2.right * distance))
            {
                animator.SetBool(ANIMATION_STATES.WALK, true);
                thisRigidbody2D.MovePosition(new Vector2(thisRigidbody2D.position.x + distance, thisRigidbody2D.position.y));
            }
        }

        private void JumpTo(Vector2 target)
        {
            animator.SetBool(ANIMATION_STATES.JUMP, true);
            thisRigidbody2D.velocity = thisRigidbody2D.position.ProjectileMotion(target);
        }
    }
}