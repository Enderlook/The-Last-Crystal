using System;
using FloatPool;
using FloatPool.Decorators;
using Pools.Layers.BarCallbackDecreaseReduction;

namespace Pools
{
    public class BarCallbackDecreaseReduction : Raw.BarCallbackDecreaseReduction { }

    namespace Raw
    {
        public class BarCallbackDecreaseReduction : Pool<ManagerLayer>
        {
            public override float Max => decorable.Max;
            public override float Current => decorable.Current;
            public override float Ratio => decorable.Ratio;

            public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => decorable.Decrease(amount, allowUnderflow);
            public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => decorable.Increase(amount, allowOverflow);
            public override void Initialize() => decorable.Initialize();
            public override void InternalUpdate(float deltatime) => decorable.InternalUpdate(deltatime);
        }
    }

    namespace Layers.BarCallbackDecreaseReduction
    {
        [Serializable]
        public class BarCallbackDecreaseReductionLayer : DecreaseReductionDecorator<BarCallback.BarCallbackFloatLayer> { }
        [Serializable]
        public class ManagerLayer : DecoratorsManager<BarCallbackDecreaseReductionLayer> { }
    }
}

