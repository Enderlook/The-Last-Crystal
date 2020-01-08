using UnityEngine;

using Random = UnityEngine.Random;

namespace ScriptableSound
{
    [CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableSound")]
    public class SoundList : Sound
    {

        [Header("Setup")]
        [SerializeField, Tooltip("Sounds to play.")]
        private Sound[] sounds;

        [Header("Configuration")]
        [SerializeField, Tooltip("Play mode order.")]
        private PlayModeOrder playMode;

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
                    sounds[index].Play();
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