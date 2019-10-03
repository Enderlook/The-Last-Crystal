using System;
using CreaturesAddons;
using UnityEngine;

namespace EnemyAddons
{
    public class AIWeaponShooter : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Weapons configuration.")]
        private AIWeapons[] weapons;
#pragma warning restore CS0649

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "It's used by Unity.")]
        private void Update()
        {
            foreach (AIWeapons weapon in weapons)
            {
                if (weapon.ShouldAttack)
                    weapon.weapon.TryExecute();
            }
        }
    }

    [Serializable]
    public class AIWeapons
    {
        [Tooltip("Weapon.")]
        public Weapon weapon;
        [Tooltip("Slash.")]
        public Slash slash;

        public bool ShouldAttack => slash.rayCasting.Raycast(1 << slash.layerToHit);
    }
}


