using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace AdditionalAttributes.PostCompiling
{
    internal static class ReflectionHelper
    {
        public static IEnumerable<(Type type, T attribute)> GetAllAttributesWithCustomAttributeInPlayerAndEditorAssemblies<T>()
        {
            foreach (Type type in PostCompilingAssembliesHelper.GetAllTypesOfPlayerAndEditorAssemblies())
            {
                // Check if a class came from Attribute before using reflection because otherwise it would be a waste of performance
                if (type.IsSubclassOf(typeof(Attribute)) && type.GetCustomAttribute(typeof(T), true) is T attribute)
                {
                    yield return (type, attribute);
                }
            }
        }

        private static IEnumerable<(T memberInfo, Type type, Attribute attribute)> GettAllAttributesOfMembersOf<T>(Type type, Func<Type, BindingFlags, T[]> getMembers) where T : MemberInfo
        {
            foreach (T memberInfo in getMembers(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Attribute _attribute = null;
                try
                {
                    foreach (Attribute attribute in memberInfo.GetCustomAttributes())
                    {
                        _attribute = attribute;
                        //yield return (memberInfo, type, attribute);
                    }
                }
                catch (BadImageFormatException) { } // https://github.com/mono/mono/issues/17278

                if (_attribute != null)
                    yield return (memberInfo, type, _attribute);
            }
        }

        public static IEnumerable<(MemberInfo memberInfo, Type type, Attribute attribute)> GettAllAttributesOfMembersOf(Type type)
        {
            return GettAllAttributesOfMembersOf(type, (e, b) => e.GetMembers(b));
        }

        public static IEnumerable<(FieldInfo fieldInfo, Type type, Attribute attribute)> GettAllAttributesOfFieldsOf(Type type)
        {
            return GettAllAttributesOfMembersOf(type, (e, b) => e.GetFields(b));
        }

        public static IEnumerable<(PropertyInfo propertyInfo, Type type, Attribute attribute)> GettAllAttributesOfPropertiesOf(Type type)
        {
            return GettAllAttributesOfMembersOf(type, (e, b) => e.GetProperties(b));
        }

        public static IEnumerable<(MethodInfo methodInfo, Type type, Attribute attribute)> GettAllAttributesOfMethodsOf(Type type)
        {
            return GettAllAttributesOfMembersOf(type, (e, b) => e.GetMethods(b));
        }

        private static readonly Type[] unityDefaultNonPrimitiveSerilizables = new Type[]
        {
            typeof(Vector2), typeof(Vector3), typeof(Vector4),
            typeof(Rect), typeof(Quaternion), typeof(Matrix4x4),
            typeof(Color), typeof(Color32), typeof(LayerMask),
            typeof(AnimationCurve), typeof(Gradient), typeof(RectOffset), typeof(GUIStyle)
        };

        public static bool CanBeSerializedByUnity(this FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPublic || fieldInfo.GetCustomAttribute<SerializeField>() != null)
            {
                if (fieldInfo.IsStatic || fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
                    return false;

                Type type = fieldInfo.FieldType;

                if (type.IsArray)
                    type = type.GetElementType();

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                    type = type.GetGenericArguments()[0];

                if (type.IsAbstract || type.IsGenericType)
                    return false;

                if (type.IsPrimitive || type.IsEnum || type.IsValueType || unityDefaultNonPrimitiveSerilizables.Contains(type))
                    return true;

                if (type.IsSubclassOf(typeof(UnityEngine.Object)))
                    return true;

                if (type.GetCustomAttributes<SerializableAttribute>() != null)
                    return true;
            }
            return false;
        }

    }
}