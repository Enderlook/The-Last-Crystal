using AdditionalExceptions;

using System;
using System.Linq;
using System.Reflection;

namespace AdditionalExtensions
{
    public static class ReflectionExtesions
    {
        /// <summary>
        /// Returns the value of the first member of <see cref="obj"/> which:
        /// <list type="bullet">
        ///     <item><description>If <see cref="MethodInfo"/>, its <see cref="MethodInfo.ReturnType"/> must be <typeparamref name="T"/> and it must not require mandatory parameters (can have optionals or params).</description></item>
        ///     <item><description>If <see cref="PropertyInfo"/>, its <see cref="PropertyInfo.PropertyType"/> must be <typeparamref name="T"/> and it must have a setter.</description></item>
        ///     <item><description>If <see cref="FieldInfo"/>, its <see cref="FieldInfo.FieldType"/> must be <typeparamref name="T"/>.</description></item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="obj">Object to look for <see cref="MemberInfo"/> and results.</param>
        /// <param name="memberName">Name of the <see cref="MemberInfo"/> looked for.</param>
        /// <returns>Result of the first <see cref="MemberInfo"/> of <see cref="ojb"/> in match the criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="obj"/> or <paramref name="memberName"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="memberName"/> is empty.</exception>
        /// <exception cref="MemberNotFoundException">Thrown no <see cref="MemberInfo"/> with name <paramref name="memberName"/> could be found in <paramref name="obj"/>.</exception>
        /// <exception cref="MatchingMemberNotFoundException">Thrown no <see cref="MemberInfo"/> with name <paramref name="memberName"/> in <paramref name="obj"/> matched the necessary requirements.</exception>
        public static T GetValueFromFirstMember<T>(this object obj, string memberName)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (memberName is null) throw new ArgumentNullException(nameof(memberName));
            else if (memberName.Length == 0) throw new ArgumentException("Can't be empty", nameof(memberName));

