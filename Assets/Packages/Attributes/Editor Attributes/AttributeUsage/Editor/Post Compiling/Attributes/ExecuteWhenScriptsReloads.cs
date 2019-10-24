using AdditionalAttributes.PostCompiling.Execute.Internal;

namespace AdditionalAttributes.PostCompiling.Execute
{
    /// <summary>
    /// Executes the method decorated by this attribute each time Unity compiles code.<br>
    /// The method to decorate must have the signature DoSomething().
    /// </summary>
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