using Additions.Attributes;

using System;

using UnityEngine;

namespace Additions.Components.ScriptableSound
{
    [RequireComponent(typeof(AudioSource))]
    public class SimpleSoundPlayer : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound to play."), Expandable]
        private Sound sound;

        [SerializeField, Tooltip("If start playing on start.")]
        private bool playOnStart;

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
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            sound = sound.CreatePrototype();
            if (playOnStart)
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
        public void Play() => Play(null); // We require this method for Unity events buttons.

        /// <summary>
        /// Play <see cref="sound"/>.
        /// </summary>
        /// <param name="endCallback"><see cref="Action"/> called after <see cref="sound"/> ends.</param>
        public void Play(Action endCallback)
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
        public static void CreateOneTimePlayer(Sound sound, bool playOnAwake = false, bool destroyOnFinish = false)
        {
            GameObject gameObject = new GameObject("One Time Simple Sound Player");
            SimpleSoundPlayer simpleSoundPlayer = gameObject.AddComponent<SimpleSoundPlayer>();
            simpleSoundPlayer.playOnStart = playOnAwake;
            simpleSoundPlayer.destroyOnFinish = destroyOnFinish;
            simpleSoundPlayer.sound = sound.CreatePrototype();
        }

        /// <summary>
        /// Create a new <see cref="GameObject"/> with this component on it.
        /// </summary>
        /// <param name="soundPlay"><see cref="SoundPlay"/> where <see cref="Sound"/> will be taken.</param>
        /// <param name="playOnAwake">Whenever it should start playing on awake.</param>
        /// <param name="destroyOnFinish">Whenever it should be destroyed after end playing.</param>
        public static void CreateOneTimePlayer(SoundPlay soundPlay, bool playOnAwake = false, bool destroyOnFinish = false) => CreateOneTimePlayer(soundPlay.Sound, playOnAwake, destroyOnFinish);
    }
}