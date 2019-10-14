#if UNITY_EDITOR
using System.Linq;
using UnityEngine;

namespace UnityEditorHelper
{
    public static class DebugHelper
    {
        /// <summary>
        /// Print to console all objects as strings separated by ', '.
        /// </summary>
        /// <param name="objects">Objects to print in console.</param>
        /// <seealso cref="LogError(object[])"/>
        /// <seealso cref="LogWarning(object[])"/>
        public static void Log(params object[] objects) => Debug.Log(GetStrings(objects));

        /// <summary>
        /// Print to console all objects as strings separated by ', '.
        /// </summary>
        /// <param name="objects">Objects to print in console.</param>
        /// <seealso cref="Log(object[])"/>
        /// <seealso cref="LogWarning(object[])"/>
        public static void LogError(params object[] objects) => Debug.LogError(GetStrings(objects));

        /// <summary>
        /// Print to console all objects as strings separated by ', '.
        /// </summary>
        /// <param name="objects">Objects to print in console.</param>
        /// <seealso cref="Log(object[])"/>
        /// <seealso cref="LogWarning(object[])"/>
        public static void LogWarning(params object[] objects) => Debug.LogWarning(GetStrings(objects));

        /// <summary>
        /// Return an string with all objects as strings separated by ', '.<br>
        /// <see langword="null"/> are turned into "null".
        /// </summary>
        /// <param name="objects">Objects to join as string.</param>
        public static string GetStrings(params object[] objects) => string.Join(", ", objects.Select(e => e == null ? "null" : e.ToString()));
    }
}
#endif