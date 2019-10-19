using AdditionalAttributes;
using CreaturesAddons;
using Serializables.Physics;
using UnityEngine;

public class GroundChecker : MonoBehaviour, IInit
{
#pragma warning disable CS0649
    [SerializeField, Tooltip("Ground checking raycast.")]
    private RayCastingWithOffset checker;

    [SerializeField, Tooltip("Layer of ground."), Layer]
    private int groundLayer;
#pragma warning restore CS0649

    private int GroundLayer => 1 << groundLayer;

    public void Init(Creature creature) => checker.SetReference(transform);

    public bool IsGrounded() => checker.Raycast(GroundLayer);

    public bool IsGrounded(Vector2 offset)
    {
        checker.SetOffset(offset);
        bool isGrounded = checker.Raycast(GroundLayer);
        checker.ResetOffset();
        return isGrounded;
    }
}
