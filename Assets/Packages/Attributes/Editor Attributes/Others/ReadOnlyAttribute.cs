using System;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public sealed class ReadOnlyAttribute : PropertyAttribute { }
}