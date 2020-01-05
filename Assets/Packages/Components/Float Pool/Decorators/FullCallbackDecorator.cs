using FloatPool.Internal;

using System;

using UnityEngine;
using UnityEngine.Events;

namespace FloatPool.Decorators
{
    [Serializable]
    public class FullCallbackDecorator : Decorator
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Event called when Current reaches Max due to Increase method call.")]
        private UnityEvent callback;
#pragma warning restore CS0649

        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            (float remaining, float taken) result = base.Increase(amount, allowOverflow);
            if (Current == Max)
                callback.Invoke();
            return result;
        }
    }
}