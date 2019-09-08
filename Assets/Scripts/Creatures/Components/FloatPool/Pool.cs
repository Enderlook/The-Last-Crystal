using System;
using FloatPool.Decorators;
using FloatPool.Internal;
using UnityEngine;

namespace FloatPool
{
    [Serializable]
    public class Pool : MonoBehaviour, IFloatPool
    {
        public FloatPool basePool;

        [SerializeField, HideInInspector]
        private bool hasCallback;
        [HasConfirmationField(nameof(hasCallback))]
        public CallbackDecorator callback;

        [SerializeField, HideInInspector]
        private bool hasBar;
        [HasConfirmationField(nameof(hasBar))]
        public BarDecorator bar;

        [SerializeField, HideInInspector]
        private bool hasRecharger;
        [HasConfirmationField(nameof(hasRecharger))]
        public RechargerDecorator recharger;

        [SerializeField, HideInInspector]
        private bool hasChangeCallback;
        [HasConfirmationField(nameof(hasChangeCallback))]
        public ChangeCallbackDecorator changeCallback;

        [SerializeField, HideInInspector]
        private bool hasDecreaseReduction;
        [HasConfirmationField(nameof(hasDecreaseReduction))]
        public DecreaseReductionDecorator decreaseReduction;

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

        private Tuple<Decorator, bool>[] decorators;
        private Tuple<Decorator, bool>[] Decorators {
            get {
                if (decorators == null)
                {
                    decorators = new Tuple<Decorator, bool>[]
                        {
                            new Tuple<Decorator, bool>(callback, hasCallback),
                            new Tuple<Decorator, bool>(bar, hasBar),
                            new Tuple<Decorator, bool>(recharger, hasRecharger),
                            new Tuple<Decorator, bool>(changeCallback, hasChangeCallback),
                            new Tuple<Decorator, bool>(decreaseReduction, hasDecreaseReduction),
                        };
                }
                return decorators;
            }
        }

        public void ConstructDecorators()
        {
            pool = basePool;
            foreach (Tuple<Decorator, bool> decorator in Decorators)
            {
                if (decorator.Item2)
                {
                    decorator.Item1.SetDecorable(pool);
                    pool = decorator.Item1;
                }
            }
        }

        public U GetLayer<U>() where U : IFloatPool
        {
            foreach (Tuple<Decorator, bool> layer in Decorators)
            {
                if (layer.Item1.GetType() == typeof(U) && layer.Item2)
                    return (U)(IFloatPool)layer.Item1;
            }
            return default;
        }

        public void Initialize() => FloatPool.Initialize();
        public void InternalUpdate(float deltatime) => FloatPool.InternalUpdate(deltatime);
        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => FloatPool.Decrease(amount, allowUnderflow);
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => FloatPool.Increase(amount, allowOverflow);
    }
}