using System;
using AdditionalAttributes;
using UnityEngine;

namespace Serializables.Physics
{
    /// <summary>
    /// Used to perform raycasting with predefined parameters.<br>
    /// It can be either serialized in Unity inspector or construct using new.
    /// </summary>
    [Serializable]
    public class RayCasting
    {
#pragma warning disable CS0649, CA2235
        [Tooltip("The starting point of the ray in local coordinates.")]
        public Vector2 source;
        [Tooltip("The direction of the ray.")]
        public Vector2 direction;
        [SerializeField, Tooltip("The max distance the ray should check for collisions.")]
        private float distance = Mathf.Infinity;
        [SerializeField, Tooltip("If nor null, it's position will be used in addition with Source to perform Raycasting")]
        protected Transform referenceTransform;
#pragma warning restore CS0649, CA2235

        [NonSerialized]
        protected Vector2 referenceVector = Vector2.zero;

        protected virtual Vector2 Reference => referenceTransform == null ? referenceVector : (Vector2)referenceTransform.position;
        private Vector2 WorldOrigin {
            get => Source + Reference;
#if UNITY_EDITOR
            set => Source = value - Reference;
#endif
        }

        [SerializeField, Tooltip("Sprite used to check X and Y flip.\nIt's accumulative with Flip X and Flip Y.")]
#pragma warning disable CA2235
        private SpriteRenderer spriteRenderer;
#pragma warning restore CA2235

        [SerializeField, Tooltip("Whenever the ray source and direction should be X flipped.\nThis is accumulative with the Sprite Renderer flip.")]
        private bool flipX;
        public bool FlipX {
            get => spriteRenderer == null ? flipX : Invert(flipX, spriteRenderer.flipX);
            set => flipX = spriteRenderer == null ? flipX : Invert(value, spriteRenderer.flipX);
        }
        [SerializeField, Tooltip("Whenever the ray source and direction should be Y flipped.\nThis is accumulative with the Sprite Renderer flip.")]
        private bool flipY;
        public bool FlipY {
            get => spriteRenderer == null ? flipY : Invert(flipY, spriteRenderer.flipY);
            set => flipY = spriteRenderer == null ? flipY : Invert(value, spriteRenderer.flipY);
        }

        private static bool Invert(bool a, bool b) => a != b;

        public Vector2 Source {
            get => FlipIfNecessary(source);
            set => source = FlipIfNecessary(value);
        }
        public Vector2 Direction {
            get => FlipIfNecessary(direction);
            set => direction = FlipIfNecessary(value);
        }

        private static float Flip(float value, bool invert) => invert ? -value : value;
        private Vector2 FlipIfNecessary(Vector2 value) => new Vector2(Flip(value.x, FlipX), Flip(value.y, FlipY));

        public void SetReference(Transform referencePosition, SpriteRenderer referenceSprite = null)
        {
            referenceTransform = referencePosition;
            spriteRenderer = referenceSprite;
        }
        public void SetReference(Vector2 referencePosition, SpriteRenderer referenceSprite = null)
        {
            referenceVector = referencePosition;
            referenceTransform = null;
            spriteRenderer = referenceSprite;
        }

        public RaycastHit2D Raycast()
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.Raycast(WorldOrigin, Direction, distance);
        }

        public RaycastHit2D Raycast(int layerMask)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.Raycast(WorldOrigin, Direction, distance, layerMask);
        }

        public RaycastHit2D Raycast(int layerMask, int minDepth)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.Raycast(WorldOrigin, Direction, distance, layerMask, minDepth);
        }

        public RaycastHit2D Raycast(int layerMask, int minDepth, int maxDepth)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.Raycast(WorldOrigin, Direction, distance, layerMask, minDepth, maxDepth);
        }

        public int Raycast(ContactFilter2D contactFilter, RaycastHit2D[] results)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.Raycast(WorldOrigin, Direction, contactFilter, results, distance);
        }

        public RaycastHit2D[] RaycastAll()
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastAll(WorldOrigin, Direction, distance);
        }

        public RaycastHit2D[] RaycastAll(int layerMask)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastAll(WorldOrigin, Direction, distance, layerMask);
        }

        public RaycastHit2D[] RaycastAll(int layerMask, int minDepth)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastAll(WorldOrigin, Direction, distance, layerMask, minDepth);
        }

        public RaycastHit2D[] RaycastAll(int layerMask, int minDepth, int maxDepth)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastAll(WorldOrigin, Direction, distance, layerMask, minDepth, maxDepth);
        }

        public int RaycastNonAlloc(RaycastHit2D[] results)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastNonAlloc(WorldOrigin, Direction, results, distance);
        }

        public int RaycastNonAlloc(RaycastHit2D[] results, int layerMask)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastNonAlloc(WorldOrigin, Direction, results, distance, layerMask);
        }

        public int RaycastNonAlloc(RaycastHit2D[] results, int layerMask, int minDepth)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastNonAlloc(WorldOrigin, Direction, results, distance, layerMask, minDepth);
        }

        public int RaycastNonAlloc(RaycastHit2D[] results, int layerMask, int minDepth, int maxDepth)
        {
#if UNITY_EDITOR
            DebugLine();
#endif
            return Physics2D.RaycastNonAlloc(WorldOrigin, Direction, results, distance, layerMask, minDepth, maxDepth);
        }

#if UNITY_EDITOR
#pragma warning disable IDE0051, CS0414, CS0649
        [SerializeField, Tooltip("Draw line in editor."), HideInInspector]
        private bool draw;
        [SerializeField, Tooltip("Edit raycast in editor."), HasConfirmationField(nameof(draw))]
        private bool edit;
        [SerializeField, Tooltip("Whenever a raycast is call, it will display it in scene.")]
        private bool debug;
#pragma warning restore CS0414, CS0649
        [SerializeField]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0052:Quitar miembros privados no leídos", Justification = "It's used by RayCastingDrawer.")]
#pragma warning disable CA2235
        private Color color = Color.red;
#pragma warning restore CA2235

        private Vector2 End {
            get => Source + Direction * distance;
            set {
                Vector2 end = value - Source;
                Direction = end.normalized;
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
}