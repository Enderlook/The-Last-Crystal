using System;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsageRequireDataType(typeof(Vector2), typeof(Vector2Int), typeof(Vector3), typeof(Vector3), typeof(Vector4), typeof(Transform))]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DrawVectorRelativeToTransformAttribute : Attribute
    {
        /// <summary>
        /// Whenever it should use <see cref="UnityEditor.Handles.PositionHandle(UnityEngine.Vector3, UnityEngine.Quaternion)"/> or <see cref="UnityEditor.Handles.FreeMoveHandle(UnityEngine.Vector3, UnityEngine.Quaternion, float, UnityEngine.Vector3, UnityEditor.Handles.CapFunction)"/> to draw the handler.
        /// </summary>
        public bool UsePositionHandler { get; }

        /// <summary>
        /// Icon displayed in scene. If empty no icon will be displayed.
        /// </summary>
        public string Icon { get; }

        /// <summary>
        /// Reference used to show handler. If empty, transform the gameobject will be used.
        /// </summary>
        public string Reference { get; }

        public DrawVectorRelativeToTransformAttribute(bool usePositionHandler = false, string reference = "")
        {
            UsePositionHandler = usePositionHandler;
            Reference = reference;
        }

        public DrawVectorRelativeToTransformAttribute(string icon, bool usePositionHandler = false, string reference = "")
        {
            UsePositionHandler = usePositionHandler;
            Icon = icon;
            Reference = reference;
        }
    }
}