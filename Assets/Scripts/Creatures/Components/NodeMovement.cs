using System.Collections.Generic;
using CreaturesAddons;
using Navigation;
using UnityEngine;

public class NodeMovement : MonoBehaviour, IAwake, IMove
{
    [Header("Configuration")]
    [SerializeField, Tooltip("Maximum speed movement.")]
    private float speed = 1;

#pragma warning disable CS0649
    [Header("Setup")]
    [SerializeField, Tooltip("Navigation agent system.")]
    private NavigationAgent navigationAgent;

    [SerializeField, Tooltip("Layer to check for ground.")]
    private LayerMask ground;
    [SerializeField, Tooltip("Used to check if it's touching ground.")]
    private Transform groundCheck;
#pragma warning restore CS0649

    private const float CHECK_GROUND_DISTANCE = 0.1f;
    private const float MARGIN_ERROR_DISTANCE = 0.1f;

    private Rigidbody2D thisRigidbody2D;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Transform goal;

    private static class ANIMATION_STATES
    {
        public const string
            WALK = "Walk",
            JUMP = "Jump";
    }

    void IAwake.Awake(Creature creature)
    {
        thisRigidbody2D = creature.thisRigidbody2D;
        spriteRenderer = creature.GetComponent<SpriteRenderer>();
        animator = creature.GetComponent<Animator>();
        goal = Global.crystal;
    }

    void IMove.Move(float deltaTime, float speedMultiplier)
    {
        // Don't move while airbone
        if (!IsGrounded())
            return;

        animator.SetBool(ANIMATION_STATES.JUMP, false);

        // Don't move without goal
        if (goal == null)
            return;

        List<Connection> path = navigationAgent.FindPathTo(goal.position);

        // If we aren't already there
        if (path.Count > 0)
        {
            Connection connection = path[0];
            Vector2 target = connection.end.position;

            float distanceToMove = speed * deltaTime * speedMultiplier;
            float distanceToTarget = XDistanceToTarget(target);

            if (distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget) && path.Count > 1)
            {
                // We are too close to this target, better get a new one in order to not get stuck
                connection = path[1];
                target = connection.end.position;

                distanceToTarget = XDistanceToTarget(target);
                distanceToMove = distanceToMove + MARGIN_ERROR_DISTANCE > Mathf.Abs(distanceToTarget) ? distanceToTarget : distanceToMove * Mathf.Sign(distanceToTarget);
            }

            spriteRenderer.flipX = Mathf.Sign(distanceToTarget) < 0;

            if (connection.IsExtreme)
                JumpTo(connection.end.position);
            else
                Translate(distanceToMove * Mathf.Sign(distanceToTarget));
        }
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, CHECK_GROUND_DISTANCE, ground);

    private float XDistanceToTarget(Vector2 target) => target.x - thisRigidbody2D.position.x;

    private void Translate(float distance)
    {
        animator.SetBool(ANIMATION_STATES.WALK, true);
        thisRigidbody2D.MovePosition(new Vector2(thisRigidbody2D.position.x + distance, thisRigidbody2D.position.y));
    }

    private void JumpTo(Vector2 target)
    {
        animator.SetBool(ANIMATION_STATES.JUMP, true);
        thisRigidbody2D.velocity = ProjectileMotion(target, thisRigidbody2D.position, 1f);
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

    private static Vector2 ProjectileMotion(Vector2 target, Vector2 origin, float t)
    {
        float Vx(float x) => x / Mathf.Cos(GetCos(target, origin) / 180 * Mathf.PI) * t;
        float Vy(float y) => y / Mathf.Abs(Mathf.Sin(GetSin(target, origin) / 180 * Mathf.PI)) * t + .5f * Mathf.Abs(Physics2D.gravity.y) * t;

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