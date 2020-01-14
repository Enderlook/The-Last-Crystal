using System;

namespace Additions.Attributes.AttributeUsage
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class DoNotInspectAttribute : Attribute { }
}