using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class HasConfirmationFieldAttribute : Attribute
{
    public string ConfirmFieldName { get; private set; }

    public HasConfirmationFieldAttribute(string confirmFieldName) => ConfirmFieldName = confirmFieldName;
}