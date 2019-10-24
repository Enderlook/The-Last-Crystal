using System;
using System.Collections.Generic;
using System.Reflection;
using AdditionalAttributes.AttributeUsage.Internal;
using UnityEngine;

namespace AdditionalAttributes.AttributeUsage
{
    [AttributeUsageRequireDataType(typeof(Attribute), typeFlags = AttributeUsageHelper.TypeFlags.CheckSubclassTypes)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageMethodAttribute : Attribute
    {
        /// <summary>
        /// The parameter mode.
        /// </summary>
        public enum ParameterMode
        {
            /// <summary>
            /// Specifies an common parameter.<br>
            /// If <see cref="parameterNumber"/> is 0, this will be ignored.
            /// </summary>
            Common,
            /// <summary>
            /// Specifies an in parameter.<br>
            /// If <see cref="parameterNumber"/> is 0, this will be ignored.
            /// </summary>
            In,
            /// <summary>
            /// Specifies an out parameter.
            /// If <see cref="parameterNumber"/> is 0, this will be ignored.
            /// </summary>
            Out,
            /// <summary>
            /// Specifies a reference parameter.
            /// If <see cref="parameterNumber"/> is 0, this will be ignored.
            /// </summary>
            Ref,
            /// <summary>
            /// Specifies that this parameter should not exist.
            /// </summary>
            VoidOrNone
        }

        /// <summary>
        /// Number of the parameter to check.<br>
        /// Example:<br>
        ///     • 0 -> Return type.<br>
        ///     • 1 -> First method parameter.<br>
        ///     • 2 -> Second method parameter<br>
        ///     • 3 -> Third method parameter...
        /// </summary>
        private readonly int parameterNumber;

        /// <summary>
        /// Determine the type of parameter. Use <see cref="ParameterMode.VoidOrNone"/> to specify that it should not exist.
        /// </summary>
        public ParameterMode parameterType;

        /// <summary>
        /// Additional checking rules.
        /// </summary>
        public AttributeUsageHelper.TypeFlags checkingFlags;

        private readonly Type[] basicTypes;

        /// <summary>
        /// Each time Unity compile script, they will be analyzed to check if the attribute is being used in proper methods.
        /// </summary>
        /// <param name="parameterNumber">Number of the parameter to check.<br>
        /// Example:<br>
        ///     • 0 -> Return type.<br>
        ///     • 1 -> First method parameter.<br>
        ///     • 2 -> Second method parameter<br>
        ///     • 3 -> Third method parameter...</param>
        /// <param name="types">Data types allowed. Use <see cref="CheckingFlags.IsBlackList"/> in <see cref="checkingFlags"/> to become it forbidden data types.<br>
        /// If empty, any data type is allowed.</param>
        /// To specify that a parameter should not exist, use <see cref="ParameterMode.VoidOrNone"/> in <see cref="parameterType"/>.</param>
        public AttributeUsageMethodAttribute(int parameterNumber, params Type[] types)
        {
            this.parameterNumber = parameterNumber;
            basicTypes = types;
        }

#if UNITY_EDITOR
        private HashSet<Type> Types => types ?? (types = AttributeUsageHelper.GetHashsetTypes(basicTypes, false));
        private HashSet<Type> types;

        private string allowedTypes;
        private string AllowedTypes => allowedTypes ?? (allowedTypes = AttributeUsageHelper.GetTextTypes(types, checkingFlags, false));

        private readonly string messageBase = $"According to {nameof(AttributeUsageMethodAttribute)},";

        /// <summary>
        /// Unity Editor only.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="attributeName"></param>
        public void CheckAllowance(MethodInfo methodInfo, string attributeName)
        {
            string GetMessageInit() => $"{messageBase} {attributeName} require a method with";

            if (parameterNumber == 0)
            {
                Debug.Log(parameterNumber);
                // Check return type
                Type type = methodInfo.ReturnType;

                if (parameterType == ParameterMode.VoidOrNone)
                {
                    if (type == typeof(void))
                        Debug.LogException(new ArgumentException($"{GetMessageInit()} a return type must be {typeof(void).Name}. {methodInfo.Name} is {type} type."));
                }
                else if (parameterType == ParameterMode.Common)
                    AttributeUsageHelper.CheckContains(nameof(AttributeUsageMethodAttribute), Types, checkingFlags, false, AllowedTypes, type, attributeName, "Return Type");
                else
                    Debug.LogException(new ArgumentException($"{nameof(AttributeUsageMethodAttribute)} can only have as {nameof(parameterType)} {nameof(ParameterMode.Common)} or {nameof(ParameterMode.VoidOrNone)} if used with {nameof(parameterNumber)} 0. Attribute in {attributeName} has a {nameof(parameterNumber)} 0 but {nameof(parameterType)} {parameterType}."));
            }
            else
            {
                // Check parameter
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                ParameterInfo parameterInfo;
                try
                {
                    parameterInfo = parameterInfos[parameterNumber - 1];
                }
                catch (IndexOutOfRangeException e)
                {
                    // Parameter doesn't exist, check if was on purpose
                    if (parameterType != ParameterMode.VoidOrNone)
                        Debug.LogException(new ArgumentOutOfRangeException($"{GetMessageInit()} a {parameterNumber}º parameter. {methodInfo.Name} only have {parameterInfos.Length} parameter{(parameterInfos.Length > 0 ? "s" : "")}.", e));
                    return;
                }

                // Check if should exist
                if (parameterType == ParameterMode.VoidOrNone)
                {
                    Debug.LogException(new ArgumentException($"{GetMessageInit()} only {parameterNumber - 1} parameter{(parameterNumber - 1 > 0 ? "s" : "")}.")); // - 1 because the last parameter is ParameterMode.VoidOrNone
                    return;
                }

                // Get parameter keyword
                ParameterMode mode = ParameterMode.Common;
                if (parameterInfo.ParameterType.IsByRef)
                {
                    // Both `out` and `ref` has IsByRef = true, so we must check IsOut in order to split them.
                    // https://stackoverflow.com/a/38110036 from https://stackoverflow.com/questions/1551761/ref-parameters-and-reflection
                    if (parameterInfo.IsOut)
                        mode = ParameterMode.Out;
                    else
                        mode = ParameterMode.Out;
                }
                else if (parameterInfo.IsIn)
                    mode = ParameterMode.In;

                // Check parameter keyword
                void RaiseKeyWordException(string keywordString) => Debug.LogException(new ArgumentException($"{GetMessageInit()} a parameter at {parameterInfo.Position} position named {parameterInfo.Name} that has {keywordString} keyword, according to {nameof(parameterType)} {nameof(ParameterMode)} {mode}. Instead has {parameterType}."));
                if (parameterType != mode)
                {
                    if (parameterType != ParameterMode.Out)
                    {
                        RaiseKeyWordException("'out'");
                        return;
                    }
                    else if (parameterType != ParameterMode.Ref)
                    {
                        RaiseKeyWordException("'ref'");
                        return;
                    }
                    else if (parameterType != ParameterMode.In)
                    {
                        RaiseKeyWordException("'in'");
                        return;
                    }
                    else if (parameterType != ParameterMode.Common)
                    {
                        RaiseKeyWordException("no");
                        return;
                    }
                    else
                    {
                        Debug.LogException(new InvalidOperationException("Invalid state. This should never happen... if you can see this, it means it wasn't so 'unreachable state' after all..."));
                        return;
                    }
                }

                if (Types.Count != 0) // It 0, any type is allowed
                    AttributeUsageHelper.CheckContains(nameof(AttributeUsageMethodAttribute), Types, checkingFlags, false, AllowedTypes, parameterInfo.ParameterType, $"{attributeName} parameter {parameterInfo.Name} in position {parameterInfo.Position}", $"Parameter");
            }
        }
    }
#endif
}