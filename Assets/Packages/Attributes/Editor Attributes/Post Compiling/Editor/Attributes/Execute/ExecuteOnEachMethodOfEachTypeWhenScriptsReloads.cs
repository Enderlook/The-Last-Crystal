using System;
using System.Reflection;
using AdditionalAttributes.AttributeUsage;
using AdditionalAttributes.PostCompiling.Internal;

namespace AdditionalAttributes.PostCompiling
{
    /// <summary>
    /// Executes the method decorated by this attribute for each method on each <see cref="Type"/> compiled by Unity each time Unity compiles code.<br>
    /// The method to decorate must have the signature DoSomething(<see cref="Sytem.Reflection.MethodInfo"/>).
    /// </summary>
    [AttributeUsageAccessibility(BindingFlags.Static | BindingFlags.NonPublic)]
    [AttributeUsageMethod(1, typeof(MethodInfo))]
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class ExecuteOnEachMethodOfEachTypeWhenScriptsReloads : BaseExecuteWhenScriptsReloads
    {
        /// <summary>
        /// Executes the method decorated by this attribute for each method on each <see cref="Type"/> compiled by Unity.<br>
        /// The method to decorate must have the signature DoSomething(<see cref="Sytem.Reflection.MethodInfo"/>).
        /// </summary>
        /// <param name="loop">In which loop of the execution will this script execute.</param>
        public ExecuteOnEachMethodOfEachTypeWhenScriptsReloads(int loop = 0) : base(loop) { }
    }
}