using System;
using System.Collections.Generic;

namespace Additions.Attributes.AttributeUsage
{
    [AttributeUsageRequireDataType(typeof(Attribute), typeFlags = TypeCasting.CheckSubclassTypes)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageRequireDataTypeAttribute : Attribute
    {
        private readonly Type[] basicTypes;

        /// <summary>
        /// Additional checking rules.
        /// </summary>
        public TypeCasting typeFlags;

        /// <summary>
        /// If <see langword="true"/>, <see cref="Types"/> will be forbidden types (blacklist).<br>
        /// If <see langword="false"/>, they will be the only allowed types (white list).<br>
        /// </summary>
        public bool isBlackList;

        /// <summary>
        /// If <see langword="true"/>, it will also check for array o list versions of types.<br>
        /// Useful because Unity <see cref="PropertyDrawer"/> are draw on each element of an array or list <see cref="SerializedProperty"/>.
        /// </summary>
        public bool includeEnumerableTypes;

        /// <summary>
        /// Each time Unity compile script, they will be analyzed to check if the attribute is being used in proper DataTypes.
        /// </summary>
        /// <param name="types">Data types allowed. Use <see cref="CheckingFlags.IsBlackList"/> in <see cref="typeFlags"/> to become it forbidden data types.</param>
        public AttributeUsageRequireDataTypeAttribute(params Type[] types) => basicTypes = types;

#if UNITY_EDITOR
        private HashSet<Type> Types => types ?? (types = AttributeUsageHelper.GetHashsetTypes(basicTypes, includeEnumerableTypes));
        private HashSet<Type> types;

        private string allowedTypes;
        private string AllowedTypes => allowedTypes ?? (allowedTypes = AttributeUsageHelper.GetTextTypes(Types, typeFlags, isBlackList));

        /// <summary>
        /// Unity Editor only.
        /// </summary>
        /// <param name="toCheckType"></param>
        /// <param name="toCheckName"></param>
        /// <param name="attributeName"></param>
        /// <remarks>Only use in Unity Editor.</remarks>
        public void CheckAllowance(Type toCheckType, string toCheckName, string attributeName) => AttributeUsageHelper.CheckContains(nameof(AttributeUsageRequireDataTypeAttribute), Types, typeFlags, isBlackList, AllowedTypes, toCheckType, attributeName, toCheckName);
    }
#endif
}