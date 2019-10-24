using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsageRequireDataType(typeof(Attribute), checkingFlags = CheckingFlags.CheckSubclassTypes)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageRequireDataTypeAttribute : Attribute
    {
        [Flags]
        public enum CheckingFlags
        {
            /// <summary>
            /// Nothing.
            /// </summary>
            None = 0,

            /// <summary>
            /// If used, <see cref="Types"/> will be forbidden types (blacklist).<br>
            /// If not used, they will be the only allowed types (white list).<br>
            /// </summary>
            IsBlackList = 1 << 1,

            /// <summary>
            /// If used, it will also check for array o list versions of types.<br>
            /// Useful because Unity <see cref="PropertyDrawer"/> are draw on each element of an array or list <see cref="SerializedProperty"/>.<br>
            /// </summary>
            IncludeEnumerableTypes = 1 << 2,

            /// <summary>
            /// Whenever it should check if the type is a subclass of one of the listed types.
            /// </summary>
            CheckSubclassTypes = 1 << 3,

            /// <summary>
            /// Whenever it should check if the type is  superclass of one of the listed types.
            /// </summary>
            CheckSuperclassTypes = 1 << 4,

            /// <summary>
            /// Whenever it should check for assignable from type to one of the listed types.
            /// </summary>
            CheckIsAssignableTypes = 1 << 5,

            /// <summary>
            /// <see cref="CheckSubclassTypes"/> or <see cref="CheckIsAssignableTypes"/>.
            /// </summary>
            CheckSubclassOrAssignable = CheckSubclassTypes | CheckIsAssignableTypes,

            /// <summary>
            /// Whenever it should check if type can be assigned to one of the listed types.
            /// </summary>
            CheckCanBeAssignedTypes = 1 << 6,

            /// <summary>
            /// <see cref="CheckIsAssignableTypes"/> or <see cref="CheckCanBeAssignedTypes"/>.
            /// </summary>

            CheckSuperClassOrCanBeAssigned = CheckIsAssignableTypes | CheckCanBeAssignedTypes,
        };

        private readonly Type[] basicTypes;

        /// <summary>
        /// On <see langword="true"/>, to it will also check for the Array and List version of <see cref="basicTypes"/> types.
        /// </summary>
        public CheckingFlags checkingFlags = CheckingFlags.IncludeEnumerableTypes;

        /// <summary>
        /// Each time Unity compile script, they will be analyzed to check if the attribute is being used in proper DataTypes.
        /// </summary>
        /// <param name="types">Data types allowed. Use <see cref="CheckingFlags.IsBlackList"/> in <see cref="checkingFlags"/> to become it forbidden data types.</param>
        public AttributeUsageRequireDataTypeAttribute(params Type[] types) => basicTypes = types;

#if UNITY_EDITOR
        /// <summary>
        /// Data types to check.<br>
        /// Only available in Unity Editor.
        /// </summary>
        public HashSet<Type> Types {
            get {
                if (types == null)
                {
                    // Check
                    if ((checkingFlags & CheckingFlags.IncludeEnumerableTypes) != 0)
                    {
                        int length = basicTypes.Length;
                        types = new HashSet<Type>();
                        for (int i = 0; i < length; i++)
                        {
                            Type type = basicTypes[i];
                            Types.Add(type);
                            Types.Add(typeof(List<>).MakeGenericType(type));
                            Types.Add(type.MakeArrayType());
                        }

                    }
                    else
                        types = new HashSet<Type>(basicTypes);
                }
                return types;
            }
        }
        private HashSet<Type> types;

        private string errorMessage;
        private string ErrorMessage => errorMessage ?? (errorMessage = $"{(checkingFlags.HasFlag(CheckingFlags.IsBlackList) ? "doesn't accept" : "only accept")} types of {string.Join(", ", Types.Select(e => e.Name))}");

        public void CheckAllowance(Type toCheckType, string toCheckName, string attributeName)
        {
            bool contains = Types.Contains(toCheckType);

            if (!contains)
            {
                void Check(Func<Type, Type, bool> test)
                {
                    foreach (Type type in Types)
                    {
                        bool result = test(toCheckType, type);
                        if (result)
                        {
                            contains = result;
                            break;
                        }
                    }
                }

                // Check if checkingFlags has the following flags
                // We could use checkingFlags.HasaFlag(flag), but it's ~10 times slower
                if ((checkingFlags & CheckingFlags.CheckSubclassTypes) != 0)
                    Check((f, t) => f.IsSubclassOf(t));
                if ((checkingFlags & CheckingFlags.CheckSuperclassTypes) != 0 && !contains)
                    Check((f, t) => f.IsSubclassOf(t));
                if ((checkingFlags & CheckingFlags.CheckSuperclassTypes) != 0 && !contains)
                    Check((f, t) => t.IsSubclassOf(f));
                if ((checkingFlags & CheckingFlags.CheckSuperclassTypes) != 0 && !contains)
                    Check((f, t) => f.IsAssignableFrom(t));
                if ((checkingFlags & CheckingFlags.CheckCanBeAssignedTypes) != 0 && !contains)
                    Check((f, t) => t.IsAssignableFrom(f));
            }

            if (contains == ((checkingFlags & CheckingFlags.IsBlackList) != 0))
                Debug.LogException(new ArgumentException($"{attributeName} {ErrorMessage}. {toCheckName} is {toCheckType.Name} type."));
        }
#endif
    }
}