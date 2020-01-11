using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public sealed class ReadOnlyAttribute : PropertyAttribute { }
}