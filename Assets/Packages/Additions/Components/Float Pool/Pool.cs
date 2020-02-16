using Additions.Attributes;
using Additions.Components.FloatPool.Decorators;
using Additions.Components.FloatPool.Internal;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Additions.Components.FloatPool
{
    [Serializable]
    public class Pool : MonoBehaviour, IFloatPool
    {
        public FloatPool basePool;

#pragma warning disable CS0649, IDE0051
        [SerializeField, HideInInspector]
        private bool hasEmptyCallback;

        [SerializeField, ShowIf(nameof(hasEmptyCallback))]
        private EmptyCallbackDecorator emptyCallback;

        [SerializeField, HideInInspector]
        private bool hasFullCallback;

        [SerializeField, ShowIf(nameof(hasFullCallback))]
        private FullCallbackDecorator fullCallback;

        [SerializeField, HideInInspector]
        private bool hasChangeCallback;

        [SerializeField, ShowIf(nameof(hasChangeCallback))]
        private ChangeCallbackDecorator changeCallback;

        [SerializeField, HideInInspector]
        private bool hasBar;

        [SerializeField, ShowIf(nameof(hasBar))]
        private BarDecorator bar;

        [SerializeField, HideInInspector]
        private bool hasRecharger;

        [SerializeField, ShowIf(nameof(hasRecharger))]
        private RechargerDecorator recharger;

        [SerializeField, HideInInspector]
        private bool hasDecreaseReduction;

        [SerializeField, ShowIf(nameof(hasDecreaseReduction))]
        private DecreaseReductionDecorator decreaseReduction;
#pragma warning restore IDE0051, CS0649

        private IFloatPool pool;
        public IFloatPool FloatPool {
            get {
                if (pool == null)
                    ConstructDecorators();
                return pool;
            }
        }

        public float Max => FloatPool.Max;

        public float Current => FloatPool.Current;

        public float Ratio => FloatPool.Ratio;

        private IEnumerable<Decorator> GetAppliedDecorators()
        {
            if (hasEmptyCallback)
                yield return emptyCallback;
            if (hasFullCallback)
                yield return fullCallback;
            if (hasChangeCallback)
                yield return changeCallback;
            if (hasBar)
                yield return bar;
            if (hasRecharger)
                yield return recharger;
            if (hasDecreaseReduction)
                yield return decreaseReduction;
        }

        public void ConstructDecorators()
        {
            pool = basePool;
            foreach (Decorator decorator in GetAppliedDecorators())
            {
                ((IDecorator)decorator).SetDecorable(pool);
                pool = decorator;
            }
        }

        public U GetLayer<U>() where U : IFloatPool
        {
            foreach (Decorator decorator in GetAppliedDecorators())
                if (decorator.GetType() == typeof(U))
                    return (U)(IFloatPool)decorator;
            return default;
        }

        public void Initialize() => FloatPool.Initialize();

        public void UpdateBehaviour(float deltatime) => FloatPool.UpdateBehaviour(deltatime);
        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => FloatPool.Decrease(amount, allowUnderflow);
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => FloatPool.Increase(amount, allowOverflow);
    }
}