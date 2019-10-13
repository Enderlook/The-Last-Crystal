using UnityEngine;

namespace AdditionalExtensions
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
        public static T CastOrDefault<T>(this object obj)
        {
            return obj is T ? (T)obj : (default);
        }

        /// <summary>
        /// Don't use me.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class RequireStruct<T> where T : struct { }
        /// <summary>
        /// Don't use me.
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        public static T? CastOrNull<T>(this object obj, RequireStruct<T> ignoreMe = null) where T : struct
        {
            return obj is T ? (T?)(T)obj : null;
        }

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
        public static T CastOrNull<T>(this object obj, RequireClass<T> ignoreMe = null) where T : class
        {
            return obj is T ? (T)obj : null;
        }
    }
}