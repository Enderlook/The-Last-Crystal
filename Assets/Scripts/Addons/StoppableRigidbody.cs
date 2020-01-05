using CreaturesAddons;

using UnityEngine;

namespace AdditionalComponents
{
    public class StoppableRigidbody : MonoBehaviour, IInit
    {
        private Rigidbody2D thisRigidbody2D;

        void IInit.Init(Creature creature) => thisRigidbody2D = creature.thisRigidbody2D;

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
                    thisRigidbody2D.isKinematic = false;
                    thisRigidbody2D.velocity = velocity;
                    thisRigidbody2D.angularVelocity = angularVelocity;
                }
                else
                {
                    thisRigidbody2D.mass *= speedMultiplier;
                    thisRigidbody2D.velocity /= speedMultiplier;
                    thisRigidbody2D.angularVelocity /= speedMultiplier;
                }
                speedMultiplier = value;
                if (speedMultiplier == 0)
                {
                    velocity = thisRigidbody2D.velocity;
                    angularVelocity = thisRigidbody2D.angularVelocity;
                    thisRigidbody2D.velocity = Vector2.zero;
                    thisRigidbody2D.angularVelocity = 0;
                    thisRigidbody2D.isKinematic = true;
                }
                else
                {
                    thisRigidbody2D.mass /= speedMultiplier;
                    thisRigidbody2D.velocity *= speedMultiplier;
                    thisRigidbody2D.angularVelocity *= speedMultiplier;
                }
            }
        }
    }
}