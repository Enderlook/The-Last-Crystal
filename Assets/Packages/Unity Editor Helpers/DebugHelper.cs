#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Debug = UnityEngine.Debug;

namespace UnityEditorHelper
{
    public static class DebugHelper
    {
        public enum TraceMode { Hidden, OneLine, FullStackTrace }

        /// <summary>
        /// Default <see cref="TraceMode"/> used when a method provides a null trace.
        /// </summary>
        public static TraceMode traceMode = TraceMode.FullStackTrace;

        /// <summary>
        /// Print to console all <paramref name="objects"/> as strings separated by ', ', preceded by message.
        /// </summary>
        /// <param name="objects">Objects to print in console.</param>
        public static void Log(params object[] objects) => Debug.Log(GetStrings(objects));

        /// <summary>
        /// Print to console all <paramref name="objects"/> as strings separated by ', ', preceded by message.
        /// </summary>
        /// <param name="objects">Objects to print in console.</param>
        public static void LogWarning(params object[] objects) => Debug.LogWarning(GetStrings(objects));

        /// <summary>
        /// Print to console all <paramref name="objects"/> as strings separated by ', ', preceded by message.
        /// </summary>
        /// <param name="objects">Objects to print in console.</param>
        public static void LogError(params object[] objects) => Debug.LogError(GetStrings(objects));

        private static void MethodInformation(TraceMode? mode, string memberName, string sourceFilePath, int sourceLineNumber, UnityEngine.Object context, Action<string, UnityEngine.Object> debug)
        {
            if (mode == null)
                mode = traceMode;
            if (mode == TraceMode.Hidden)
                return;
            StringBuilder stringBuilder = new StringBuilder("<i><color=green><size=10> StackTrace info of last log. ");

            stringBuilder
                .Append(memberName)
                .Append(" (at ")
                .Append("Assets");

            foreach (string str in sourceFilePath.Split(new string[] { "Assets" }, StringSplitOptions.None).Skip(1))
            {
                stringBuilder.Append(str);
            }

            stringBuilder
                .Append(':')
                .Append(sourceLineNumber)
                .Append(")");

            if (mode == TraceMode.FullStackTrace)
                stringBuilder
                    .Append("\n")
                    .Append(new StackTrace().ToString().Split(new string[] { "\n" }, StringSplitOptions.None).Last());

            debug(stringBuilder.ToString() + "</size></color></i>", context);
        }

        /// <summary>
        /// Print to console all <paramref name="objects"/> as strings separated by ', ', preceded by message.
        /// </summary>
        /// <param name="message">A message to print in console. It will be treated as an additional object.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="traceMode">Tracing mode showed in console. If <see langword="null"/> it will use class <see cref="traceMode"/>.</param>
        /// <param name="memberName">Do not complete.</param>
        /// <param name="sourceFilePath">Do not complete.</param>
        /// <param name="sourceLineNumber">Do not complete.</param>
        /// <param name="objects">Objects to print in console.</param>
        public static void Log(string message = null, UnityEngine.Object context = null, TraceMode? traceMode = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,
            params object[] objects)
        {
            Debug.Log(GetStrings(message, objects), context);
            MethodInformation(traceMode, memberName, sourceFilePath, sourceLineNumber, context, Debug.Log);
        }

        /// <summary>
        /// Print to console all <paramref name="objects"/> as strings separated by ', ', preceded by message.
        /// </summary>
        /// <param name="message">A message to print in console. It will be treated as an additional object.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="traceMode">Tracing mode showed in console. If <see langword="null"/> it will use class <see cref="traceMode"/>.</param>
        /// <param name="memberName">Do not complete.</param>
        /// <param name="sourceFilePath">Do not complete.</param>
        /// <param name="sourceLineNumber">Do not complete.</param>
        /// <param name="objects">Objects to print in console.</param>
        public static void LogError(string message = null, UnityEngine.Object context = null, TraceMode? traceMode = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,
            params object[] objects)
        {
            Debug.Log(GetStrings(message, objects), context);
            MethodInformation(traceMode, memberName, sourceFilePath, sourceLineNumber, context, Debug.LogError);
        }

