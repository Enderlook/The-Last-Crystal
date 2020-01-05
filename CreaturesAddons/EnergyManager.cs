using FloatPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreaturesAddons
{
    public class EnergyManager : MonoBehaviour, IFloatPool
    {
        [SerializeField, Tooltip("Energy Pool")]
        private Pool energy;

        public float Max => ((IFloatPool)energy).Max;

        public float Current => ((IFloatPool)energy).Current;

        public float Ratio => ((IFloatPool)energy).Ratio;

        public (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => ((IFloatPool)energy).Decrease(amount, allowUnderflow);
        public (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => ((IFloatPool)energy).Increase(amount, allowOverflow);
        public void Initialize() => ((IFloatPool)energy).Initialize();
        public void UpdateBehaviour(float deltaTime) => ((IFloatPool)energy).UpdateBehaviour(deltaTime);
    }
}