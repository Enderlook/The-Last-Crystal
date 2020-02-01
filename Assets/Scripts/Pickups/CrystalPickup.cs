using Creatures;
using Master;
using UnityEngine;

namespace Pickups
{
    public class CrystalPickup : Pickup
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Amount of health restored to the crystal.")]
        private float healingAmount;
#pragma warning restore CS0649


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Global.TransformCreature.Crystal.GetTranform().GetComponent<IHasHealth>().TakeHealing(healingAmount, true, true);
            PlaySoundAndDestroy();
        }
    }
}
