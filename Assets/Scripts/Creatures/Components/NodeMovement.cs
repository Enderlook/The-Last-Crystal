using System.Collections.Generic;
using AdditionalExtensions;
using CreaturesAddons;
using Navigation;
using UnityEngine;

public class NodeMovement : MonoBehaviour, IInit, IMove
{
#pragma warning disable CS0649
    [Header("Configuration")]
    [SerializeField, Tooltip("Maximum speed movement.")]
    private float speed = 1;

    [SerializeField, Tooltip("Follow player")]
    private bool targetPlayer;

    [Header("Setup")]
    [SerializeField, Tooltip("Navigation agent system.")]
    private NavigationAgent navigationAgent;

    [SerializeField, Layer, Tooltip("Layer to check for ground.")]
    private int ground;
    [SerializeField, Tooltip("Used to check if it's touching ground.")]
    private Transform groundCheck;
#pragma warning restore CS0649

    private const float CHECK_GROUND_DISTANCE = 0.1f;
    private const float MARGIN_ERROR_DISTANCE = 0.1f;
    private const float MARGIN_CLOSE_DISTANCE = 0.25f;

    private Rigidbody2D thisRigidbody2D;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isAirbone = false;
    private BasicClockwork clockWork;
    private const float LANDING_STUN_TIME = 0.4f;

    private Transform goal;
    List<Transform> players;

    private static class ANIMATION_STATES
    {
        public const string
            WALK = "Walk",
            JUMP = "Jump";
    }

    void IInit.Init(Creature creature)
    {
        players = Global.players;
        thisRigidbody2D = creature.thisRigidbody2D;
        spriteRenderer = creature.sprite;
        animator = creature.animator;
        goal = targetPlayer ? players.RandomElement() : Global.crystal;
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
        if (!IsGrounded())
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

        // Get his current goal
        goal = SetGoal();

        // Don't move without goal
        if (goal == null && !targetPlayer)
            return;

        List<Connection> path = navigationAgent.FindPathTo(goal.position);

        float distanceToMove = speed * deltaTime * speedMultiplier;

        // If we aren't already there
        if (path.Count > 0)
        {
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
        {
            float distanceToTarget = XDistanceToTarget(goal.position);
            if (Mathf.Abs(distanceToTarget) > MARGIN_CLOSE_DISTANCE)
            {
                if (distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget))
                    Translate(distanceToTarget);
                else
                    Translate(distanceToMove * Mathf.Sign(distanceToTarget));
            }
        }
    }

    private Transform SetGoal()
    {
        // If all players death. Assign crystal
        if (targetPlayer && players.Count == 0)
            return Global.crystal;

        // If a player revive and the actual goal is crystal. Then assign player
        if (targetPlayer && goal == Global.crystal && players.Count != 0)
            return players.RandomElement();

        // If the current player died. Assign like goal another player that's still alive
        if (targetPlayer && goal == null)
            return players.RandomElement();

        return null;
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, CHECK_GROUND_DISTANCE, 1 << ground);

    private float XDistanceToTarget(Vector2 target) => target.x - thisRigidbody2D.position.x;

    private void Translate(float distance)
    {
        spriteRenderer.flipX = Mathf.Sign(distance) < 0;
        animator.SetBool(ANIMATION_STATES.WALK, true);
        thisRigidbody2D.MovePosition(new Vector2(thisRigidbody2D.position.x + distance, thisRigidbody2D.position.y));
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