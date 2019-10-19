﻿using System.Collections.Generic;
using Navigation;
using UnityEngine;

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
        private NavigationAgent navigationAgent;

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
            navigationAgent = GetComponent<NavigationAgent>();
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

                float distanceToTarget = XDistanceToTarget(target);

                if (distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget) && path.Count > 1)
                {
                    // We are too close to this target, better get a new one in order to not get stuck
                    connection = path[1];
                    target = connection.end.position;

                    distanceToTarget = XDistanceToTarget(target);
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
                float distanceToTarget = XDistanceToTarget(targetTransform.position);
                if (Mathf.Abs(distanceToTarget) > MARGIN_CLOSE_DISTANCE)
                {
                    if (distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget))
                        Translate(distanceToTarget);
                    else
                        Translate(distanceToMove * Mathf.Sign(distanceToTarget));
                }
            }
        }

        private float XDistanceToTarget(Vector2 target) => target.x - thisRigidbody2D.position.x;

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
            thisRigidbody2D.velocity = ProjectileMotion(target, thisRigidbody2D.position);
        }

        private static float GetTg(Vector2 target, Vector2 origin)
        {
            float Atg(float tg) => Mathf.Atan(tg) * 180 / Mathf.PI;
            Vector2 tO = target - origin;
            float tan = tO.y / tO.x;
            return Mathf.Round(Atg(tan));
        }

        private static float GetSin(Vector2 target, Vector2 origin)
        {
            float Asin(float s) => Mathf.Asin(s) * 180 / Mathf.PI;
            Vector2 tO = target - origin;
            float magnitude = tO.magnitude;
            float sin = tO.y / magnitude;
            return Mathf.Round(Asin(sin));
        }

        private static float GetCos(Vector2 target, Vector2 origin)
        {
            float Acos(float c) => Mathf.Acos(c) * 180 / Mathf.PI;
            Vector2 tO = target - origin;
            float magnitude = tO.magnitude;
            Vector2 dir = tO / magnitude;
            float cos = tO.x / magnitude;
            float result = dir.x >= 0 ? Mathf.Round(Acos(cos)) : Mathf.Round(Acos(-cos));
            return result;
        }

        private static Vector2 ProjectileMotion(Vector2 target, Vector2 origin)
        {
            float Vx(float x) => x / Mathf.Cos(GetCos(target, origin) / 180 * Mathf.PI);
            float Vy(float y) => y / Mathf.Abs(Mathf.Sin(GetSin(target, origin) / 180 * Mathf.PI)) + .5f * Mathf.Abs(Physics2D.gravity.y);

            Vector2 magnitude = target - origin;
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
}