        /// <summary>
        /// Print to console all <paramref name="objects"/> as strings separated by ', ', preceded by message.
        /// </summary>
        /// <param name="message">A message to print in console. It will be treated as an additional object.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="traceMode">Tracing mode showed in console. If <see langword="null"/> it will use class <see cref="traceMode"/>.</param>
        /// <param name="memberName">Do not complete.</param>
        /// <param name="sourceFilePath">Do not complete.</param>
        /// <param name="sourceLineNumber">Do not complete.</param>
        /// <param name="objects">Objects to print in console.</param>
        public static void LogWarning(string message = null, UnityEngine.Object context = null, TraceMode? traceMode = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,
            params object[] objects)
        {
            Debug.Log(GetStrings(message, objects), context);
            MethodInformation(traceMode, memberName, sourceFilePath, sourceLineNumber, context, Debug.LogWarning);
        }

        /// <summary>
        /// Return an string with all objects as strings separated by ', '.<br>
        /// <see langword="null"/> are turned into "null".
        /// </summary>
        /// <param name="objects">Objects to join as string.</param>
        public static string GetStrings(params object[] objects) => string.Join(", ", objects.Select(e => e == null ? "null" : e.ToString()));

        private static string GetStrings(string message, object[] objects) => (message == null ? "" : message + ", ") + GetStrings(objects);

        /// <summary>
        /// Print to console all <paramref name="enumerable"/> as strings separated by , preceded by message.
        /// </summary>
        /// <param name="enumerable">Enumerable to print in console.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="compact">Whenever it use a single log or several ones, one per line.</param>
        /// <param name="traceMode">Tracing mode showed in console. If <see langword="null"/> it will use class <see cref="traceMode"/>.</param>
        /// <param name="memberName">Do not complete.</param>
        /// <param name="sourceFilePath">Do not complete.</param>
        /// <param name="sourceLineNumber">Do not complete.</param>
        public static void LogLines(IEnumerable<object> enumerable, UnityEngine.Object context = null, bool compact = false, TraceMode? traceMode = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (compact)
                Debug.Log(string.Join("\n", enumerable), context);
            else
            {
                foreach (object item in enumerable)
                {
                    Debug.Log(item, context);
                }
            }

            MethodInformation(traceMode, memberName, sourceFilePath, sourceLineNumber, context, Debug.Log);
        }

        /// <summary>
        /// Print to console all <paramref name="enumerable"/> as strings separated by , preceded by message.
        /// </summary>
        /// <param name="enumerable">Enumerable to print in console.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="compact">Whenever it use a single log or several ones, one per line.</param>
        /// <param name="traceMode">Tracing mode showed in console. If <see langword="null"/> it will use class <see cref="traceMode"/>.</param>
        /// <param name="memberName">Do not complete.</param>
        /// <param name="sourceFilePath">Do not complete.</param>
        /// <param name="sourceLineNumber">Do not complete.</param>
        public static void LogWarningLines(IEnumerable<object> enumerable, UnityEngine.Object context = null, bool compact = false, TraceMode? traceMode = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (compact)
                Debug.LogWarning(string.Join("\n", enumerable), context);
            else
            {
                foreach (object item in enumerable)
                {
                    Debug.LogWarning(item, context);
                }
            }

            MethodInformation(traceMode, memberName, sourceFilePath, sourceLineNumber, context, Debug.LogWarning);
        }

        /// <summary>
        /// Print to console all <paramref name="enumerable"/> as strings separated by , preceded by message.
        /// </summary>
        /// <param name="enumerable">Enumerable to print in console.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="compact">Whenever it use a single log or several ones, one per line.</param>
        /// <param name="traceMode">Tracing mode showed in console. If <see langword="null"/> it will use class <see cref="traceMode"/>.</param>
        /// <param name="memberName">Do not complete.</param>
        /// <param name="sourceFilePath">Do not complete.</param>
        /// <param name="sourceLineNumber">Do not complete.</param>
        public static void LogErrorLines(IEnumerable<object> enumerable, UnityEngine.Object context = null, bool compact = false, TraceMode? traceMode = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (compact)
                Debug.LogError(string.Join("\n", enumerable), context);
            else
            {
                foreach (object item in enumerable)
                {
                    Debug.LogError(item, context);
                }
            }

            MethodInformation(traceMode, memberName, sourceFilePath, sourceLineNumber, context, Debug.LogError);
        }
    }
}
#endif