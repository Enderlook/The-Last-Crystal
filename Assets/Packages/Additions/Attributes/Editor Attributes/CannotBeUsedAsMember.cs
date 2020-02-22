using System;

namespace Additions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CannotBeUsedAsMemberAttribute : Attribute { }
}