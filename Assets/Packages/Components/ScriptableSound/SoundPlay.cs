using Additions.Attributes;

using CreaturesAddons;

using System;

using UnityEngine;

namespace Additions.Components.ScriptableSound
{
    [Serializable]
    public class SoundPlay : IUpdate
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("AudioSource where sound is played.")]
        private AudioSource audioSource;

        [SerializeField, Tooltip("Sound to play."), Expandable]
        private Sound sound;

        public Sound Sound => sound;
#pragma warning restore CS0649

        /// <summary>
        /// Whenever <see cref="sound"/> is playing or not.
        /// </summary>
        public bool IsPlaying => sound.IsPlaying;

        /// <summary>
        /// Initializes this <see cref="SoundPlay"/>.<br/>
        /// If this method isn't called before using any other member of this instance it won't produce error but has an wrong behavior.
        /// </summary>
        public void Init() => sound = sound.CreatePrototype();

        /// <summary>
        /// Updates behavior.
        /// </summary>
        /// <param name="deltaTime">Time since last update in seconds. <seealso cref="Time.deltaTime"/></param>
        public void UpdateBehaviour(float deltaTime) => sound.UpdateBehaviour(deltaTime);

        /// <summary>
        /// Play <see cref="sound"/>.
        /// </summary>
        /// <param name="endCallback"><see cref="Action"/> called after <see cref="sound"/> ends.</param>
        public void Play(Action endCallback = null)
        {
            sound.SetConfiguration(new SoundConfiguration(audioSource, endCallback));
            sound.Play();
        }

        /// <summary>
        /// Stop <see cref="sound"/> from playing.
        /// </summary>
        public void Stop() => sound.Stop();
    }
}