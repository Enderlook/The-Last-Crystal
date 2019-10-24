using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdditionalAttributes.Internal
{
    public static class AttributeUsageHelper
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

        public static HashSet<Type> GetHashsetTypes(Type[] types, CheckingFlags checkingFlags)
        {
            if ((checkingFlags & CheckingFlags.IncludeEnumerableTypes) != 0)
            {
                HashSet<Type> hashSet = new HashSet<Type>();
                for (int i = 0; i < types.Length; i++)
                {
                    Type type = types[i];
                    hashSet.Add(type);
                    hashSet.Add(typeof(List<>).MakeGenericType(type));
                    hashSet.Add(type.MakeArrayType());
                }
                return hashSet;

            }
            else
                return new HashSet<Type>(types);
        }

        public static string GetTextTypes(HashSet<Type> types, CheckingFlags checkingFlags)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .Append((checkingFlags & CheckingFlags.IsBlackList) != 0 ? "doesn't" : "only")
                .Append(" accept types of ")
                .Append(string.Join(", ", types.Select(e => e.Name)));
            if ((checkingFlags & CheckingFlags.CheckSubclassTypes) != 0)
                stringBuilder.Append(", their subclasses");
            if ((checkingFlags & CheckingFlags.CheckSuperclassTypes) != 0)
                stringBuilder.Append(", their superclasses");
            if ((checkingFlags & CheckingFlags.CheckSuperclassTypes) != 0)
                stringBuilder.Append(", types assignable to them");
            if ((checkingFlags & CheckingFlags.CheckCanBeAssignedTypes) != 0)
                stringBuilder.Append(", types assignable from them");
            return stringBuilder.ToString();
        }

        public static void CheckContains(string attributeCheckerName, HashSet<Type> types, CheckingFlags checkingFlags, string allowedTypes, Type toCheckType, string attributeName, string toCheckName)
        {
            bool contains = types.Contains(toCheckType);

            if (!contains)
            {
                void Check(Func<Type, Type, bool> test)
                {
                    foreach (Type type in types)
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
                // We could use checkingFlags.HasFlag(flag), but it's ~10 times slower
                if ((checkingFlags & CheckingFlags.CheckSubclassTypes) != 0)
                    Check((f, t) => f.IsSubclassOf(t));
                if ((checkingFlags & CheckingFlags.CheckSuperclassTypes) != 0 && !contains)
                    Check((f, t) => t.IsSubclassOf(f));
                if ((checkingFlags & CheckingFlags.CheckCanBeAssignedTypes) != 0 && !contains)
                    Check((f, t) => f.IsAssignableFrom(t));
                if ((checkingFlags & CheckingFlags.CheckIsAssignableTypes) != 0 && !contains)
                    Check((f, t) => t.IsAssignableFrom(f));
            }

            if (contains == ((checkingFlags & CheckingFlags.IsBlackList) != 0))
                Debug.LogException(new ArgumentException($"According to {attributeCheckerName}, {attributeName} {allowedTypes}. {toCheckName} is {toCheckType.Name} type."));
        }
    }
}