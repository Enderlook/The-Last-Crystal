using Additions.Components.FloatPool;
using Additions.Components.FloatPool.Decorators;
using Additions.Prefabs.HealthBarGUI;
using Additions.Utils;

using UnityEngine;

namespace Creatures
{
    public class EnergyManager : MonoBehaviour, IInitialize<Creature>, IFloatPool
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Energy pool.")]
        private Pool energy;
#pragma warning restore CS0649

        public void Initialize(Creature creature) => energy.Initialize();

        public void SetEnergyBar(HealthBar energyBar) => energy.GetLayer<BarDecorator>().Bar = energyBar;

        public float Max => ((IFloatPool)energy).Max;

        public float Current => ((IFloatPool)energy).Current;

        public float Ratio => ((IFloatPool)energy).Ratio;

        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => ((IFloatPool)energy).Decrease(amount, allowUnderflow);
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => ((IFloatPool)energy).Increase(amount, allowOverflow);
        public void Initialize() => ((IFloatPool)energy).Initialize();
        public void UpdateBehaviour(float deltaTime) => ((IFloatPool)energy).UpdateBehaviour(deltaTime);
    }
}