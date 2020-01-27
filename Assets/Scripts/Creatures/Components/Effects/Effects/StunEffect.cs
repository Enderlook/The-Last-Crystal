using UnityEngine;

namespace Creatures.Effects.Effects
{
    public class StunEffect : Effect<Creature>, IStartEffect, IEndEffect
    {
        private Color colorEffect;

        public StunEffect(float duration, Color colorEffect) : base(duration) => this.colorEffect = colorEffect;

        public override bool ReplaceCurrentInstance => true;

        public void OnStart()
        {
            host.AddColorTint(colorEffect);
            host.isStunned = true;
        }

        public void OnEnd(bool wasAborted)
        {
            host.RemoveColorTint(colorEffect);
            host.isStunned = false;
        }
    }
}