using System;
using System.Collections.Generic;
using FloatPool.Decorators;
using FloatPool.Internal;
using UnityEngine;
using System.Reflection;

namespace FloatPool
{
    [Serializable]
    public class Pool : MonoBehaviour, IFloatPool
    {
        public FloatPool basePool;

#pragma warning disable CS0649
#pragma warning disable IDE0051
        [SerializeField, HideInInspector]
        private bool hasEmptyCallback;
        [SerializeField, HasConfirmationField(nameof(hasEmptyCallback))]
        private EmptyCallbackDecorator emptyCallback;

        [SerializeField, HideInInspector]
        private bool hasFullCallback;
        [SerializeField, HasConfirmationField(nameof(hasFullCallback))]
        private FullCallbackDecorator fullCallback;

        [SerializeField, HideInInspector]
        private bool hasChangeCallback;
        [SerializeField, HasConfirmationField(nameof(hasChangeCallback))]
        private ChangeCallbackDecorator changeCallback;

        [SerializeField, HideInInspector]
        private bool hasBar;
        [SerializeField, HasConfirmationField(nameof(hasBar))]
        private BarDecorator bar;

        [SerializeField, HideInInspector]
        private bool hasRecharger;
        [SerializeField, HasConfirmationField(nameof(hasRecharger))]
        private RechargerDecorator recharger;

        [SerializeField, HideInInspector]
        private bool hasDecreaseReduction;
        [SerializeField, HasConfirmationField(nameof(hasDecreaseReduction))]
        private DecreaseReductionDecorator decreaseReduction;
#pragma warning restore IDE0051
#pragma warning restore CS0649

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
            foreach (FieldInfo field in HasConfirmationFieldAttribute.GetConfirmedFields(this))
            {
                yield return (Decorator)field.GetValue(this);
            }
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
            {
                if (decorator.GetType() == typeof(U))
                    return (U)(IFloatPool)decorator;
            }
            return default;
        }

        public void Initialize() => FloatPool.Initialize();
        public void UpdateBehaviour(float deltatime) => FloatPool.UpdateBehaviour(deltatime);
        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => FloatPool.Decrease(amount, allowUnderflow);
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => FloatPool.Increase(amount, allowOverflow);
    }
}