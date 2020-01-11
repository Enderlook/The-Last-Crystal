using AdditionalAttributes;

using System;

using UnityEngine;

namespace ScriptableSound
{
    [RequireComponent(typeof(AudioSource))]
    public class SimpleSoundPlayer : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound to play."), Expandable]
        private Sound sound;

        [SerializeField, Tooltip("If start playing on awake.")]
        private bool playOnAwake;

        [SerializeField, Tooltip("Whenever it should destroy the gameObjet when sound(s) ends.")]
        private bool destroyOnFinish;
#pragma warning restore CS0649

        private bool hasPlay;

        private AudioSource audioSource;

        /// <summary>
        /// Whenever <see cref="sound"/> is playing or not.
        /// </summary>
        public bool IsPlaying => sound.IsPlaying;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sound = sound.CreatePrototype();
            if (playOnAwake)
                Play();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Update()
        {
            sound.UpdateBehaviour(Time.deltaTime);
            if (destroyOnFinish && hasPlay && !sound.IsPlaying)
                Destroy(gameObject);
        }

        /// <summary>
        /// Play <see cref="sound"/>.
        /// </summary>
        /// <param name="endCallback"><see cref="Action"/> called after <see cref="sound"/> ends.</param>
        private void Play(Action endCallback = null)
        {
            Action callback = endCallback;
            if (destroyOnFinish)
                callback += () => Destroy(this);

            sound.SetConfiguration(new SoundConfiguration(audioSource, endCallback));
            sound.Play();
            hasPlay = true;
        }

        /// <summary>
        /// Stop <see cref="sound"/> from playing.
        /// </summary>
        public void Stop() => sound.Stop();

        /// <summary>
        /// Create a new <see cref="GameObject"/> with this component on it.
        /// </summary>
        /// <param name="sound"><see cref="Sound"/> included in the component.</param>
        /// <param name="playOnAwake">Whenever it should start playing on awake.</param>
        /// <param name="destroyOnFinish">Whenever it should be destroyed after end playing.</param>
        /// <returns></returns>
        public static SimpleSoundPlayer CreateOneTimePlayer(Sound sound, bool playOnAwake = false, bool destroyOnFinish = false)
        {
            GameObject gameObject = new GameObject("One Time Simple Sound Player");
            SimpleSoundPlayer simpleSoundPlayer = gameObject.AddComponent<SimpleSoundPlayer>();
            simpleSoundPlayer.playOnAwake = playOnAwake;
            simpleSoundPlayer.destroyOnFinish = destroyOnFinish;
            simpleSoundPlayer.sound = sound.CreatePrototype();

            return simpleSoundPlayer;
        }
    }
}