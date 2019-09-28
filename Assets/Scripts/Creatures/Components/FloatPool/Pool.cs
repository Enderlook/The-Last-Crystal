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
        [SerializeField, HideInInspector]
        private bool hasEmptyCallback;
        [HasConfirmationField(nameof(hasEmptyCallback))]
        public EmptyCallbackDecorator emptyCallback;

        [SerializeField, HideInInspector]
        private bool hasFullCallback;
        [HasConfirmationField(nameof(hasFullCallback))]
        public FullCallbackDecorator fullCallback;

        [SerializeField, HideInInspector]
        private bool hasChangeCallback;
        [HasConfirmationField(nameof(hasChangeCallback))]
        public ChangeCallbackDecorator changeCallback;

        [SerializeField, HideInInspector]
        private bool hasBar;
        [HasConfirmationField(nameof(hasBar))]
        public BarDecorator bar;

        [SerializeField, HideInInspector]
        private bool hasRecharger;
        [HasConfirmationField(nameof(hasRecharger))]
        public RechargerDecorator recharger;

        [SerializeField, HideInInspector]
        private bool hasDecreaseReduction;
        [HasConfirmationField(nameof(hasDecreaseReduction))]
        public DecreaseReductionDecorator decreaseReduction;
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
                decorator.SetDecorable(pool);
                pool = decorator;
            }
        }

        public U GetLayer<U>() where U : IFloatPool
        {
            foreach (Decorator decorator in GetAppliedDecorators())
            {
                Debug.Log(decorator.GetType());
                if (decorator.GetType() == typeof(U))
                    return (U)(IFloatPool)decorator;
            }
            return default;
        }

        public void Initialize() => FloatPool.Initialize();
        public void InternalUpdate(float deltatime) => FloatPool.InternalUpdate(deltatime);
        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => FloatPool.Decrease(amount, allowUnderflow);
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => FloatPool.Increase(amount, allowOverflow);
    }
}