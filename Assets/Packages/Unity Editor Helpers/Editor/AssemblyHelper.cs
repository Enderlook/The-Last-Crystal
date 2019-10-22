using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdditionalExtensions;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;
using UnityAssembly = UnityEditor.Compilation.Assembly;

namespace UnityEditorHelper
{
    public static class AssemblyHelper
    {
#pragma warning disable CS0649
        private static readonly Dictionary<int, Action<Type>> executeOnEachTypeLessEnums = new Dictionary<int, Action<Type>>();
        public static void SubscribeOnEachTypeLessEnums(Action<Type> action, int order) => SubscribeCallback(executeOnEachTypeLessEnums, action, order);

        private static readonly Dictionary<int, Action<Type>> executeOnEachTypeEnum = new Dictionary<int, Action<Type>>();
        public static void SubscribeOnEachTypeEnum(Action<Type> action, int order) => SubscribeCallback(executeOnEachTypeEnum, action, order);

        private static readonly Dictionary<int, Action<MemberInfo>> executeOnEachMemberOfTypes = new Dictionary<int, Action<MemberInfo>>();
        public static void SubscribeOnEachMemberOfTypes(Action<MemberInfo> action, int order) => SubscribeCallback(executeOnEachMemberOfTypes, action, order);

        private static readonly Dictionary<int, Action<FieldInfo>> executeOnEachFieldOfTypes = new Dictionary<int, Action<FieldInfo>>();
        public static void SubscribeOnEachFieldOfTypes(Action<FieldInfo> action, int order) => SubscribeCallback(executeOnEachFieldOfTypes, action, order);

        private static readonly Dictionary<int, Action<PropertyInfo>> executeOnEachPropertyOfTypes = new Dictionary<int, Action<PropertyInfo>>();
        public static void SubscribeOnEachPropertyOfTypes(Action<PropertyInfo> action, int order) => SubscribeCallback(executeOnEachPropertyOfTypes, action, order);

        private static readonly Dictionary<int, Action<MethodInfo>> executeOnEachMethodOfTypes = new Dictionary<int, Action<MethodInfo>>();
        public static void SubscribeOnEachMethodOfTypes(Action<MethodInfo> action, int order) => SubscribeCallback(executeOnEachMethodOfTypes, action, order);

        private static readonly Dictionary<int, Action<ConstructorInfo>> executeOnEachConstructorOfTypes = new Dictionary<int, Action<ConstructorInfo>>();
        public static void SubscribeOnEachConstructorOfTypes(Action<ConstructorInfo> action, int order) => SubscribeCallback(executeOnEachConstructorOfTypes, action, order);

        private static readonly Dictionary<int, Action<EventInfo>> executeOnEachEventOfTypes = new Dictionary<int, Action<EventInfo>>();
        public static void SubscribeOnEachEventOfTypes(Action<EventInfo> action, int order) => SubscribeCallback(executeOnEachEventOfTypes, action, order);

#pragma warning restore CS0649
        private static void SubscribeCallback<T>(Dictionary<int, Action<T>> dictionary, Action<T> action, int order)
        {
            if (dictionary.ContainsKey(order))
                dictionary[order] += action;
            else
                dictionary.Add(order, action);
        }

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        [DidReloadScripts(2)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Editor")]
        private static void WalkThroughtPlayerAndEditorAssemblies()
        {
            List<Type> enumTypes = new List<Type>();
            List<Type> nonEnumTypes = new List<Type>();

            List<MemberInfo> memberInfos = new List<MemberInfo>();

            List<FieldInfo> fieldInfos = new List<FieldInfo>();
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
            List<MethodInfo> methodInfos = new List<MethodInfo>();
            List<ConstructorInfo> constructorInfos = new List<ConstructorInfo>();
            List<EventInfo> eventInfos = new List<EventInfo>();

            foreach (Type classType in GetAllTypesOfPlayerAndEditorAssemblies(true))
            {
                if (classType.IsEnum)
                    enumTypes.Add(classType);
                else
                {
                    nonEnumTypes.Add(classType);

                    foreach (MemberInfo member in classType.GetMembers(bindingFlags))
                    {
                        memberInfos.Add(member);

                        switch (member.MemberType)
                        {
                            case MemberTypes.Field:
                                fieldInfos.Add((FieldInfo)member);
                                break;
                            case MemberTypes.Property:
                                propertyInfos.Add((PropertyInfo)member);
                                break;
                            case MemberTypes.Method:
                                methodInfos.Add((MethodInfo)member);
                                break;
                            case MemberTypes.Constructor:
                                constructorInfos.Add((ConstructorInfo)member);
                                break;
                            case MemberTypes.Event:
                                eventInfos.Add((EventInfo)member);
                                break;
                        }
                    }
                }
            }

            foreach (int loop in GetKeySortedUnion(
                executeOnEachTypeEnum.Keys,
                executeOnEachTypeLessEnums.Keys,
                executeOnEachMemberOfTypes.Keys,
                executeOnEachFieldOfTypes.Keys,
                executeOnEachPropertyOfTypes.Keys,
                executeOnEachMethodOfTypes.Keys,
                executeOnEachEventOfTypes.Keys,
                executeOnEachConstructorOfTypes.Keys
                ))
            {
                void ExecuteList<T>(Dictionary<int, Action<T>> callbacks, List<T> values) => ExecuteLoop(loop, callbacks, values);
                ExecuteList(executeOnEachTypeEnum, enumTypes);
                ExecuteList(executeOnEachTypeLessEnums, nonEnumTypes);
                ExecuteList(executeOnEachMemberOfTypes, memberInfos);
                ExecuteList(executeOnEachFieldOfTypes, fieldInfos);
                ExecuteList(executeOnEachPropertyOfTypes, propertyInfos);
                ExecuteList(executeOnEachMethodOfTypes, methodInfos);
                ExecuteList(executeOnEachEventOfTypes, eventInfos);
                ExecuteList(executeOnEachConstructorOfTypes, constructorInfos);
            }
        }

        private static IEnumerable<int> GetKeySortedUnion(params IEnumerable<int>[] keys)
        {
            return keys.SelectMany(e => e).Distinct().OrderByDescending(e => e).Reverse();
        }

        private static void ExecuteLoop<T>(int loop, Dictionary<int, Action<T>> callbacks, List<T> values)
        {
            if (callbacks.TryGetValue(loop, out Action<T> action))
                values.ForEach(action);
        }

        /// <summary>
        /// Get all types of all Player and Editor assemblies.
        /// </summary>
        /// <param name="includeEnumTypes">Whenever it should include enum types or not.</param>
        /// <returns>All types of Player and Editor assemblies.</returns>
        public static IEnumerable<Type> GetAllTypesOfPlayerAndEditorAssemblies(bool includeEnumTypes)
        {
            IEnumerable<UnityAssembly> unityAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.Editor).Concat(CompilationPipeline.GetAssemblies(AssembliesType.Player));
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (unityAssemblies.ContainsBy(e => e.name == assembly.GetName().Name))
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}