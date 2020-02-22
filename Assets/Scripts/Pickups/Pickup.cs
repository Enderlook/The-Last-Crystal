using Additions.Attributes;
using Additions.Components.ScriptableSound;
using Additions.Serializables.Atoms;

using UnityEngine;

namespace Pickups
{
    public class Pickup : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound played when picked up.")]
        private Sound pickupSound;

        [SerializeField, Tooltip("Duration of the pickup in seconds before self destroy."), Expandable, RestrictType(typeof(AtomGet<float>))]
        private Atom lifespan;
#pragma warning restore CS0649

        private void Awake() => Destroy(gameObject, lifespan.GetValue<float>());

        protected void PlaySoundAndDestroy()
        {
            SimpleSoundPlayer.CreateOneTimePlayer(pickupSound, true, true);
            Destroy(gameObject);
        }
    }
}