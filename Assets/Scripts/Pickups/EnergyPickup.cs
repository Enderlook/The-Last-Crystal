using Additions.Attributes;
using Additions.Serializables.Atoms;

using Creatures;

using UnityEngine;

namespace Pickups
{
    public class EnergyPickup : Pickup
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Amount of energy restored."), Expandable, RestrictType(typeof(AtomGet<float>))]
        private Atom energyAmount;
#pragma warning restore CS0649

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            CreatureWithEnergy creature = collision.gameObject.GetComponent<CreatureWithEnergy>();
            if (creature != null)
            {
                creature.AddEnergy(energyAmount.GetValue<float>(), true, true);
                PlaySoundAndDestroy();
            }
        }
    }
}