using System;

namespace Additions.Attributes.AttributeUsage
{
    [AttributeUsageRequireDataType(typeof(Attribute), typeFlags = TypeCasting.CheckSubclassTypes)]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageFieldMustBeSerializableByUnity : Attribute { }
}