using CreaturesAddons;
using UnityEngine;

namespace EnemyAddons
{
    public class AIWeaponShooter : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Weapons configuration")]
        private AIWeapons[] weapons;
#pragma warning restore CS0649
        
        void Update()
        {
            foreach(AIWeapons weapon in weapons)
            {
                if (weapon.ShouldAttack)
                    weapon.weapon.TryExecute();
            }
        }
    }

    [System.Serializable]
    public class AIWeapons
    {
        [Tooltip("Weapon.")]
        public Weapon weapon;
        [Tooltip("Slash.")]
        public Slash slash;

        public bool ShouldAttack => slash.rayCasting.Raycast(1 << slash.layerToHit);
    }
}


