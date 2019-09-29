using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public sealed class DrawVectorRelativeToTransform : Attribute
{
    public string Icon { get; private set; }

    public DrawVectorRelativeToTransform() { }
    public DrawVectorRelativeToTransform(string icon) => Icon = icon;
}
