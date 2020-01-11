using Additions.Components.ScriptableSound;

using UnityEngine;

namespace CreaturesAddons.Weapons
{
    public class Kamikaze : PassiveMelee
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound played on hit.")]
        private Sound hitSound;
#pragma warning restore CS0649

        public override void ProduceDamage(object victim)
        {
            base.ProduceDamage(victim);
            SimpleSoundPlayer.CreateOneTimePlayer(hitSound, true, true);
            Destroy(thisTransform.gameObject);
        }
    }
}