﻿using AdditionalAttributes.AttributeUsage;
using AdditionalAttributes.PostCompiling.Internal;

namespace AdditionalAttributes.PostCompiling
{
    /// <summary>
    /// Executes the method decorated by this attribute each time Unity compiles code.<br>
    /// The method to decorate must have the signature DoSomething().
    /// </summary>
    [AttributeUsageMethod(1, parameterType = AttributeUsageMethodAttribute.ParameterMode.VoidOrNone)]
    public sealed class ExecuteWhenScriptsReloads : ExecuteOnEachWhenScriptsReloads
    {
        /// <summary>
        /// Executes the method decorated by this attribute.<br>
        /// The method to decorate must have the signature DoSomething().
        /// </summary>
        /// <param name="loop">In which loop of the execution will this script execute.</param>
        public ExecuteWhenScriptsReloads(int loop = 0) : base(loop) { }
    }
}