using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StoppableRigidbody : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D {
        get {
            if (thisRigidbody2D == null)
                thisRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            return thisRigidbody2D;
        }
    }
    private Rigidbody2D thisRigidbody2D;

    private float angularVelocity;
    private Vector2 velocity;
    private float speedMultiplier = 1;
    /// <summary>
    /// Change this value affect the speed of the current and future <see cref="Rigidbody2D"/>.<br/>
    /// If 0, the <seealso cref="Rigidbody2D"/> will be stopped and turn into <c><see cref="Rigidbody2D.isKinematic"/> = <see langword="true"/></c>.
    /// </summary>
    public float SpeedMultiplier {
        get => speedMultiplier;
        set {
            if (speedMultiplier == value) return;
            if (speedMultiplier == 0)
            {
                Rigidbody2D.isKinematic = false;
                Rigidbody2D.velocity = velocity;
                Rigidbody2D.angularVelocity = angularVelocity;
            }
            else
            {
                Rigidbody2D.mass *= speedMultiplier;
                Rigidbody2D.velocity /= speedMultiplier;
                Rigidbody2D.angularVelocity /= speedMultiplier;
            }
            speedMultiplier = value;
            if (speedMultiplier == 0)
            {
                velocity = Rigidbody2D.velocity;
                angularVelocity = Rigidbody2D.angularVelocity;
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.angularVelocity = 0;
                Rigidbody2D.isKinematic = true;
            }
            else
            {
                Rigidbody2D.mass /= speedMultiplier;
                Rigidbody2D.velocity *= speedMultiplier;
                Rigidbody2D.angularVelocity *= speedMultiplier;
            }
        }
    }
}
