using AdditionalAttributes;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif
using RangeAttribute = UnityEngine.RangeAttribute;

public class GroundChecker : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField, Tooltip("Ground checking raycast."), DrawVectorRelativeToTransform]
    private Vector2 position;

    [SerializeField, Tooltip("Checking radius."), Range(0, 1)]
    private float radius;

    [SerializeField, Tooltip("Layer of ground."), Layer]
    private int groundLayer;
#pragma warning restore CS0649

    private Vector2 Position => (Vector2)transform.position + position;

    private int GroundLayer => 1 << groundLayer;
    
    public bool IsGrounded() => Physics2D.OverlapCircle(Position, radius, GroundLayer);

    public bool IsGrounded(Vector2 offset) => Physics2D.OverlapCircle(Position + offset, radius, GroundLayer);

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position, radius);
    }
#endif
}
