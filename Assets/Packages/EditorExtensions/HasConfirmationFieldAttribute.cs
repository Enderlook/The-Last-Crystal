using System;
using System.Collections.Generic;
using System.Reflection;

[AttributeUsage(AttributeTargets.Field)]
public sealed class HasConfirmationFieldAttribute : Attribute
{
    public string ConfirmFieldName { get; private set; }

    public HasConfirmationFieldAttribute(string confirmFieldName) => ConfirmFieldName = confirmFieldName;

    public static IEnumerable<string> GetFieldsWithConfirmationAttribute<T>(T instance, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    {
        Type type = instance.GetType();
        foreach (FieldInfo field in type.GetFields(bindingFlags))
        {
            if (field.IsDefined(typeof(HasConfirmationFieldAttribute), true))
            {
                yield return field.Name;
            }
        }
    }
}