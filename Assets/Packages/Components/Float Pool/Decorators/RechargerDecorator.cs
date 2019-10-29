using System;
using FloatPool.Internal;
using Master;
using SoundSystem;
using UnityEngine;
using UnityEngine.Events;

namespace FloatPool.Decorators
{
    [Serializable]
    public class RechargerDecorator : Decorator
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Value per second increases in Current.")]
        private float rechargeRate;

        [SerializeField, Tooltip("Amount of time in seconds after call Decrease method in order to start recharging.")]
        private float rechargingDelay;
        private float _currentRechargingDelay = 0f;

        [SerializeField, Tooltip("Sound played while recharging.")]
        private Playlist playlist;
        [SerializeField, Tooltip("Audio Source used to play sound.")]
        private AudioSource audioSource;

        [SerializeField, Tooltip("Event executed when start recharging.")]
        private UnityEvent startCallback;
        private bool _startCalled = false;
        [SerializeField, Tooltip("Event executed when end recharging.\nIf ended before Current reached Max it will be true. Otherwise false.")]
        private UnityEventBoolean endCallback;
        [SerializeField, Tooltip("Event executed when can recharge.\nIf it is recharging it will be true")]
        private UnityEventBoolean activeCallback;
#pragma warning restore CS0649

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            ResetRechargingDelay(true);
            return base.Decrease(amount, allowUnderflow);
        }

        /// <summary>
        /// Reset <see cref="_currentRechargingDelay"/> to 0 and calls <see cref="endCallback"/>.
        /// </summary>
        /// <param name="isForced">Whenever it was forced or it reached <see cref="Current"/> to <see cref="Max"/>.</param>
        private void ResetRechargingDelay(bool isForced)
        {
            _currentRechargingDelay = 0;
            CallEndCallback(isForced);
        }

        public override void UpdateBehaviour(float deltaTime)
        {
            Recharge(deltaTime);
            base.UpdateBehaviour(deltaTime);
        }

        /// <summary>
        /// Check whenever <see cref="_currentRechargingDelay"/> is 0 and <see cref="Current"/> should be recharge.<br/>
        /// Also execute additional functionalities when appropriate.
        /// </summary>
        /// <param name="deltaTime"></param>
        private void Recharge(float deltaTime)
        {
            if (_currentRechargingDelay >= rechargingDelay)
            {
                if (Current < Max)
                {
                    CallStartCallback();
                    Increase(rechargeRate * deltaTime);
                    PlayRechargingSound();
                    activeCallback.Invoke(true);
                }
                else
                {
                    activeCallback.Invoke(false);
                    CallEndCallback(false);
                }
            }
            else
            {
                _currentRechargingDelay += deltaTime;
            }
        }

        /// <summary>
        /// Calls <see cref="startCallback"/> only if <see cref="_startCalled"/> is <see langword="false"/>.<br/>
        /// Also sets <see cref="_startCalled"/> to <see langword="true"/>.
        /// </summary>
        private void CallStartCallback()
        {
            if (!_startCalled)
            {
                _startCalled = true;
                startCallback.Invoke();
            }
        }

        /// <summary>
        /// Calls <see cref="endCallback"/> only if <see cref="_endCalled"/> is <see langword="true"/>.<br/>
        /// Also sets <see cref="_startCalled"/> to <see langword="false"/>.
        /// </summary>
        /// <param name="isForced">Whenever it was forced or it reached <see cref="Current"/> to <see cref="Max"/>.</param>
        private void CallEndCallback(bool isForced)
        {
            if (_startCalled)
            {
                _startCalled = false;
                endCallback.Invoke(isForced);
            }
        }

        private void PlayRechargingSound()
        {
            if (audioSource != null && playlist != null && !audioSource.isPlaying)
                playlist.Play(audioSource, Settings.IsSoundActive);
        }

        [Serializable]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It's only used by Unity so it doesn't matter if it isn't visible.")]
        public class UnityEventBoolean : UnityEvent<bool> { }
    }
}