using System;
using System.Collections.Generic;
using AdditionalAttributes.Internal;

namespace AdditionalAttributes
{
    [AttributeUsageRequireDataType(typeof(Attribute), checkingFlags = AttributeUsageHelper.CheckingFlags.CheckSubclassTypes)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageRequireDataTypeAttribute : Attribute
    {
        private readonly Type[] basicTypes;

        /// <summary>
        /// Additional checking rules.
        /// </summary>
        public AttributeUsageHelper.CheckingFlags checkingFlags = AttributeUsageHelper.CheckingFlags.IncludeEnumerableTypes;

        /// <summary>
        /// Each time Unity compile script, they will be analyzed to check if the attribute is being used in proper DataTypes.
        /// </summary>
        /// <param name="types">Data types allowed. Use <see cref="CheckingFlags.IsBlackList"/> in <see cref="checkingFlags"/> to become it forbidden data types.</param>
        public AttributeUsageRequireDataTypeAttribute(params Type[] types) => basicTypes = types;

#if UNITY_EDITOR
        private HashSet<Type> Types => types ?? (types = AttributeUsageHelper.GetHashsetTypes(basicTypes, checkingFlags));
        private HashSet<Type> types;

        private string allowedTypes;
        private string AllowedTypes => allowedTypes ?? (allowedTypes = AttributeUsageHelper.GetTextTypes(types, checkingFlags));

        public void CheckAllowance(Type toCheckType, string toCheckName, string attributeName)
        {
            AttributeUsageHelper.CheckContains(nameof(AttributeUsageRequireDataTypeAttribute), Types, checkingFlags, AllowedTypes, toCheckType, attributeName, toCheckName);
        }
    }
#endif
}