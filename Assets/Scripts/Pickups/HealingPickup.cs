using Additions.Attributes;
using Additions.Serializables.Atoms;

using Creatures;

using UnityEngine;

namespace Pickups
{
    public class HealingPickup : Pickup
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Amount of health restored."), Expandable, RestrictType(typeof(AtomGet<float>))]
        private Atom healingAmount;
#pragma warning restore CS0649

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IHasHealth hasHealth = collision.gameObject.GetComponent<IHasHealth>();
            if (hasHealth != null)
            {
                hasHealth.TakeHealing(healingAmount.GetValue<float>(), true, true);
                PlaySoundAndDestroy();
            }
        }
    }
}