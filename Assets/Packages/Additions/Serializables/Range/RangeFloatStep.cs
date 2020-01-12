using System;

using UnityEngine;

namespace Additions.Serializables.Ranges
{
    [Serializable]
    public class RangeFloatStep : RangeFloat, IRangeStep<float>, IRangeStepInt<float>
    {
        [SerializeField, Tooltip("Step values used when producing random numbers.")]
        // Used in RangeStepDrawer as string name. Don't forget to change string if this is renamed.
#pragma warning disable CS0649
        private float step;
#pragma warning restore CS0649

        public float Step => step;

        public override float Value => (float)Math.Round(base.Value / step, MidpointRounding.AwayFromZero) * step;

        public override int ValueInt => base.ValueInt / (int)step * (int)step;

        public float ValueWithoutStep => base.Value;

        public int ValueIntWithoutStep => base.ValueInt;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random float.</param>
        public static explicit operator float(RangeFloatStep source) => source.Value;

        /// <summary>
        /// Return a random integer value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeFloatStep source) => source.ValueInt;
    }
}