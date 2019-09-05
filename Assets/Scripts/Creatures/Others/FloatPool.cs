using UnityEngine;
using UnityEngine.Events;

namespace FloatPool
{
    public interface IFloatPool
    {
        /// <summary>
        /// Maximum amount. <see cref="Current"/> can't be greater than this value.<br/>
        /// </summary>
        /// <seealso cref="Current"/>
        float Max { get; }

        /// <summary>
        /// Current amount. It can't be greater than <see cref="MaxCurrent"/><br/>
        /// </summary>
        /// <seealso cref="Max"/>
        float Current { get; }

        /// <summary>
        /// Ration between <see cref="Current"/> and <see cref="Max"/>.
        /// </summary>
        float Ratio { get; }

        void Initialize();
        /// <summary>
        /// Update values.
        /// </summary>
        /// <param name="deltatime">Time in seconds since last update (<see cref="Time.deltaTime"/>).</param>
        void Update(float deltatime);

        /// <summary>
        /// Reduce <see cref="Current"/> by <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">Amount to reduce <see cref="Current"/>.</param>
        /// <param name="allowUnderflow">Whenever <see cref="Current"/> could reach negative values or not.</param>
        /// <returns><c>remaining</c>: Amount clamped below 0. <c>taken</c>: difference between <paramref name="amount"/> and <c>remaining</c>.</returns>
        (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false);

        /// <summary>
        /// Increase <see cref="Current"/> by <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">Amount to increase <see cref="Current"/>.</param>
        /// <param name="allowUnderflow">Whenever <see cref="Current"/> could be higher than <see cref="Max"/> or not.</param>
        /// <returns><c>remaining</c>: Amount clamped above <see cref="Max"/>. <c>taken</c>: difference between <paramref name="amount"/> and <c>remaining</c>.</returns>
        (float remaining, float taken) Increase(float amount, bool allowOverflow = false);
    }

    [System.Serializable]
    public class FloatPool : IFloatPool
    {
        [Header("Main Configuration")]
        [Tooltip("Maximum Current.")]
        public float startingMax = 100;
        public float Max { get; private set; }


        [Tooltip("Starting Current. Set -1 to use Max value.")]
        public float startingCurrent = -1;

        public float Current { get; private set; }

        public float Ratio => Current / Max;

        /// <summary>
        /// Initializes the value of <see cref="Current"/> and <see cref="Max"/> with <see cref="startingCurrent"/> and <seealso cref="startingMax"/>.
        /// If <see cref="startingCurrent"/> is -1, <see cref="startingMax"/> will be used instead to set <see cref="Current"/>..
        /// </summary>
        public void Initialize()
        {
            Current = startingCurrent == -1 ? startingMax : startingCurrent;
            Max = startingMax;
        }

        public void Update(float deltaTime) { }

        /// <summary>
        /// Changes the value of <see cref="Current"/> by <paramref name="amount"/>, and clamp values to 0 and <see cref="Max"/> if <paramref name="allowUnderflow"/> and <paramref name="allowOverflow"/> are <see langword="false"/>, respectively.
        /// </summary>
        /// <param name="amount">Amount to change <see cref="Current"/></param>
        /// <param name="allowOverflow">Whenever <paramref name="amount"/> can increase <see cref="Current"/> over <see cref="Max"/> or not.</param>
        /// <param name="allowUnderflow">Whenever <paramref name="amount"/> can decrease <see cref="Current"/> below 0 or not.</param>
        /// <returns>Amount above <see cref="Max"/> or below 0 if they are allowed. If there isn't overflow nor underflow (or they weren't allowed) it will return 0.</br>
        /// Be warned that underflow below 0 is returned as negative numbers.</returns>
        private float ChangeValue(float amount, bool allowOverflow = false, bool allowUnderflow = false)
        {
            Current += amount;
            float flow = 0;
            if (Current > Max && !allowOverflow)
            {
                flow = Max - Current;
                Current = Max;
            }
            else if (Current < 0 && !allowUnderflow)
            {
                flow = Current;
                Current = 0;
            }
            return flow;
        }

        /// <summary>
        /// Reduce <see cref="Current"/> by <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">Amount to reduce <see cref="Current"/>.</param>
        /// <param name="allowUnderflow">Whenever <see cref="Current"/> could reach negative values or not.</param>
        /// <returns><c>remaining</c>: Amount clamped below 0. <c>taken</c>: difference between <paramref name="amount"/> and <c>remaining</c>.</returns>
        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            if (amount < 0)
                Debug.LogWarning($"The amount was negative. {nameof(Current)} is increasing.");
            float remaining = -ChangeValue(-amount, allowUnderflow: allowUnderflow);
            return (remaining, amount - remaining);
        }

