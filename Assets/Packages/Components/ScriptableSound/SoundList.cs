using AdditionalAttributes;

using System.Linq;

using UnityEngine;

using Utils;

using Random = UnityEngine.Random;

namespace ScriptableSound
{
    [CreateAssetMenu(fileName = "SoundList", menuName = "Sound/SoundList")]
    public class SoundList : Sound
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sounds to play."), Expandable]
        private Sound[] sounds;

        [SerializeField, Tooltip("The order in which sounds will be played.")]
        private PlayModeOrder playMode = PlayModeOrder.Sequence;

        [SerializeField, Tooltip("The mode of how sounds will be played.")]
        private PlayListMode playListMode;

        [SerializeField, Min(-1), Tooltip("If playListMode is FullList, it's the amount of times the full list will be played.\n" +
            "If playListMode is IndividualSounds, it's the amount of sounds that will be played.\n" +
            "In any case, if this field is 0, no sound will be played. If -1, it will be and endless loop.")]
        private int playsAmount = 1;
#pragma warning restore CS0649

        private int remainingPlays;

        /// <summary>
        /// Only used by <see cref="PlayModeOrder.PingPong"/>.
        /// </summary>
        private bool isReverse;

        /// <summary>
        /// Only used by <see cref="PlayModeOrder.Random"/>.
        /// </summary>
        private int amountPlay;

        private int index;

        private Sound CurrentSound => sounds[index];

        public override void UpdateBehaviour(float deltaTime)
        {
            if (IsPlaying)
            {
                CurrentSound.UpdateBehaviour(deltaTime);
                if (!CurrentSound.IsPlaying)
                {
                    if (playsAmount == -1 || remainingPlays > 0)
                        ConfigureNextSound();
                    else
                        IsPlaying = false;
                }
            }
        }

        private void ChoseNextSound()
        {
            switch (playMode)
            {
                case PlayModeOrder.Random:
                    if (++amountPlay == sounds.Length)
                        ReducePlayAmountIf(PlayListMode.FullList);
                    index = Random.Range(0, sounds.Length);
                    break;
                case PlayModeOrder.Sequence:
                    if (++index == sounds.Length)
                    {
                        index = 0;
                        ReducePlayAmountIf(PlayListMode.FullList);
                    }
                    break;
                case PlayModeOrder.PingPong:
                    if (isReverse)
                    {
                        if (--index < 0)
                        {
                            index = sounds.Length - 1;
                            isReverse = false;
                            ReducePlayAmountIf(PlayListMode.FullList);
                        }
                    }
                    else
                    {
                        if (++index == sounds.Length)
                        {
                            index = 0;
                            isReverse = true;
                            ReducePlayAmountIf(PlayListMode.FullList);
                        }
                    }
                    break;
            }
            ReducePlayAmountIf(PlayListMode.IndividualSounds);
        }

        private void ReducePlayAmountIf(PlayListMode mode)
        {
            if (playsAmount != -1 && playListMode == mode)
                remainingPlays--;
        }

        public override void Stop()
        {
            CurrentSound.Stop();
            base.Stop();
        }

        public override void Play()
        {
            if (IsPlaying)
                Stop();
            index = -1;
            remainingPlays = playsAmount;
            ConfigureNextSound();
            base.Play();
        }

        private void ConfigureNextSound()
        {
            ChoseNextSound();
            CurrentSound.SetConfiguration(soundConfiguration);
            CurrentSound.Play();
        }

        public override Sound CreatePrototype()
        {
            SoundList prototype = CreateInstance<SoundList>();
            prototype.name = PrototypeHelper.GetPrototypeNameOf(prototype);
            prototype.playMode = playMode;
            prototype.playListMode = playListMode;
            prototype.sounds = sounds.Select(e => e.CreatePrototype()).ToArray();
            return prototype;
        }
    }
}