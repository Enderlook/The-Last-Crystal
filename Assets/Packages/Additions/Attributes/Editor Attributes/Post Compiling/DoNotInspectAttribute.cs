using System;

namespace Additions.Attributes.PostCompiling.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class DoNotInspectAttribute : Attribute { }
}