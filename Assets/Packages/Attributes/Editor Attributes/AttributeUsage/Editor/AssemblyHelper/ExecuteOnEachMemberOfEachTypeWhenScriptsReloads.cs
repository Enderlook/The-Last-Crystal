using System;
using AdditionalAttributes.Internal;
using AdditionalAttributes.Internal.Testing;

namespace AdditionalAttributes
{

    /// <summary>
    /// Executes the method decorated by this attribute for each member on each <see cref="Type"/> compiled by Unity each time Unity compiles code.<br>
    /// The method to decorate must have the signature DoSomething(<see cref="Sytem.Reflection.MemberInfo"/>).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class ExecuteOnEachMemberOfEachTypeWhenScriptsReloads : ExecuteOnEachWhenScriptsReloads
    {
        /// <summary>
        /// Executes the method decorated by this attribute for each member on each <see cref="Type"/> compiled by Unity.<br>
        /// The method to decorate must have the signature DoSomething(<see cref="Sytem.Reflection.MemberInfo"/>).
        /// </summary>
        /// <param name="loop">In which loop of the execution will this script execute.</param>
        public ExecuteOnEachMemberOfEachTypeWhenScriptsReloads(int loop = 0) : base(loop) { }
    }
}