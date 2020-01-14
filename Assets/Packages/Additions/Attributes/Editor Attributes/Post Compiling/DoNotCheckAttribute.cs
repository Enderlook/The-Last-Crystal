using System;

namespace Additions.Attributes.PostCompiling.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class DoNotCheckAttribute : Attribute
    {
        public readonly Type[] ignoreTypes;

        public DoNotCheckAttribute(params Type[] attributesToNotCheck) => ignoreTypes = attributesToNotCheck;
    }
}