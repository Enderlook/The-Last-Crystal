using Additions.Components.FloatPool.Internal;

using System;

using UnityEngine;
using UnityEngine.Events;

namespace Additions.Components.FloatPool.Decorators
{
    [Serializable]
    public class ChangeCallbackDecorator : Decorator
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Event executed each time Current value changes due to Decrease or Increase methods.")]
        private UnityEvent callback;
#pragma warning restore CS0649

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
}