            MemberInfo memberInfo = GetFirstMemberInfoInMatchReturn<T>(obj.GetType(), memberName);

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return (T)((FieldInfo)memberInfo).GetValue(obj);
                case MemberTypes.Method:
                    return (T)((MethodInfo)memberInfo).Invoke(obj);
                default:
                    throw new ImpossibleStateException();
            }
        }

        /// <summary>
        /// Returns the first member of <see cref="obj"/> which:
        /// <list type="bullet">
        ///     <item><description>If <see cref="MethodInfo"/>, its <see cref="MethodInfo.ReturnType"/> must be <typeparamref name="T"/> and it must not require mandatory parameters (can have optionals or params).</description></item>
        ///     <item><description>If <see cref="PropertyInfo"/>, its <see cref="PropertyInfo.PropertyType"/> must be <typeparamref name="T"/> and it must have a setter.</description></item>
        ///     <item><description>If <see cref="FieldInfo"/>, its <see cref="FieldInfo.FieldType"/> must be <typeparamref name="T"/>.</description></item>
        /// </list>
        /// <see cref="PropertyInfo"/> are always returned as <see cref="MethodInfo"/> because it returns their getter.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="type">Type to look for <see cref="MemberInfo"/> and results.</param>
        /// <param name="memberName">Name of the <see cref="MemberInfo"/> looked for.</param>
        /// <returns>Result of the first <see cref="MemberInfo"/> of <see cref="ojb"/> in match the criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> or <paramref name="memberName"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="memberName"/> is empty.</exception>
        /// <exception cref="MemberNotFoundException">Thrown no <see cref="MemberInfo"/> with name <paramref name="memberName"/> could be found in <paramref name="obj"/>.</exception>
        /// <exception cref="MatchingMemberNotFoundException">Thrown no <see cref="MemberInfo"/> with name <paramref name="memberName"/> in <paramref name="obj"/> matched the necessary requirements.</exception>
        public static MemberInfo GetFirstMemberInfoInMatchReturn<T>(Type type, string memberName)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (memberName is null) throw new ArgumentNullException(nameof(memberName));
            else if (memberName.Length == 0) throw new ArgumentException("Can't be empty", nameof(memberName));

            MemberInfo[] memberInfos = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Static);
            if (memberInfos.Length == 0)
                throw new MemberNotFoundException(memberName, type);

            foreach (MemberInfo memberInfo in memberInfos)
            {
                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = (FieldInfo)memberInfo;
                        if (fieldInfo.FieldType == typeof(T))
                            return fieldInfo;
                        break;
                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                        if (propertyInfo.CanRead)
                        {
                            foreach (MethodInfo accessor in propertyInfo.GetAccessors(true))
                            {
                                // Check if it is a getter (has return type) and is the type we are looking for
                                if (accessor.ReturnType == typeof(T))
                                    return accessor;
                            }
                        }
                        break;
                    case MemberTypes.Method:
                        MethodInfo methodInfo = (MethodInfo)memberInfo;
                        if (methodInfo.ReturnType == typeof(T))
                        {
                            // Check if the method doesn't require any mandatory parameter
                            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                            if (parameterInfos.Count(e => !e.IsOptional) == 0)
                                return methodInfo;
                        }
                        break;
                }
            }
            throw new MatchingMemberNotFoundException(memberName, type, typeof(T));
        }

        /// <summary>
        /// Returns the first member of <see cref="obj"/> which:
        /// <list type="bullet">
        ///     <item><description>If <see cref="MethodInfo"/>, its <see cref="MethodInfo.ReturnType"/> must be <typeparamref name="T"/> and it must not require mandatory parameters (can have optionals or params).</description></item>
        ///     <item><description>If <see cref="PropertyInfo"/>, its <see cref="PropertyInfo.PropertyType"/> must be <typeparamref name="T"/> and it must have a setter.</description></item>
        ///     <item><description>If <see cref="FieldInfo"/>, its <see cref="FieldInfo.FieldType"/> must be <typeparamref name="T"/>.</description></item>
        /// </list>
        /// <see cref="PropertyInfo"/> are always returned as <see cref="MethodInfo"/> because it returns their getter.
        /// </summary>
        /// <typeparam name="T">Type to look for <see cref="MemberInfo"/> and results.</typeparam>
        /// <typeparam name="U">Result type.</typeparam>
        /// <param name="memberName">Name of the <see cref="MemberInfo"/> looked for.</param>
        /// <returns>Result of the first <see cref="MemberInfo"/> of <see cref="ojb"/> in match the criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> or <paramref name="memberName"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="memberName"/> is empty.</exception>
        /// <exception cref="MemberNotFoundException">Thrown no <see cref="MemberInfo"/> with name <paramref name="memberName"/> could be found in <paramref name="obj"/>.</exception>
        /// <exception cref="MatchingMemberNotFoundException">Thrown no <see cref="MemberInfo"/> with name <paramref name="memberName"/> in <paramref name="obj"/> matched the necessary requirements.</exception>
        public static MemberInfo GetFirstMemberInfoInMatchReturn<T, U>(string memberName) => GetFirstMemberInfoInMatchReturn<U>(typeof(T), memberName);

        /// <summary>
        /// Invokes <paramref name="methodInfo"/> using <paramref name="obj"/> has it class instance and without any parameter (expect optionals).
        /// </summary>
        /// <param name="methodInfo">Method to invoke.</param>
        /// <param name="obj">Instance of the class to invoke.</param>
        /// <returns>Result of the method invoked.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="methodInfo"/> is <see langword="null"/>.</exception>
        public static object Invoke(this MethodInfo methodInfo, object obj)
        {
            if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
            if (!methodInfo.HasNoMandatoryParameters(out object[] parameters)) throw new HasMandatoryParametersException(methodInfo);

            return methodInfo.Invoke(obj, parameters);
        }

        /// <summary>
        /// Return if the <paramref name="methodInfo"/> only has optional or params parameters.
        /// </summary>
        /// <param name="methodInfo">Method to check.</param>
        /// <returns>Whenever it only has optional or params parameters.</returns>
        /// <see cref="https://stackoverflow.com/a/627668/7655838"/>
        public static bool HasNoMandatoryParameters(this MethodInfo methodInfo)
        {
            if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));

            return methodInfo.GetParameters().All(e => e.IsOptional || e.IsDefined(typeof(ParamArrayAttribute)));
        }

        /// <summary>
        /// Return if the <paramref name="methodInfo"/> only has optional or params parameters.
        /// </summary>
        /// <param name="methodInfo">Method to check.</param>
        /// <param name="parameters">Array with default parameters to invoke.</param>
        /// <returns>Whenever it only has optional or params parameters.</returns>
        /// <see cref="https://stackoverflow.com/a/627668/7655838"/>
        public static bool HasNoMandatoryParameters(this MethodInfo methodInfo, out object[] parameters)
        {
            if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            parameters = new object[parameterInfos.Length];
            int i = 0;
            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                if (parameterInfo.IsOptionalOrParam(out object parameter))
                    parameters[i++] = parameter;
                else
                    return false;
            }
            return true;
        }

        public static bool IsOptionalOrParam(this ParameterInfo parameterInfo) => parameterInfo.IsOptional || parameterInfo.HasDefaultValue || parameterInfo.IsDefined(typeof(ParamArrayAttribute));

        public static bool IsOptionalOrParam(this ParameterInfo parameterInfo, out object parameter)
        {
            // https://stackoverflow.com/a/16187807/7655838
            if (parameterInfo.IsOptional || parameterInfo.HasDefaultValue)
            {
                parameter = Type.Missing;
                return true;
            }
            else if (parameterInfo.IsDefined(typeof(ParamArrayAttribute)))
            {
                parameter = Array.CreateInstance(parameterInfo.ParameterType, 0);
                return true;
            }
            parameter = default;
            return false;
        }

        /// <summary>
        /// Get default value of the given <see cref="TypeInfo"/> <see cref="TypeInfo.AsType"/>.
        /// </summary>
        /// <param name="typeInfo"><see cref="TypeInfo"/> to get default value.</param>
        /// <returns>Default value of <see cref="TypeInfo"/>.</returns>
        public static object GetDefault(this TypeInfo typeInfo)
        {
            if (typeInfo.IsValueType)
                return Activator.CreateInstance(typeInfo.AsType());
            return null;
        }

        /// <summary>
        /// Get default value of the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to get default value.</param>
        /// <returns>Default value of <see cref="Type"/>.</returns>
        public static object GetDefault(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<pendiente>")]
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(string memberName, Type type) : base($"No member named {memberName} not found in {nameof(Type)} {type}.") { }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<pendiente>")]
    public class MatchingMemberNotFoundException : Exception
    {
        public MatchingMemberNotFoundException(string memberName, Type type, Type returnType) : base($"No member named {memberName} not found in {nameof(Type)} {type} which return {nameof(Type)} (method without mandatory parameters), getter (property) or value (field) is of type {returnType}.") { }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<pendiente>")]
    public class HasMandatoryParametersException : Exception
    {
        public HasMandatoryParametersException(MethodInfo methodInfo) : base($"{nameof(MethodInfo)} {methodInfo} from {nameof(Type)} {methodInfo.ReflectedType} has parameters which aren't optional nor has the attribute {nameof(ParamArrayAttribute)}.") { }
    }
}
