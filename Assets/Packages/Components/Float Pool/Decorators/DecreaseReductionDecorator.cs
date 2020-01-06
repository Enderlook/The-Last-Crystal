using FloatPool.Internal;

using Serializables;

using System;

using UnityEngine;

namespace FloatPool.Decorators
{
    [Serializable]
    public class DecreaseReductionDecorator : Decorator
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Reduction formula done in Decrease method.\n{0} is amount to reduce.\n{1} is current value.\n{2} is max value.")]
        private Calculator reductionFormula;
#pragma warning restore CS0649

        public override void Initialize()
        {
            if (string.IsNullOrEmpty(reductionFormula.formula))
                reductionFormula.formula = "{0}";
            base.Initialize();
        }

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => base.Decrease(reductionFormula.Calculate(amount, Current, Max), allowUnderflow);
    }
}