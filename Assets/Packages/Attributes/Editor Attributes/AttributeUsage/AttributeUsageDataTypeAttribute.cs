using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageDataTypeAttribute : Attribute
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
            IsBlackList = 2,

            /// <summary>
            /// If used, it will also check for array o list versions of types.<br>
            /// Useful because Unity <see cref="PropertyDrawer"/> are draw on each element of an array or list <see cref="SerializedProperty"/>.<br>
            /// </summary>
            IncludeEnumerableTypes = 4,

            /// <summary>
            /// Whenever it should check if the type is a subclass of one of the listed types.
            /// </summary>
            CheckSubclassTypes = 8,

            /// <summary>
            /// Whenever it should check if the type is  superclass of one of the listed types.
            /// </summary>
            CheckSuperclassTypes = 16,

            /// <summary>
            /// Whenever it should check for assignable from type to one of the listed types.
            /// </summary>
            CheckIsAssignableTypes = 32,

            /// <summary>
            /// Whenever it should check if type can be assigned to one of the listed types.
            /// </summary>
            CheckCanBeAssignedTypes = 64,

            /// <summary>
            /// <see cref="CheckSubclassTypes"/> or <see cref="CheckIsAssignableTypes"/>.
            /// </summary>
            CheckSubclassOrAssignable = CheckSubclassTypes | CheckIsAssignableTypes,

            /// <summary>
            /// <see cref="CheckIsAssignableTypes"/> or <see cref="CheckCanBeAssignedTypes"/>.
            /// </summary>

            CheckSuperClassOrCanBeAssigned = CheckIsAssignableTypes | CheckCanBeAssignedTypes,
        };

        private readonly Type[] basicTypes;
        public CheckingFlags checkingFlags = CheckingFlags.IncludeEnumerableTypes;

        /// <summary>
        /// Each time Unity compile script, they will be analyzed to check if the attribute is being used in proper DataTypes.
        /// </summary>
        /// <param name="types">Data types allowed. Use <see cref="CheckingFlags.IsBlackList"/> in <see cref="checkingFlags"/> to become it forbidden data types.</param>
        public AttributeUsageDataTypeAttribute(params Type[] types) => basicTypes = types;

#if UNITY_EDITOR
        /// <summary>
        /// Data types to check.<br>
        /// Only available in Unity Editor.
        /// </summary>
        public Type[] Types {
            get {
                if (types == null)
                {
                    // Check
                    if (checkingFlags.HasFlag(CheckingFlags.IncludeEnumerableTypes))
                    {
                        int length = basicTypes.Length;
                        types = new Type[length * 3];
                        for (int i = 0; i < length; i++)
                        {
                            Type type = basicTypes[i];
                            types[i] = type;
                            types[i + length] = typeof(List<>).MakeGenericType(type);
                            types[i + length * 2] = type.MakeArrayType();
                        }

                    }
                    else
                        types = basicTypes;
                }
                return types;
            }
        }
        private Type[] types;

        private string errorMessage;
        private string ErrorMessage => errorMessage ?? (errorMessage = $"{(checkingFlags.HasFlag(CheckingFlags.IsBlackList) ? "doesn't accept" : "only accept")} types of {string.Join(", ", Types.Select(e => e.Name))}");

        public void CheckAllowance(Type attribute, FieldInfo fieldInfo, Type classType)
        {
            Type fieldType = fieldInfo.FieldType;
            bool contains = Types.Contains(fieldType);

            if (!contains)
            {
                void Check(Func<Type, Type, bool> test)
                {
                    foreach (Type type in Types)
                    {
                        bool result = test(fieldType, type);
                        if (result)
                        {
                            contains = result;
                            break;
                        }
                    }
                }

                if (checkingFlags.HasFlag(CheckingFlags.CheckSubclassTypes))
                    Check((f, t) => f.IsSubclassOf(t));
                if (checkingFlags.HasFlag(CheckingFlags.CheckSuperclassTypes) && !contains)
                    Check((f, t) => f.IsSubclassOf(t));
                if (checkingFlags.HasFlag(CheckingFlags.CheckSuperclassTypes) && !contains)
                    Check((f, t) => t.IsSubclassOf(f));
                if (checkingFlags.HasFlag(CheckingFlags.CheckSuperclassTypes) && !contains)
                    Check((f, t) => f.IsAssignableFrom(t));
                if (checkingFlags.HasFlag(CheckingFlags.CheckCanBeAssignedTypes) && !contains)
                    Check((f, t) => t.IsAssignableFrom(f));
            }

            if (contains == checkingFlags.HasFlag(CheckingFlags.IsBlackList))
                Debug.LogException(new ArgumentException($"{attribute.Name} {ErrorMessage}. Field {fieldInfo.Name} of {classType.Name} is {fieldType.Name} type."));
        }
#endif
    }
}