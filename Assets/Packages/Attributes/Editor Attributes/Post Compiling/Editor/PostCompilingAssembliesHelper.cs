using AdditionalAttributes.PostCompiling.Internal;

using AdditionalExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor.Callbacks;
using UnityEditor.Compilation;

using UnityEngine;

using UnityAssembly = UnityEditor.Compilation.Assembly;

namespace AdditionalAttributes.PostCompiling
{
    public static class PostCompilingAssembliesHelper
    {
#pragma warning disable CS0649
        // Type Less Enum
        private static readonly Dictionary<int, Action<Type>> executeOnEachTypeLessEnums = new Dictionary<int, Action<Type>>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed on each <see cref="Type"/> in the assemblies compiled by Unity which <see cref="Type.IsEnum"/> is <see langword="false"/>.<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteOnEachTypeWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeOnEachTypeLessEnums(Action<Type> action, int order) => SubscribeCallback(executeOnEachTypeLessEnums, action, order);
        // Type Enum
        private static readonly Dictionary<int, Action<Type>> executeOnEachTypeEnum = new Dictionary<int, Action<Type>>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed on each <see cref="Type"/> in the assemblies compiled by Unity which <see cref="Type.IsEnum"/> is <see langword="true"/>.<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteOnEachTypeWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeOnEachTypeEnum(Action<Type> action, int order) => SubscribeCallback(executeOnEachTypeEnum, action, order);
        // Member
        private static readonly Dictionary<int, Action<MemberInfo>> executeOnEachMemberOfTypes = new Dictionary<int, Action<MemberInfo>>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed on each member of each <see cref="Type"/> in the assemblies compiled by Unity.<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteOnEachTypeWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeOnEachMemberOfTypes(Action<MemberInfo> action, int order) => SubscribeCallback(executeOnEachMemberOfTypes, action, order);
        // Serializable By Unity Field
        private static readonly Dictionary<int, Action<FieldInfo>> executeOnEachSerializableByUnityFieldOfTypes = new Dictionary<int, Action<FieldInfo>>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed on each field of each <see cref="Type"/> in the assemblies compiled by Unity which can be serialized by Unity (<seealso cref="ReflectionHelper.CanBeSerializedByUnity(FieldInfo)"/>).<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteOnEachTypeWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeOnEachSerializableByUnityFieldOfTypes(Action<FieldInfo> action, int order) => SubscribeCallback(executeOnEachSerializableByUnityFieldOfTypes, action, order);
        // Non Serializable By Unity Field
        private static readonly Dictionary<int, Action<FieldInfo>> executeOnEachNonSerializableByUnityFieldOfTypes = new Dictionary<int, Action<FieldInfo>>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed on each member of each <see cref="Type"/> in the assemblies compiled by Unity which can be serialized by Unity (<seealso cref="ReflectionHelper.CanBeSerializedByUnity(FieldInfo)"/>).<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteOnEachTypeWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeOnEachNonSerializableByUnityFieldOfTypes(Action<FieldInfo> action, int order) => SubscribeCallback(executeOnEachNonSerializableByUnityFieldOfTypes, action, order);
        // Property
        private static readonly Dictionary<int, Action<PropertyInfo>> executeOnEachPropertyOfTypes = new Dictionary<int, Action<PropertyInfo>>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed on each property of each <see cref="Type"/> in the assemblies compiled by Unity.<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteOnEachTypeWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeOnEachPropertyOfTypes(Action<PropertyInfo> action, int order) => SubscribeCallback(executeOnEachPropertyOfTypes, action, order);
        // Method
        private static readonly Dictionary<int, Action<MethodInfo>> executeOnEachMethodOfTypes = new Dictionary<int, Action<MethodInfo>>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed on each method of each <see cref="Type"/> in the assemblies compiled by Unity.<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteOnEachTypeWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeOnEachMethodOfTypes(Action<MethodInfo> action, int order) => SubscribeCallback(executeOnEachMethodOfTypes, action, order);
        // Once
        private static readonly Dictionary<int, Action> executeOnce = new Dictionary<int, Action>();
        /// <summary>
        /// Subscribes <paramref name="action"/> to be executed once wen Unity ompiles assemblies.<br>
        /// If possible, it's strongly advisable to use <see cref="ExecuteWhenScriptsReloads"/> attribute instead of this method.
        /// </summary>
        /// <param name="action">Action to subscribe.</param>
        /// <param name="order">Priority of this method to execute. After all other callbacks of lower order are executed on all targets this will be executed.</param>
        public static void SubscribeToExecuteOnce(Action action, int order) => SubscribeCallback(executeOnce, action, order);

