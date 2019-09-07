using System;

[AttributeUsage(AttributeTargets.Field)]
public class HasConfirmationFieldAttribute : Attribute
{
    public string confirmFieldName;

    public HasConfirmationFieldAttribute(string confirmFieldName) => this.confirmFieldName = confirmFieldName;
}