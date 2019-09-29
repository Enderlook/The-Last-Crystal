using System;
using UnityEngine;

[Serializable]
public class RayCasting
{
#pragma warning disable CS0649
    [SerializeField, Tooltip("The starting point of the ray in local coordinates.")]
    private Vector2 source;
    [SerializeField, Tooltip("The direction of the ray.")]
    private Vector2 direction;
    [SerializeField, Tooltip("The max distance the ray should check for collisions.")]
    private float distance = Mathf.Infinity;
    [SerializeField, Tooltip("If nor null, it's position will be used in addition with Source to perform Raycasting")]
    private Transform referenceTransform;
#pragma warning restore CS0649

    private Vector2 referenceVector = Vector2.zero;

    private Vector2 Reference => referenceTransform == null ? referenceVector : (Vector2)referenceTransform.position;
    private Vector2 WorldOrigin {
        get => source + Reference;
#if UNITY_EDITOR
        set => source = value - Reference;
#endif
    }

    public void SetReference(Transform reference)
    {
        referenceTransform = reference;
        referenceVector = Vector2.zero;
    }
    public void SetReference(Vector2 reference)
    {
        referenceVector = reference;
        referenceTransform = null;
    }

    public RaycastHit2D Raycast()
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.Raycast(WorldOrigin, direction, distance);
    }

    public RaycastHit2D Raycast(int layerMask)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.Raycast(WorldOrigin, direction, distance, layerMask);
    }

    public RaycastHit2D Raycast(int layerMask, int minDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.Raycast(WorldOrigin, direction, distance, layerMask, minDepth);
    }

    public RaycastHit2D Raycast(int layerMask, int minDepth, int maxDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.Raycast(WorldOrigin, direction, distance, layerMask, minDepth, maxDepth);
    }

    public int Raycast(ContactFilter2D contactFilter, RaycastHit2D[] results)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.Raycast(WorldOrigin, direction, contactFilter, results, distance);
    }

    public RaycastHit2D[] RaycastAll()
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastAll(WorldOrigin, direction, distance);
    }

    public RaycastHit2D[] RaycastAll(int layerMask)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastAll(WorldOrigin, direction, distance, layerMask);
    }

    public RaycastHit2D[] RaycastAll(int layerMask, int minDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastAll(WorldOrigin, direction, distance, layerMask, minDepth);
    }

    public RaycastHit2D[] RaycastAll(int layerMask, int minDepth, int maxDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastAll(WorldOrigin, direction, distance, layerMask, minDepth, maxDepth);
    }

    public int RaycastNonAlloc(RaycastHit2D[] results)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastNonAlloc(WorldOrigin, direction, results, distance);
    }

    public int RaycastNonAlloc(RaycastHit2D[] results, int layerMask)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastNonAlloc(WorldOrigin, direction, results, distance, layerMask);
    }

    public int RaycastNonAlloc(RaycastHit2D[] results, int layerMask, int minDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastNonAlloc(WorldOrigin, direction, results, distance, layerMask, minDepth);
    }

    public int RaycastNonAlloc(RaycastHit2D[] results, int layerMask, int minDepth, int maxDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.RaycastNonAlloc(WorldOrigin, direction, results, distance, layerMask, minDepth, maxDepth);
    }

#if UNITY_EDITOR
#pragma warning disable IDE0051
#pragma warning disable CS0414
    [SerializeField]
    private bool draw = false;
#pragma warning restore CS0414
    [SerializeField, Tooltip("Whenever a raycast is call, it will display it in scene.")]
    private bool debug = false;
    [SerializeField]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0052:Quitar miembros privados no leídos", Justification = "It's used by RayCastingDrawer.")]
    private Color color = Color.red;

    private Vector2 End {
        get => source + direction * distance;
        set {
            Vector2 end = value - source;
            direction = end.normalized;
            distance = end.magnitude;
        }
    }
    private Vector2 WorldEnd {
        get => End + Reference;
        set => End = value - Reference;
    }

    private void DebugLine()
    {
        if (debug)
            DrawLine();
    }
    public void DrawLine()
    {
        if (distance == Mathf.Infinity)
            Debug.DrawRay(WorldOrigin, direction, color);
        else
            Debug.DrawLine(WorldOrigin, WorldEnd, color);
    }
#pragma warning restore IDE0051
#endif
}