        private static void SubscribeCallback<T>(Dictionary<int, Action<T>> dictionary, Action<T> action, int order)
        {
            if (dictionary.ContainsKey(order))
                dictionary[order] += action;
            else
                dictionary.Add(order, action);
        }

        private static void SubscribeCallback(Dictionary<int, Action> dictionary, Action action, int order)
        {
            if (dictionary.ContainsKey(order))
                dictionary[order] += action;
            else
                dictionary.Add(order, action);
        }

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static readonly List<Type> enumTypes = new List<Type>();
        private static readonly List<Type> nonEnumTypes = new List<Type>();
        private static readonly List<MemberInfo> memberInfos = new List<MemberInfo>();
        private static readonly List<FieldInfo> fieldInfosNonSerializableByUnity = new List<FieldInfo>();
        private static readonly List<FieldInfo> fieldInfosSerializableByUnity = new List<FieldInfo>();
        private static readonly List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
        private static readonly List<MethodInfo> methodInfos = new List<MethodInfo>();

        [DidReloadScripts(2)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Editor")]
        private static void ExecuteAnalysis()
        {
            ScanAssemblies();
            ExecuteCallbacks();
        }

        private static void ScanAssemblies()
        {
            foreach (Type classType in GetAllTypesOfPlayerAndEditorAssemblies())
            {
                if (classType.GetCustomAttribute<DoNotInspectAttribute>() == null)
                {
                    if (classType.IsEnum)
                        enumTypes.Add(classType);
                    else
                    {
                        nonEnumTypes.Add(classType);

                        foreach (MemberInfo memberInfo in classType.GetMembers(bindingFlags))
                        {
                            memberInfos.Add(memberInfo);

                            switch (memberInfo.MemberType)
                            {
                                case MemberTypes.Field:
                                    FieldInfo fieldInfo = (FieldInfo)memberInfo;
                                    if (fieldInfo.CanBeSerializedByUnity())
                                        fieldInfosSerializableByUnity.Add((FieldInfo)memberInfo);
                                    else
                                        fieldInfosNonSerializableByUnity.Add((FieldInfo)memberInfo);
                                    break;
                                case MemberTypes.Property:
                                    propertyInfos.Add((PropertyInfo)memberInfo);
                                    break;
                                case MemberTypes.Method:
                                    MethodInfo methodInfo = (MethodInfo)memberInfo;
                                    methodInfos.Add(methodInfo);
                                    GetExecuteAttributes(methodInfo);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private static void GetExecuteAttributes(MethodInfo methodInfo)
        {
            if (!methodInfo.IsStatic)
                return;
            foreach (BaseExecuteWhenScriptsReloads attribute in methodInfo.GetCustomAttributes<BaseExecuteWhenScriptsReloads>())
            {
                int loop = attribute.loop;
                if (attribute is ExecuteOnEachTypeWhenScriptsReloads executeOnEachTypeWhenScriptsReloads)
                {
                    ExecuteOnEachTypeWhenScriptsReloads.TypeFlags typeFlags = executeOnEachTypeWhenScriptsReloads.typeFilter;

                    if (TryGetDelegate(methodInfo, out Action<Type> action))
                    {
                        if ((typeFlags & ExecuteOnEachTypeWhenScriptsReloads.TypeFlags.IsEnum) != 0)
                            SubscribeOnEachTypeEnum(action, loop);
                        if ((typeFlags & ExecuteOnEachTypeWhenScriptsReloads.TypeFlags.IsNonEnum) != 0)
                            SubscribeOnEachTypeLessEnums(action, loop);
                    }
                }
                else if (attribute is ExecuteOnEachMemberOfEachTypeWhenScriptsReloads executeOnEachMemberOfEachTypeWhenScriptsReloads)
                {
                    if (TryGetDelegate(methodInfo, out Action<MemberInfo> action))
                        SubscribeOnEachMemberOfTypes(action, loop);
                }
                else if (attribute is ExecuteOnEachFieldOfEachTypeWhenScriptsReloads executeOnEachFieldOfEachTypeWhenScriptsReloads)
                {
                    if (TryGetDelegate(methodInfo, out Action<FieldInfo> action))
                    {
                        ExecuteOnEachFieldOfEachTypeWhenScriptsReloads.FieldFlags fieldFags = executeOnEachFieldOfEachTypeWhenScriptsReloads.fieldFilter;
                        if ((fieldFags & ExecuteOnEachFieldOfEachTypeWhenScriptsReloads.FieldFlags.SerializableByUnity) != 0)
                            SubscribeOnEachSerializableByUnityFieldOfTypes(action, loop);
                        if ((fieldFags & ExecuteOnEachFieldOfEachTypeWhenScriptsReloads.FieldFlags.NotSerializableByUnity) != 0)
                            SubscribeOnEachNonSerializableByUnityFieldOfTypes(action, loop);
                    }
                }
                else if (attribute is ExecuteOnEachPropertyOfEachTypeWhenScriptsReloads executeOnEachPropertyOfEachTypeWhenScriptsReloads)
                {
                    if (TryGetDelegate(methodInfo, out Action<PropertyInfo> action))
                        SubscribeOnEachPropertyOfTypes(action, loop);
                }
                else if (attribute is ExecuteOnEachMethodOfEachTypeWhenScriptsReloads executeOnEachMethodOfEachTypeWhenScriptsReloads)
                {
                    if (TryGetDelegate(methodInfo, out Action<MethodInfo> action))
                        SubscribeOnEachMethodOfTypes(action, loop);
                }
                else if (attribute is ExecuteWhenScriptsReloads executeWhenScriptsReloads)
                {
                    if (TryGetDelegate(methodInfo, out Action action))
                    {
                        SubscribeToExecuteOnce(action, loop);
                    }
                }
            }
        }

        private static readonly string ATTRIBUTE_METHOD_ERROR = $"Method {{0}} in {{1}} does not follow the requirements of attribute {nameof(ExecuteOnEachTypeWhenScriptsReloads)}. It's signature must be {{2}}.";

        private static bool TryGetDelegate<T>(MethodInfo methodInfo, out Action<T> action)
        {
            action = (Action<T>)TryGetDelegate<Action<T>>(methodInfo);
            return action != null;
        }

        private static bool TryGetDelegate(MethodInfo methodInfo, out Action action)
        {
            action = (Action)TryGetDelegate<Action>(methodInfo);
            return action != null;
        }

        private static Delegate TryGetDelegate<T>(MethodInfo methodInfo)
        {
            try
            {
                /* At first sight `e => methodInfo.Invoke(null, Array.Empty<object>());` might seem faster
                   But actually, CreateDelegate does amortize if called through MulticastDelegate multiple times, like we are going to do.*/
                return methodInfo.CreateDelegate(typeof(T));
            }
            catch (ArgumentException e)
            {
                Type[] genericArguments = typeof(T).GetGenericArguments();
                string signature = genericArguments.Length == 0 ? "nothing" : string.Join(", ", genericArguments.Select(a => a.Name));
                Debug.LogException(new ArgumentException(string.Format(ATTRIBUTE_METHOD_ERROR, methodInfo.Name, methodInfo.DeclaringType, signature), e));
            }
            return null;
        }

        private static void ExecuteCallbacks()
        {
            foreach (int loop in GetKeySortedUnion(
                executeOnEachTypeEnum.Keys,
                executeOnEachTypeLessEnums.Keys,
                executeOnEachMemberOfTypes.Keys,
                executeOnEachSerializableByUnityFieldOfTypes.Keys,
                executeOnEachNonSerializableByUnityFieldOfTypes.Keys,
                executeOnEachPropertyOfTypes.Keys,
                executeOnEachMethodOfTypes.Keys
                ))
            {
                void ExecuteList<T>(Dictionary<int, Action<T>> callbacks, List<T> values) => ExecuteLoop(loop, callbacks, values);
                ExecuteList(executeOnEachTypeEnum, enumTypes);
                ExecuteList(executeOnEachTypeLessEnums, nonEnumTypes);
                ExecuteList(executeOnEachMemberOfTypes, memberInfos);
                ExecuteList(executeOnEachSerializableByUnityFieldOfTypes, fieldInfosSerializableByUnity);
                ExecuteList(executeOnEachNonSerializableByUnityFieldOfTypes, fieldInfosNonSerializableByUnity);
                ExecuteList(executeOnEachPropertyOfTypes, propertyInfos);
                ExecuteList(executeOnEachMethodOfTypes, methodInfos);
                if (executeOnce.TryGetValue(loop, out Action action))
                    action();
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
        /// <returns>All types of Player and Editor assemblies.</returns>
        public static IEnumerable<Type> GetAllTypesOfPlayerAndEditorAssemblies()
        {
            IEnumerable<UnityAssembly> unityAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.Editor).Concat(CompilationPipeline.GetAssemblies(AssembliesType.Player));
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (unityAssemblies.ContainsBy(e => e.name == assembly.GetName().Name))
                {
                    // This is much more expensive that ContainsBy so we put this bellow
                    // Check if we should not read it
                    if (assembly.GetCustomAttribute<DoNotInspectAttribute>() == null)
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
}