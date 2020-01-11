using System;

using UnityEngine;

namespace Additions.Serializables.Ranges
{
    [Serializable]
    public class RangeIntStep : RangeInt, IRangeStep<int>, IRangeStepInt<int>
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Step values used when producing random numbers.")]
        // Used in RangeStepDrawer as string name. Don't forget to change string if this is renamed.
        private int step;
#pragma warning restore CS0649

        public int Step => step;

        public override int Value => base.Value / step * step;

        public int ValueWithoutStep => base.Value;

        int IRangeStepInt<int>.ValueIntWithoutStep => ValueWithoutStep;

        int IRangeStepInt<int>.ValueInt => Value;

        int IRangeStep<int>.ValueWithoutStep => ValueWithoutStep;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeIntStep"/> instance used to determine the random float.</param>
        public static explicit operator int(RangeIntStep source) => source.Value;
    }
}