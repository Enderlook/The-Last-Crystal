using System;

namespace AdditionalAttributes.PostCompiling
{
    [AttributeUsage(AttributeTargets.All)]
    public class DoNotInspectAttribute : Attribute { }
}