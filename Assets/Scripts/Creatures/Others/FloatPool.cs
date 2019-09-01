using UnityEngine;
using UnityEngine.Events;

namespace FloatPool
{
    public abstract class AbstractFloatPool
    {
        /// <summary>
        /// Maximum amount. <see cref="Current"/> can't be greater than this value.<br/>
        /// </summary>
        /// <seealso cref="Current"/>
        public virtual float Max { get; internal set; }
        /// <summary>
        /// Current amount. It can't be greater than <see cref="MaxCurrent"/><br/>
        /// </summary>
        /// <seealso cref="Max"/>
        public virtual float Current { get; internal set; }
        /// <summary>
        /// Ration between <see cref="Current"/> and <see cref="Max"/>.
        /// </summary>
        public virtual float Ratio {
            get => Current / Max;
            internal set => Current = Max / value;
        }

        public abstract void Initialize();
        /// <summary>
        /// Update values.
        /// </summary>
        /// <param name="deltatime">Time in seconds since last update (<see cref="Time.deltaTime"/>).</param>
        public abstract void Update(float deltatime);
        /// <summary>
        /// Reduce <see cref="Current"/> by <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">Amount to reduce <see cref="Current"/>.</param>
        /// <param name="allowUnderflow">Whenever <see cref="Current"/> could reach negative values or not.</param>
        /// <returns><c>remaining</c>: Amount clamped below 0. <c>taken</c>: difference between <paramref name="amount"/> and <c>remaining</c>.</returns>
        public abstract (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false);
        /// <summary>
        /// Increase <see cref="Current"/> by <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">Amount to increase <see cref="Current"/>.</param>
        /// <param name="allowUnderflow">Whenever <see cref="Current"/> could be higher than <see cref="Max"/> or not.</param>
        /// <returns><c>remaining</c>: Amount clamped above <see cref="Max"/>. <c>taken</c>: difference between <paramref name="amount"/> and <c>remaining</c>.</returns>
        public abstract (float remaining, float taken) Increase(float amount, bool allowOverflow = false);
    }

    [System.Serializable]
    public class FloatPool : AbstractFloatPool
    {
        [Header("Main Configuration")]
        [Tooltip("Maximum Current.")]
        public float startingMax = 100;

        [Tooltip("Starting Current. Set -1 to use Max value.")]
        public float startingCurrent = -1;

        /// <summary>
        /// Initializes the value of <see cref="Current"/> and <see cref="Max"/> with <see cref="startingCurrent"/> and <seealso cref="startingMax"/>.
        /// If <see cref="startingCurrent"/> is -1, <see cref="startingMax"/> will be used instead to set <see cref="Current"/>..
        /// </summary>
        public override void Initialize()
        {
            float current = startingCurrent == -1 ? startingMax : startingCurrent;
            Current = current;
            Max = startingMax;
        }

        public override void Update(float deltaTime) { }

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
        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
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
        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            if (amount < 0)
                Debug.LogWarning($"The amount was negative. {nameof(Current)} is decreasing.");
            float remaining = ChangeValue(amount, allowOverflow: allowOverflow);
            return (remaining, amount - remaining);
        }
    }

    public abstract class Decorator<T> : AbstractFloatPool where T : AbstractFloatPool
    {
        public T decorable;

        public override float Max { get => decorable.Max; internal set => decorable.Max = value; }
        public override float Current { get => decorable.Current; internal set => decorable.Current = value; }
        public override float Ratio { get => decorable.Ratio; internal set => decorable.Ratio = value; }

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => decorable.Decrease(amount, allowUnderflow);
        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => decorable.Increase(amount, allowOverflow);
        public override void Initialize() => decorable.Initialize();
        public override void Update(float deltaTime) => decorable.Update(deltaTime);
    }

    [System.Serializable]
    public class CallbackDecorator<T> : Decorator<T> where T : AbstractFloatPool
    {
        [Header("Callback Configuration")]
        [Tooltip("Event called when Current become 0 or bellow.")]
        public UnityEvent emptyCallback;
        [Tooltip("Event called when Current reaches Max.")]
        public UnityEvent fullCallback;

        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            (float remaining, float taken) result = base.Increase(amount, allowOverflow);
            if (Current == Max)
                fullCallback.Invoke();
            return result;
        }

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            (float remaining, float taken) result = base.Decrease(amount, allowUnderflow);
            if (Current == 0)
                emptyCallback.Invoke();
            return result;
        }
    }

    [System.Serializable]
    public class BarDecorator<T> : Decorator<T> where T : AbstractFloatPool
    {
        [Header("Bar Configuration")]
        [Tooltip("Bar used to show values.")]
        public HealthBar bar;

        private void UpdateValues() => bar?.UpdateValues(Current, Max);

        public override void Initialize()
        {
            base.Initialize();
            bar?.ManualUpdate(Current, Max);
        }

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            (float remaining, float taken) result = base.Decrease(amount, allowUnderflow);
            UpdateValues();
            return result;
        }

        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            (float remaining, float taken) result = base.Increase(amount, allowOverflow);
            UpdateValues();
            return result;
        }
    }

    [System.Serializable]
    public class UnityEventBoolean : UnityEvent<bool> { }

    [System.Serializable]
    public class RechargingDecorator<T> : Decorator<T> where T : AbstractFloatPool
    {
        [Header("Recharging Configuration")]
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
        /// /// <param name="isForced">Whenever it was forced or it reached <see cref="Current"/> to <see cref="Max"/>.</param>
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

    [System.Serializable]
    public class ChangeCallbackDecorator<T> : Decorator<T> where T : AbstractFloatPool
    {
        [Tooltip("Event executed each time Max or Current values changes.")]
        public UnityEvent callback;

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            (float remaining, float taken) result = base.Decrease(amount, allowUnderflow);
            callback.Invoke();
            return result;
        }

        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            (float remaining, float taken) result = base.Increase(amount, allowOverflow);
            callback.Invoke();
            return result;
        }
    }

    [System.Serializable]
    public class DecreaseReductionDecorator<T> : Decorator<T> where T : AbstractFloatPool
    {
        [Tooltip("Reduction percent done in Decrease method.")]
        [Range(0, 1)]
        public float reductionPercent;

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => base.Decrease(amount * (1 - reductionPercent), allowUnderflow);
    }
}