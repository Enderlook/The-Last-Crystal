using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class DrawVectorRelativeToTransformAttribute : Attribute
{
    public string Icon { get; private set; }

    public DrawVectorRelativeToTransformAttribute() { }
    public DrawVectorRelativeToTransformAttribute(string icon) => Icon = icon;
}
