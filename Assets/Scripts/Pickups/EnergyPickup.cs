using Creatures;

using UnityEngine;

namespace Pickups
{
    public class EnergyPickup : Pickup
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Amount of energy restored.")]
        private float energyAmount;
#pragma warning restore CS0649

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            CreatureWithEnergy creature = collision.gameObject.GetComponent<CreatureWithEnergy>();
            if (creature != null)
            {
                creature.AddEnergy(energyAmount, true, true);
                PlaySoundAndDestroy();
            }
        }
    }
}