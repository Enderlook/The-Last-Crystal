﻿using AdditionalAttributes;

using UnityEngine;

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

        public override void Update()
        {
            if (IsPlaying)
            {
                CurrentSound.Update();
                if (!CurrentSound.IsPlaying)
                {
                    if (playsAmount == -1 || playsAmount > 0)
                    {
                        ChoseNextSound();
                        CurrentSound.SetConfiguration(soundConfiguration);
                        CurrentSound.Play();
                    }
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
                    ReducePlayAmountIf(PlayListMode.IndividualSounds);
                    index = Random.Range(0, sounds.Length);
                    break;
                case PlayModeOrder.Sequence:
                    if (++index == sounds.Length)
                    {
                        index = 0;
                        ReducePlayAmountIf(PlayListMode.FullList);
                    }
                    ReducePlayAmountIf(PlayListMode.IndividualSounds);
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
        }

        private void ReducePlayAmountIf(PlayListMode mode)
        {
            if (playsAmount != -1 && playListMode == mode)
                playsAmount--;
        }

        public override void Stop()
        {
            CurrentSound.Stop();
            base.Stop();
        }

        public override void Play()
        {
            index = -1;
            ChoseNextSound();
            CurrentSound.SetConfiguration(soundConfiguration);
            CurrentSound.Play();
            base.Play();
        }
    }
}