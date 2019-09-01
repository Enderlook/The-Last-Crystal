using UnityEngine;

namespace FloatPool
{
    public interface IFloatPool
    {
        /// <summary>
        /// Maximum amount. <see cref="Current"/> can't be greater than this value.<br/>
        /// </summary>
        /// <seealso cref="Current"/>
        float Max { get; set; }

        /// <summary>
        /// Current amount. It can't be greater than <see cref="MaxCurrent"/><br/>
        /// </summary>
        /// <seealso cref="Max"/>
        float Current { get; set; }

        /// <summary>
        /// Ration between <see cref="Current"/> and <see cref="Max"/>.
        /// </summary>
        float Ratio { get; set; }

        void Initialize();

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
        public float Max { get; set; }


        [Tooltip("Starting Current. Set -1 to use Max value.")]
        public float startingCurrent = -1;

        public float Current { get; set; }

        public float Ratio {
            get => Current / Max;
            set => Current = Max / value;
        }

        /// <summary>
        /// Initializes the value of <see cref="Current"/> and <see cref="Max"/> with <see cref="startingCurrent"/> and <seealso cref="startingMax"/>.
        /// If <see cref="startingCurrent"/> is -1, <see cref="startingMax"/> will be used instead to set <see cref="Current"/>..
        /// </summary>
        public void Initialize()
        {
            float current = startingCurrent == -1 ? startingMax : startingCurrent;
            Current = current;
            Max = startingMax;
        }

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

        public virtual float Max { get => decorable.Max; set => decorable.Max = value; }
        public virtual float Current { get => decorable.Current; set => decorable.Current = value; }
        public virtual float Ratio { get => decorable.Ratio; set => decorable.Ratio = value; }

        public virtual (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => decorable.Decrease(amount, allowUnderflow);
        public virtual (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => decorable.Increase(amount, allowOverflow);
        public virtual void Initialize() => decorable.Initialize();
    }

    [System.Serializable]
    public class CallbackDecorator : Decorator
    {
        private System.Action emptyCallback;
        private System.Action fullCallback;

        /// <summary>
        /// Configure the object.
        /// </summary>
        /// <param name="emptyCallback">Method called when <see cref="Max"/> or <see cref="Current"/> become 0.</param>
        /// <param name="fullCallback">Method called when <see cref="Current"/> becomes <see cref="Max"/>.</param>
        public void SetConfiguration(System.Action emptyCallback = null, System.Action fullCallback = null)
        {
            this.emptyCallback = emptyCallback;
            this.fullCallback = fullCallback;
        }

        private void CheckEmpty(float value)
        {
            if (value == 0)
                emptyCallback?.Invoke();
        }

        public override float Max {
            get => base.Max;
            set {
                base.Max = value;
                CheckEmpty(value);
            }
        }
        public override float Current {
            get => base.Current;
            set {
                base.Current = value;
                CheckEmpty(value);
                if (value == Max)
                    fullCallback?.Invoke();
            }
        }
    }
}