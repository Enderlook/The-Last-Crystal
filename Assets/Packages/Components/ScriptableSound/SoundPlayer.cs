using AdditionalAttributes;

using System;
using System.Linq;

using UnityEngine;

namespace ScriptableSound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        private AudioSource audioSource;
#pragma warning disable CS0649
        [SerializeField, Tooltip("List of sounds to play."), Expandable]
        private Sound[] sounds;

        [SerializeField, Tooltip("If start playing on awake.")]
        private bool playOnAwake;

        [SerializeField, Tooltip("Which playlist play on awake."), ShowIf(nameof(playOnAwake), indented = true)]
        private int onAwakeIndex;
#pragma warning restore CS0649

        private int index;

        /// <summary>
        /// Whenever the last sound used is playing or not.
        /// </summary>
        public bool IsPlaying => sounds[index].IsPlaying;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sounds = sounds.Select(e => e.CreatePrototype()).ToArray();
            if (playOnAwake)
                Play(onAwakeIndex);
        }

        /// <summary>
        /// Play <see cref="sounds"/> at <see cref="index"/>.
        /// </summary>
        /// <param name="endCallback"><see cref="Action"/> called after <see cref="sound"/> ends.</param>
        public void Play(int index, Action endCallback = null)
        {
            Sound sound = sounds[index];
            sound.SetConfiguration(new SoundConfiguration(audioSource, endCallback));
            sound.Play();
            this.index = index;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Update() => sounds[index].UpdateBehaviour(Time.deltaTime);

        /// <summary>
        /// Stop <see cref="sounds"/> at <see cref="index"/> from playing.
        /// </summary>
        public void Stop() => sounds[index].Stop();
    }
}