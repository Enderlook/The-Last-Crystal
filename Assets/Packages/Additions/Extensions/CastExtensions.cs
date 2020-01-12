using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Additions.Extensions
{
    public static class CastExtensions
    {
        /// <summary>
        /// Try to cast <paramref name="obj"/> into <typeparamref name="T"/> in <paramref name="result"/>.<br/>
        /// If <paramref name="obj"/> isn't <typeparamref name="T"/>, <paramref name="result"/> is set with <c>default(<typeparamref name="T"/>)</c>.
        /// </summary>
        /// <typeparam name="T">Type of the value to cast.</typeparam>
        /// <param name="obj"><see cref="Object"/> to cast.</param>
        /// <param name="result">Casted result.</param>
        /// <returns><see langword="true"/> if the cast was successful. <see langword="false"> if it wasn't able to cast.</returns>
        /// <seealso href="https://codereview.stackexchange.com/questions/17982/trycastt-method">Source.</see>
        /// <seealso cref="CastOrDefault{T}(object)"/>
        /// <seealso cref="CastOrNull{T}(object, RequireStruct{T})"/>
        /// <seealso cref="CastOrNull{T}(object, RequireClass{T})"/>
        public static bool TryCast<T>(this object obj, out T result)
        {
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Try to cast <paramref name="obj"/> into <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value to cast.</typeparam>
        /// <param name="obj"><see cref="Object"/> to cast.</param>
        /// <returns>Return <c>(<typeparamref name="T"/>)<paramref name="obj"/></c>. <c>default(<typeparamref name="T"/>)<c> if it can't cast.</returns>
        public static T CastOrDefault<T>(this object obj) => obj is T ? (T)obj : (default);


        /// <summary>
        /// Don't use me.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It won't be used.")]
        public class RequireStruct<T> where T : struct { }

        /// <summary>
        /// Don't use me.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It won't be used.")]
        public class RequireClass<T> where T : class { }


        /// <summary>
        /// Try to cast <paramref name="obj"/> into <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value to cast.</typeparam>
        /// <param name="obj"><see cref="Object"/> to cast.</param>
        /// <param name="ignoreMe">Ignore this. Don't put anything here.</param>
        /// <returns>Return <c>(<typeparamref name="T"/>)<paramref name="obj"/></c>. <see langword="null"/> if it can't cast.</returns>
        /// <seealso href="https://stackoverflow.com/questions/2974519/generic-constraints-where-t-struct-and-where-t-class"/>
        /// <seealso cref="TryCast{T}(object, out T)"/>
        /// <seealso cref="CastOrDefault{T}(object)"/>
        /// <seealso cref="CastOrNull{T}(object, RequireClass{T})"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Required in order to make the overload.")]
        public static T? CastOrNull<T>(this object obj, RequireStruct<T> ignoreMe = null) where T : struct => obj is T ? (T?)(T)obj : null;

        /// <summary>
        /// Try to cast <paramref name="obj"/> into <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value to cast.</typeparam>
        /// <param name="obj"><see cref="Object"/> to cast.</param>
        /// <param name="ignoreMe">Ignore this. Don't put anything here.</param>
        /// <returns>Return <c>(<typeparamref name="T"/>)<paramref name="obj"/></c>. <see langword="null"/> if it can't cast.</returns>
        /// <seealso href="https://stackoverflow.com/questions/2974519/generic-constraints-where-t-struct-and-where-t-class"/>
        /// <seealso cref="TryCast{T}(object, out T)"/>
        /// <seealso cref="CastOrDefault{T}(object)"/>
        /// <seealso cref="CastOrNull{T}(object, RequireStruct{T})"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Required in order to make the overload.")]
        public static T CastOrNull<T>(this object obj, RequireClass<T> ignoreMe = null) where T : class => obj is T ? (T)obj : null;

        private static readonly Dictionary<Type, IEnumerable<Type>> PrimitiveTypeTable = new Dictionary<Type, IEnumerable<Type>>
        {
            { typeof(decimal), new[] { typeof(long), typeof(ulong) } },
            { typeof(double), new[] { typeof(float) } },
            { typeof(float), new[] { typeof(long), typeof(ulong) } },
            { typeof(ulong), new[] { typeof(uint) } },
            { typeof(long), new[] { typeof(int), typeof(uint) } },
            { typeof(uint), new[] { typeof(byte), typeof(ushort) } },
            { typeof(int), new[] { typeof(sbyte), typeof(short), typeof(ushort) } },
            { typeof(ushort), new[] { typeof(byte), typeof(char) } },
            { typeof(short), new[] { typeof(byte) } }
        };

        private static bool IsPrimitiveCastableTo(this Type fromType, Type toType)
        {
            if (fromType == null) throw new ArgumentNullException(nameof(fromType));
            if (toType == null) throw new ArgumentNullException(nameof(toType));

            Queue<Type> keyTypes = new Queue<Type>(new[] { toType });
            while (keyTypes.Count > 0)
            {
                Type key = keyTypes.Dequeue();
                if (key == fromType) return true; if (PrimitiveTypeTable.ContainsKey(key)) PrimitiveTypeTable[key].ToList().ForEach(keyTypes.Enqueue);
            }
            return false;
        }

        /// <summary>
        /// Determines if <paramref name="from"/> is castable to <paramref name="to"/>.
        /// This method does more than the is-operator and allows for primitives and implicit/explicit conversions to be compared properly.
        /// </summary>
        /// <param name="from">The type to cast from.</param>
        /// <param name="to">The type to be casted to.</param>
        /// <returns><see langword="true"/> if <paramref name="from"/> can be casted to <paramref name="to"/>. <see langword="false"/> otherwise.</returns>
        /// <see cref="https://stackoverflow.com/questions/18256742/c-sharp-is-operator-check-castability-for-all-conversions-available"/>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="from"/> or <paramref name="to"/> are <see langword="null"/></exception>
        public static bool IsCastableTo(this Type from, Type to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            return to.IsAssignableFrom(from)
                || from.IsPrimitiveCastableTo(to)
                || from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Any(m => m.ReturnType == to && m.Name == "op_Implicit" || m.Name == "op_Explicit");
        }

        /// <summary>
        /// Determines if <paramref name="from"/> is castable to <typeparamref name="T"/>.
        /// This method does more than the is-operator and allows for primitives and implicit/explicit conversions to be compared properly.
        /// </summary>
        /// <typeparam name="T">The type to be casted to.</typeparam>
        /// <param name="from">The type to cast from.</param>
        /// <returns><see langword="true"/> if <paramref name="from"/> can be casted to <typeparamref name="T"/>. <see langword="false"/> otherwise.</returns>
        /// <see cref="https://stackoverflow.com/questions/18256742/c-sharp-is-operator-check-castability-for-all-conversions-available"/>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="from"/> is <see langword="null"/></exception>

        public static bool IsCastableTo<T>(this Type from) => from.IsCastableTo(typeof(T));

        /// <summary>
        /// Determines if <typeparamref name="T"/> is castable to <typeparamref name="U"/>.
        /// This method does more than the is-operator and allows for primitives and implicit/explicit conversions to be compared properly.
        /// </summary>
        /// <typeparam name="T">The type to cast from.</typeparam>
        /// <typeparam name="U">The type to be casted to.</typeparam>
        /// <returns><see langword="true"/> if <paramref name="from"/> can be casted to <typeparamref name="T"/>. <see langword="false"/> otherwise.</returns>
        /// <see cref="https://stackoverflow.com/questions/18256742/c-sharp-is-operator-check-castability-for-all-conversions-available"/>
        public static bool IsCastableTo<T, U>() => typeof(T).IsCastableTo(typeof(U));
    }
}