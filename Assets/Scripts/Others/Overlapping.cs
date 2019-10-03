using System;
using UnityEngine;

[Serializable]
public class Overlapping
{
#pragma warning disable CS0649
    [SerializeField, Tooltip("The starting point of the ray in local coordinates.")]
    private Vector2 source;
    [SerializeField, Tooltip("Range of OverlapCircle.")]
    [Range(0f, 10f)]
    private float range;
    [SerializeField, Tooltip("If nor null, it's position will be used in addition with Source to perform Raycasting")]
    private Transform referenceTransform;
#pragma warning restore CS0649

    private Vector2 referenceVector = Vector2.zero;

    private Vector2 Reference => referenceTransform == null ? referenceVector : (Vector2)referenceTransform.position;
    private Vector2 WorldOrigin
    {
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

    public Collider2D OverlapCircle()
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircle(WorldOrigin, range);
    }

    public Collider2D OverlapCircle(int layerMask)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircle(WorldOrigin, range, layerMask);
    }

    public Collider2D OverlapCircle(int layerMask, int minDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircle(WorldOrigin, range, layerMask, minDepth);
    }

    public Collider2D OverlapCircle(int layerMask, int minDepth, int maxDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircle(WorldOrigin, range, layerMask, minDepth, maxDepth);
    }

    public int OverlapCircle(ContactFilter2D contactFilter, Collider2D[] results)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircle(WorldOrigin, range, contactFilter, results);
    }

    public Collider2D[] OverlapCircleAll()
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleAll(WorldOrigin, range);
    }

    public Collider2D[] OverlapCircleAll(int layerMask)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleAll(WorldOrigin, range, layerMask);
    }

    public Collider2D[] OverlapCircleAll(int layerMask, int minDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleAll(WorldOrigin, range, layerMask, minDepth);
    }

    public Collider2D[] OverlapCircleAll(int layerMask, int minDepth, int maxDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleAll(WorldOrigin, range, layerMask, minDepth, maxDepth);
    }

    public int OverlapCircleNonAlloc(Collider2D[] results)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleNonAlloc(WorldOrigin, range, results);
    }

    public int OverlapCircleNonAlloc(Collider2D[] results, int layerMask)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleNonAlloc(WorldOrigin, range, results, layerMask);
    }

    public int OverlapCircleNonAlloc(Collider2D[] results, int layerMask, int minDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleNonAlloc(WorldOrigin, range, results, layerMask, minDepth);
    }

    public int OverlapCircleNonAlloc(Collider2D[] results, int layerMask, int minDepth, int maxDepth)
    {
#if UNITY_EDITOR
        DebugLine();
#endif
        return Physics2D.OverlapCircleNonAlloc(WorldOrigin, range, results, layerMask, minDepth, maxDepth);
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

    private void DebugLine()
    {
        if (debug)
            DrawCircle();
    }
    public void DrawCircle()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(WorldOrigin, range);
    }
#pragma warning restore IDE0051
#endif
}