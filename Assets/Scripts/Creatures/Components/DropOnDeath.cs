using Additions.Serializables;
using System;
using UnityEngine;

namespace Creatures
{
    public class DropOnDeath : MonoBehaviour, IDie
    {
        [Flags]
        public enum DeathCondition
        {
            Always = WhenMurdered | WhenSuicided,
            WhenSuicided = 1 << 2,
            WhenMurdered = 1 << 1
        }

        [SerializeField, Tooltip("Possible GameObjects to drop.")]
        private WeightedGameObjects dropsTable;

        [SerializeField, Tooltip("In which conditions it should drop")]
        private DeathCondition deathCondition = DeathCondition.Always;

        void IDie.Die(bool suicide)
        {
            if ((((deathCondition & DeathCondition.WhenSuicided) != 0 && suicide)
                || ((deathCondition & DeathCondition.WhenMurdered) != 0 && !suicide))
                && dropsTable.TryGetRandomElement(out GameObject result))
                Instantiate(result, transform.position, Quaternion.identity);
        }
    }
}