        /// <summary>
        /// Increase <see cref="Current"/> by <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">Amount to increase <see cref="Current"/>.</param>
        /// <param name="allowUnderflow">Whenever <see cref="Current"/> could be higher than <see cref="Max"/> or not.</param>
        /// <returns><c>remaining</c>: Amount clamped above <see cref="Max"/>. <c>taken</c>: difference between <paramref name="amount"/> and <c>remaining</c>.</returns>
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            if (amount < 0)
                Debug.LogWarning($"The amount was negative. {nameof(Current)} is decreasing.");
            float remaining = ChangeValue(amount, allowOverflow: allowOverflow);
            return (remaining, amount - remaining);
        }
    }

    public abstract class Decorator : IFloatPool
    {
        protected IFloatPool decorable;

        /// <summary>
        /// Set decorable. Mandatory.
        /// </summary>
        /// <param name="decorable">Decorable.</param>
        public void SetDecorable(IFloatPool decorable) => this.decorable = decorable;

        public virtual float Max => decorable.Max;
        public virtual float Current => decorable.Current;
        public virtual float Ratio => decorable.Ratio;

        public virtual (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => decorable.Decrease(amount, allowUnderflow);
        public virtual (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => decorable.Increase(amount, allowOverflow);
        public virtual void Initialize() => decorable.Initialize();
        public virtual void Update(float deltaTime) => decorable.Update(deltaTime);
    }

    [System.Serializable]
    public class CallbackDecorator : Decorator
    {
        [Header("Callback Configuration")]
        [Tooltip("Event called when Current become 0 or bellow due to Decrease method call.")]
        public UnityEvent emptyCallback;
        [Tooltip("Event called when Current reaches Max due to Increase method call.")]
        public UnityEvent fullCallback;

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            (float remaining, float taken) result = base.Decrease(amount, allowUnderflow);
            if (Current == 0)
                emptyCallback.Invoke();
            return result;
        }

        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            (float remaining, float taken) result = base.Decrease(amount, allowOverflow);
            if (Current == Max)
                fullCallback.Invoke();
            return result;
        }
    }

    [System.Serializable]
    public class BarDecorator : Decorator
    {
        [Header("Bar Configuration")]
        [Tooltip("Bar used to show values.")]
        public HealthBar bar;

        private void UpdateValues()
        {
            if (bar != null)
                bar.UpdateValues(Current);
        }

        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            (float remaining, float taken) result = base.Increase(amount, allowOverflow);
            UpdateValues();
            return result;
        }

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            (float remaining, float taken) result = base.Increase(amount, allowUnderflow);
            UpdateValues();
            return result;
        }

        public override void Initialize()
        {
            base.Initialize();
            if (bar != null)
                bar.ManualUpdate(Current, Max);
        }
    }

    [System.Serializable]
    public class UnityEventBoolean : UnityEvent<bool> { }

    [System.Serializable]
    public class RechargerDecorator : Decorator
    {
        [Header("Recharger Configuration")]
        [Tooltip("Value per second increases in Current.")]
        public float rechargeRate;

        [Tooltip("Amount of time in seconds after call Decrease method in order to start recharging.")]
        public float rechargingDelay;
        private float _currentRechargingDelay = 0f;

        [Tooltip("Sound played while recharging.")]
        public Playlist playlist;
        [Tooltip("Audio Source used to play sound.")]
        public AudioSource audioSource;

        [Tooltip("Event executed when start recharging.")]
        public UnityEvent startCallback;
        private bool _startCalled = false;
        [Tooltip("Event executed when end recharging.\nIf ended before Current reached Max it will be true. Otherwise false.")]
        public UnityEventBoolean endCallback;
        [Tooltip("Event executed when can recharge.\nIf it is recharging it will be true")]
        public UnityEventBoolean activeCallback;

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

        public override void Update(float deltaTime)
        {
            Recharge(deltaTime);
            base.Update(deltaTime);
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
            if (audioSource != null && playlist != null && !audioSource.isPlaying)
                playlist.Play(audioSource, Settings.IsSoundActive);
        }
    }
}