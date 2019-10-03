using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class DrawVectorRelativeToTransformAttribute : Attribute
{
    /// <summary>
    /// Whenever it should use <see cref="UnityEditor.Handles.PositionHandle(UnityEngine.Vector3, UnityEngine.Quaternion)"/> or <see cref="UnityEditor.Handles.FreeMoveHandle(UnityEngine.Vector3, UnityEngine.Quaternion, float, UnityEngine.Vector3, UnityEditor.Handles.CapFunction)"/> to draw the handler.
    /// </summary>
    public bool UsePositionHandler { get; private set; }

    /// <summary>
    /// Icon displayed in scene. If empty no icon will be displayed.
    /// </summary>
    public string Icon { get; private set; }

    public DrawVectorRelativeToTransformAttribute(bool usePositionHandler = false) => UsePositionHandler = usePositionHandler;
    public DrawVectorRelativeToTransformAttribute(string icon, bool usePositionHandler = false)
    {
        UsePositionHandler = usePositionHandler;
        Icon = icon;
    }
}
