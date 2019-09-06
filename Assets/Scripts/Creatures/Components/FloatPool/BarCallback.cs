using System;
using FloatPool;
using FloatPool.Decorators;
using Pools.Layers.BarCallback;

namespace Pools
{
    public class BarCallback : Raw.BarCallbackPool { }

    namespace Raw
    {
        public class BarCallbackPool : Pool<ManagerLayer>
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

    namespace Layers.BarCallback
    {
        [Serializable]
        public class BarFloatLayer : BarDecorator<FloatPool.FloatPool> { }
        [Serializable]
        public class BarCallbackFloatLayer : CallbackDecorator<BarFloatLayer> { }
        [Serializable]
        public class ManagerLayer : DecoratorsManager<BarCallbackFloatLayer> { }
    }
}

