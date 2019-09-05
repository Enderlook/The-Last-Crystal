using System.Collections.Generic;
using System.Reflection;
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
        float Current { get; }
        /// <summary>
        /// Current amount. It can't be greater than <see cref="MaxCurrent"/><br/>
        /// </summary>
        /// <seealso cref="Max"/>
        float Max { get; }
        /// <summary>
        /// Ration between <see cref="Current"/> and <see cref="Max"/>.
        /// </summary>
        float Ratio { get; }

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
        /// <summary>
        /// Update values.
        /// </summary>
        /// <param name="deltatime">Time in seconds since last update (<see cref="Time.deltaTime"/>).</param>
        void Initialize();
        void Update(float deltatime);
    }

    public abstract class AbstractFloatPool : IFloatPool
    {
        public virtual float Max { get; internal set; }
        public virtual float Current { get; internal set; }
        public virtual float Ratio {
            get => Current / Max;
            internal set => Current = Max / value;
        }

        public abstract void Initialize();
        public abstract void Update(float deltatime);
        public abstract (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false);
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

    public abstract class Decorator<T> : AbstractFloatPool where T : IFloatPool
    {
        public T decorable;

        public override float Max => decorable.Max;
        public override float Current => decorable.Current;
        public override float Ratio => decorable.Ratio;

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => decorable.Decrease(amount, allowUnderflow);
        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => decorable.Increase(amount, allowOverflow);
        public override void Initialize() => decorable.Initialize();
        public override void Update(float deltaTime) => decorable.Update(deltaTime);
    }

    public class DecoratorAccessor<T, V> where T : V where V : class
    {
        public T decorable;
        private List<V> layers;

        public U GetLayer<U>() where U : V
        {
            if (layers == null)
                GetLayers();
            foreach (V layer in layers)
            {
                if (typeof(U) == layer.GetType())
                {
                    return (U)System.Convert.ChangeType(layer, typeof(U));
                }
            }
            return default;
        }

        private void GetLayers()
        {
            layers = new List<V>
            {
                decorable
            };

            void Layer(V layer)
            {
                FieldInfo field = layer.GetType().GetField(nameof(decorable));
                if (field != null && field.GetValue(layer) is V newLayer)
                {
                    layers.Add(newLayer);
                    Layer(newLayer);
                }
            }
            Layer(decorable);
        }
    }

    [System.Serializable]
    public class DecoratorsManager<T> : DecoratorAccessor<T, IFloatPool>, IFloatPool where T : IFloatPool
    {
        public float Current => decorable.Current;
        public float Max => decorable.Max;
        public float Ratio => decorable.Ratio;

        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => decorable.Decrease(amount, allowUnderflow);
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => decorable.Increase(amount, allowOverflow);
        public void Initialize() => decorable.Initialize();
        public void Update(float deltatime) => decorable.Update(deltatime);
    }

    [System.Serializable]
    public class CallbackDecorator<T> : Decorator<T> where T : IFloatPool
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
    public class BarDecorator<T> : Decorator<T> where T : IFloatPool
    {
        [Header("Bar Configuration")]
        [Tooltip("Bar used to show values.")]
        public HealthBar bar;

        private void UpdateValues()
        {
            if (bar != null)
                bar.UpdateValues(Current, Max);
        }

        public override void Initialize()
        {
            base.Initialize();
            if (bar != null)
                bar.ManualUpdate(Current, Max);
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
    public class RechargingDecorator<T> : Decorator<T> where T : IFloatPool
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
    public class ChangeCallbackDecorator<T> : Decorator<T> where T : IFloatPool
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
    public class ChangeCallbackDecorator : ChangeCallbackDecorator<IFloatPool> { }

    [System.Serializable]
    public class DecreaseReductionDecorator<T> : Decorator<T> where T : IFloatPool
    {
        [Tooltip("Reduction formula done in Decrease method.\n{0} is amount to reduce.\n{1} is current value.\n{2} is max value.")]
        public Calculator reductionFormula;

        public override void Initialize()
        {
            if (string.IsNullOrEmpty(reductionFormula.formula))
                reductionFormula.formula = "{0}";
            base.Initialize();
        }

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => base.Decrease(reductionFormula.Calculate(amount, Current, Max), allowUnderflow);
    }
}