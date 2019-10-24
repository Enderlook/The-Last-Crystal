using System;
using System.Reflection;
using AdditionalAttributes.AttributeUsage;

namespace AdditionalAttributes.PostCompiling.Internal
{
    [AttributeUsageAccessibility(BindingFlags.Static | BindingFlags.NonPublic)]
    public abstract class ExecuteOnEachWhenScriptsReloads : Attribute
    {
        /// <summary>
        /// In which loop of the execution will this script executed.<br>
        /// Accept any kind of number.
        /// </summary>
        public int Loop { get; private set; }

        protected ExecuteOnEachWhenScriptsReloads(int loop = 0) => Loop = loop;
    }
}