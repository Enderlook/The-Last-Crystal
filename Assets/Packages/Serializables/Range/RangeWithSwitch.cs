using Serializables;

using System;

using UnityEngine;

namespace Range
{
    public abstract class RangeSwitchable<T, U> : Switch<T, U>
    {
        private static readonly GUIContent item1GUIContent = new GUIContent("Range", "A random value from this range will be used.");

        private static readonly GUIContent item2GUIContent = new GUIContent("Value", "This value will be used.");

        private static readonly GUIContent useAlternativeGUIContent = new GUIContent("Use Range", "If checked, a random value from a range will be used.");

        protected override GUIContent Item1GUIContent => item1GUIContent;

        protected override GUIContent Item2GUIContent => item2GUIContent;

        protected override GUIContent UseAlternativeGUIContent => useAlternativeGUIContent;
    }

    [Serializable]
    public class RangeFloatSwitchable : RangeSwitchable<RangeFloat, float>, IBasicRange<float>, IBasicRangeInt<float>
    {
        public float Value => Alternative ? Value2 : Value1.Value;

        public int ValueInt => Alternative ? RangeFloat.FloatToIntByChance(Value2) : Value1.ValueInt;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random float.</param>
        public static explicit operator float(RangeFloatSwitchable source) => source.Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeFloatSwitchable source) => source.ValueInt;
    }

    [Serializable]
    public class RangeFloatStepSwitchable : RangeSwitchable<RangeFloatStep, float>, IBasicRange<float>, IBasicRangeInt<float>
    {
        public float Value => Alternative ? Value2 : Value1.Value;

        public int ValueInt => Alternative ? RangeFloat.FloatToIntByChance(Value2) : Value1.ValueInt;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random float.</param>
        public static explicit operator float(RangeFloatStepSwitchable source) => source.Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeFloatStepSwitchable source) => source.ValueInt;
    }

    [Serializable]
    public class RangeIntSwitchable : RangeSwitchable<RangeInt, int>, IBasicRangeInt<int>
    {
        public int Value => Alternative ? Value2 : Value1.Value;

        public int ValueInt => Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeIntSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeIntSwitchable source) => source.Value;
    }

    [Serializable]
    public class RangeIntStepSwitchable : RangeSwitchable<RangeIntStep, int>, IBasicRangeInt<int>
    {
        public int Value => Alternative ? Value2 : Value1.Value;

        public int ValueInt => Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeIntStepSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeIntStepSwitchable source) => source.Value;
    }
}