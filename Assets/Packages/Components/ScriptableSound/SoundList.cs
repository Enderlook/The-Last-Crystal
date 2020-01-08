using AdditionalAttributes;
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

        [SerializeField, Tooltip("Play mode order.")]
        private PlayModeOrder playMode;
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

        public override void Update()
        {
            if (IsPlaying)
            {
                Sound sound = sounds[index];
                sound.Update();
                if (!sound.IsPlaying)
                {
                    ChoseNextSound();
                    sound = sounds[index];
                    sound.SetConfiguration(soundConfiguration);
                    sound.Play();
                }
            }
        }

        private void ChoseNextSound()
        {
            switch (playMode)
            {
                case PlayModeOrder.Random:
                    if (++amountPlay == sounds.Length)
                        ReduceRemainingLoopsByOne();
                    index = Random.Range(0, sounds.Length);
                    break;
                case PlayModeOrder.Sequence:
                    if (++index == sounds.Length)
                    {
                        index = 0;
                        ReduceRemainingLoopsByOne();
                    }
                    break;
                case PlayModeOrder.PingPong:
                    if (isReverse)
                    {
                        if (--index < 0)
                        {
                            index = sounds.Length - 1;
                            isReverse = false;
                            ReduceRemainingLoopsByOne();
                        }
                    }
                    else
                    {
                        if (++index == sounds.Length)
                        {
                            index = 0;
                            isReverse = true;
                            ReduceRemainingLoopsByOne();
                        }
                    }
                    break;
            }
        }
    }
}