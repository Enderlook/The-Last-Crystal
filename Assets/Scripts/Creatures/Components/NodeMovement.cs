using CreaturesAddons;
using Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeMovement : MonoBehaviour, IAwake
{
    [Header("Configuration")]
    [SerializeField, Tooltip("Maximum speed movement.")]
    private float speed;
    [SerializeField, Tooltip("Speed acceleration.")]
    private float acceleration;

    [Header("Setup")]
    [SerializeField, Tooltip("Navigation agent system.")]
    private NavigationAgent navigationAgent;
    [SerializeField, Tooltip("Layer to check for ground.")]
    private LayerMask ground;

    private Rigidbody2D thisRigidbody2D;
    private bool isAirbone = false;

    void IAwake.Awake(Creature creature) => thisRigidbody2D = creature.thisRigidbody2D;

    private void Update()
    {
        // We are flying...
        if (isAirbone)
            return;

        // Find mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Node mouseNode = navigationAgent.navigationGraph.FindClosestNode(mousePosition);
        
        List<Connection> path = navigationAgent.FindPathTo(mouseNode);

        // If we aren't already there
        if (path.Count > 0)
        {
            Connection connection = path[0];
            Vector2 target = connection.end.position;

            if (connection.IsExtreme)
            {
                isAirbone = true;
                JumpTo(target);
            }
            else
                MoveTo(target);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*Debug.Log(collision.gameObject.layer);
        Debug.Log(ground.value);
        Debug.Log(collision.gameObject.layer == ground.ToLayer());*/
        if (collision.gameObject.layer == ground.ToLayer())
            isAirbone = false;
    }

    private void MoveTo(Vector2 target)
    {
        float distance = target.x - thisRigidbody2D.position.x;
        float toMove = speed * Time.deltaTime;
        if (Mathf.Abs(distance) > toMove)
            thisRigidbody2D.MovePosition(new Vector2(thisRigidbody2D.position.x + toMove * Mathf.Sign(distance), thisRigidbody2D.position.y));
        else
            thisRigidbody2D.MovePosition(new Vector2(thisRigidbody2D.position.x + distance, thisRigidbody2D.position.y));
    }

    private void JumpTo(Vector2 target)
    {
        thisRigidbody2D.velocity = ProjectileMotion(target, thisRigidbody2D.position, 1f);
    }

    private float GetTg(Vector2 target, Vector2 origin)
    {
        float Atg(float tg) => Mathf.Atan(tg) * 180 / Mathf.PI;

        Vector2 tO = target - origin;
        float magnitude = tO.magnitude;

        float tan = tO.y / tO.x;

        return Mathf.Round(Atg(tan));

    }

    private float GetSin(Vector2 target, Vector2 origin)
    {
        float Asin(float s) => Mathf.Asin(s) * 180 / Mathf.PI;

        Vector2 tO = target - origin;
        float magnitude = tO.magnitude;

        float sin = tO.y / magnitude;

        return Mathf.Round(Asin(sin));

    }

    private float GetCos(Vector2 target, Vector2 origin)
    {
        float Acos(float c) => Mathf.Acos(c) * 180 / Mathf.PI;

        Vector2 tO = target - origin;
        float magnitude = tO.magnitude;
        var dir = tO / magnitude;
        float cos = tO.x / magnitude;
        float result = dir.x >= 0 ? Mathf.Round(Acos(cos)) : Mathf.Round(Acos(-cos));
        return result;

    }

    private Vector2 ProjectileMotion(Vector2 target, Vector2 origin, float t)
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