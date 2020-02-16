using Additions.Attributes;
using Additions.Components.FloatPool.Internal;
using Additions.Components.SoundSystem;
using Additions.Serializables.Atoms;
using Master;

using System;

using UnityEngine;
using UnityEngine.Events;

namespace Additions.Components.FloatPool.Decorators
{
    [Serializable]
    public class RechargerDecorator : Decorator
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Value per second increases in Current."), Expandable, RestrictType(typeof(AtomGet<float>))]
        private Atom rechargeRate;

        private float RechargeRate;

        [SerializeField, Tooltip("Amount of time in seconds after call Decrease method in order to start recharging."), Expandable, RestrictType(typeof(AtomGet<float>))]
        private Atom rechargingDelay;

        private float RechargingDelay;

        private float _currentRechargingDelay;

        [SerializeField, Tooltip("Sound played while recharging.")]
        private Playlist playlist;

        [SerializeField, Tooltip("Audio Source used to play sound.")]
        private AudioSource audioSource;

        [SerializeField, Tooltip("Event executed when start recharging.")]
        private UnityEvent startCallback;

        private bool _startCalled;

        [SerializeField, Tooltip("Event executed when end recharging.\nIf ended before Current reached Max it will be true. Otherwise false.")]
        private UnityEventBoolean endCallback;

        [SerializeField, Tooltip("Event executed when can recharge.\nIf it is recharging it will be true")]
        private UnityEventBoolean activeCallback;
#pragma warning restore CS0649

        public override void Initialize()
        {
            RechargeRate = rechargeRate.GetValue<float>();
            RechargingDelay = rechargingDelay.GetValue<float>();
            base.Initialize();
        }

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
            if (_currentRechargingDelay >= RechargingDelay)
                if (Current < Max)
                {
                    CallStartCallback();
                    Increase(RechargeRate * deltaTime);
                    PlayRechargingSound();
                    activeCallback.Invoke(true);
                }
                else
                {
                    activeCallback.Invoke(false);
                    CallEndCallback(false);
                }
            else
                _currentRechargingDelay += deltaTime;
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
            if (audioSource != null)
                if (playlist != null)
                    if (!audioSource.isPlaying && Settings.IsSoundActive)
                        playlist.Play(audioSource);
                    else
                        Debug.LogWarning($"{nameof(RechargerDecorator)} doesn't have an {nameof(playlist)}.");
                else
                    Debug.LogWarning($"{nameof(RechargerDecorator)} doesn't have an {nameof(audioSource)}.");
        }

        [Serializable]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It's only used by Unity so it doesn't matter if it isn't visible.")]
        public class UnityEventBoolean : UnityEvent<bool> { }
    }
}