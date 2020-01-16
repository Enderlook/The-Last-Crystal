using Additions.Utils;

using Creatures.Weapons;

using System;

using UnityEngine;

namespace Creatures
{
    public class AutomatedAttack : MonoBehaviour, IInitialize<Creature>, IUpdate
    {
        private IAutomatedAttack[] weapons;

        public void Initialize(Creature creature) => weapons = gameObject.GetComponents<IAutomatedAttack>();

        public void UpdateBehaviour(float deltaTime) => Array.ForEach(weapons, e => e.AttackIfIsReadyAndIfTargetInRange());
    }
}


