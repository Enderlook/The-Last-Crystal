using FloatPool.Internal;

using System;

using UnityEngine;
using UnityEngine.Events;

namespace FloatPool.Decorators
{
    [Serializable]
    public class EmptyCallbackDecorator : Decorator
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Event called when Current become 0 or bellow due to Decrease method call.")]
        private UnityEvent callback;
#pragma warning restore CS0649

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            (float remaining, float taken) result = base.Decrease(amount, allowUnderflow);
            if (Current == 0)
                callback.Invoke();
            return result;
        }
    }
}