using System;
using System.Reflection;
using AdditionalAttributes.PostCompiling.Execute.Internal;

namespace AdditionalAttributes.PostCompiling.Execute
{
    /// <summary>
    /// Executes the method decorated by this attribute for each property on each <see cref="Type"/> compiled by Unity each time Unity compiles code.<br>
    /// The method to decorate must have the signature DoSomething(<see cref="Sytem.Reflection.PropertyInfo"/>).
    /// </summary>
    [AttributeUsageMethod(1, typeof(PropertyInfo))]
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class ExecuteOnEachPropertyOfEachTypeWhenScriptsReloads : ExecuteOnEachWhenScriptsReloads
    {
        /// <summary>
        /// Executes the method decorated by this attribute for each property on each <see cref="Type"/> compiled by Unity.<br>
        /// The method to decorate must have the signature DoSomething(<see cref="Sytem.Reflection.PropertyInfo"/>).
        /// </summary>
        /// <param name="loop">In which loop of the execution will this script execute.</param>
        public ExecuteOnEachPropertyOfEachTypeWhenScriptsReloads(int loop = 0) : base(loop) { }
    }
}