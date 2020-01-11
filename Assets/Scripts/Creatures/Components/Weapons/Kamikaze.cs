using ScriptableSound;

using UnityEngine;

namespace CreaturesAddons.Weapons
{
    public class Kamikaze : PassiveMelee
    {
        [SerializeField, Tooltip("Sound played on hit.")]
        private Sound hitSound;

        public override void ProduceDamage(object victim)
        {
            base.ProduceDamage(victim);
            SimpleSoundPlayer.CreateOneTimePlayer(hitSound, true, true);
            Destroy(thisTransform.gameObject);
        }
    }
}