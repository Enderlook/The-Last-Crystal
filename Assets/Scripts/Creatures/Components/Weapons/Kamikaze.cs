using Additions.Components.ScriptableSound;

using Creatures.Effects;

using UnityEngine;

namespace Creatures.Weapons
{
    public class Kamikaze : PassiveMelee
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound played on hit.")]
        private Sound hitSound;
#pragma warning restore CS0649

        private Creature creature;

        public override void Initialize(Creature creature)
        {
            base.Initialize(creature);
            this.creature = creature;
        }

        public override void ProduceDamage(IHasHealth takeDamage, ITakePush takePush, ITakeEffect<Creature> takeEffect)
        {
            base.ProduceDamage(takeDamage, takePush, takeEffect);
            SimpleSoundPlayer.CreateOneTimePlayer(hitSound, true, true);
            creature.Die(true);
        }
    }
}