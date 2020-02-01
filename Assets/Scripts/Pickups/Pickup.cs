using Additions.Components.ScriptableSound;

using UnityEngine;

namespace Pickups
{
    public class Pickup : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound played when picked up.")]
        private Sound pickupSound;
#pragma warning restore CS0649

        [SerializeField, Tooltip("Duration of the pickup in seconds before self destroy.")]
        private float lifespan = 5;

        private void Awake() => Destroy(gameObject, lifespan);

        protected void PlaySoundAndDestroy()
        {
            SimpleSoundPlayer.CreateOneTimePlayer(pickupSound, true, true);
            Destroy(gameObject);
        }
    }
}