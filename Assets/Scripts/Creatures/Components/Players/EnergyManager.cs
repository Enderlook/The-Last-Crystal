using FloatPool;
using FloatPool.Decorators;
using HealthBarGUI;
using UnityEngine;

namespace CreaturesAddons
{
    public class EnergyManager : MonoBehaviour, IInit, IUpdate, IFloatPool
    {
        [SerializeField, Tooltip("Energy pool.")]
        private Pool energy;

        public void Init(Creature creature) => energy.Initialize();

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