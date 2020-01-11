using AdditionalExceptions;

using AdditionalExtensions;

using System;
using System.Reflection;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(DrawVectorRelativeToTransformAttribute)), InitializeOnLoad]
    internal class DrawVectorRelativeToTransformEditor : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }

        static DrawVectorRelativeToTransformEditor() => SceneView.onSceneGUIDelegate += RenderSceneGUI;

        private static Vector3 DrawHandle(Vector3 position, bool usePositionHandle)
        {
            return usePositionHandle
                ? Handles.PositionHandle(position, Quaternion.identity)
                : Handles.FreeMoveHandle(position, Quaternion.identity, HandleUtility.GetHandleSize(position) * .1f, Vector2.one, Handles.CylinderHandleCap);
        }

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static Vector3 GetPositionByTransform(SerializedObject serializedObject) => ((Component)serializedObject.targetObject).transform.position;

        private static readonly string vectorTypes = $"{nameof(Vector3)}, {nameof(Vector3Int)}, {nameof(Vector2)}, {nameof(Vector2Int)}, {nameof(Vector4)}";
        private static void DisplayErrorReference(string name) => throw new Exception($"The serialized property reference {name} isn't neither {vectorTypes} nor {nameof(Transform)}.");

        private static Vector3 GetVector3ValueOf(SerializedProperty serializedProperty)
        {
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Vector3:
                    return serializedProperty.vector3Value;
                case SerializedPropertyType.Vector3Int:
                    return serializedProperty.vector3IntValue;
                case SerializedPropertyType.Vector2:
                    return serializedProperty.vector2Value;
                case SerializedPropertyType.Vector2Int:
                    return (Vector2)serializedProperty.vector2IntValue;
                case SerializedPropertyType.Vector4:
                    return serializedProperty.vector4Value;
                case SerializedPropertyType.ObjectReference:
                    if (serializedProperty.objectReferenceValue is Transform transform)
                        return transform.position;
                    DisplayErrorReference(serializedProperty.name);
                    break;
                default:
                    DisplayErrorReference(serializedProperty.name);
                    break;
            }
            throw new ImpossibleStateException();
        }

        private static Vector3 CastToVector3(object source, Type sourceType)
        {
            if (sourceType == typeof(Vector3))
                return (Vector3)source;
            if (sourceType == typeof(Vector3Int))
                return (Vector3Int)source;
            if (sourceType == typeof(Vector2))
                return (Vector2)source;
            if (sourceType == typeof(Vector2Int))
                return (Vector2)(Vector2Int)source;
            if (sourceType == typeof(Vector4))
                return (Vector4)source;
            // If everything fails, perform a last 99% error-warranted salvage cast
            return (Vector3)source;
        }

        private static Vector3 GetReference(SerializedObject serializedObject, string referenceName)
        {
            if (string.IsNullOrEmpty(referenceName))
                return GetPositionByTransform(serializedObject);

            SerializedProperty referenceProperty = serializedObject.FindProperty(referenceName);
            if (referenceProperty == null)
            {
                UnityEngine.Object targetObject = serializedObject.targetObject;
                Type type = targetObject.GetType();

                FieldInfo fieldInfo = type.GetField(referenceName, bindingFlags);
                if (fieldInfo == null)
                {
                    PropertyInfo propertyInfo = type.GetProperty(referenceName, bindingFlags);
                    if (propertyInfo == null)
                    {
                        MethodInfo methodInfo = type.GetMethod(referenceName, bindingFlags);
                        if (methodInfo != null)
                            // Get reference by method
                            return CastToVector3(methodInfo.Invoke(targetObject, Array.Empty<object>()), methodInfo.ReturnType);
                        // If everything fails, get by transform
                        return GetPositionByTransform(serializedObject);
                    }
                    else
                        // Get reference by property
                        return CastToVector3(propertyInfo.GetValue(targetObject), propertyInfo.PropertyType);
                }
                else
                    // Get reference by field
                    return CastToVector3(fieldInfo.GetValue(targetObject), fieldInfo.FieldType);
            }
            else
                // Get reference by serialized field
                return GetVector3ValueOf(referenceProperty);
        }

        private static void RenderSingleSerializedProperty(SerializedProperty serializedProperty, DrawVectorRelativeToTransformAttribute drawVectorRelativeToTransform, Vector3 reference)
        {
            Vector3 position;
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    position = serializedProperty.vector2Value = DrawHandle(serializedProperty.vector2Value + (Vector2)reference, drawVectorRelativeToTransform.usePositionHandler) - reference;
                    break;
                case SerializedPropertyType.Vector2Int:
                    serializedProperty.vector2IntValue = DrawHandle((Vector2)(serializedProperty.vector2IntValue + reference.ToVector2Int()), drawVectorRelativeToTransform.usePositionHandler).ToVector2Int() - reference.ToVector2Int();
                    position = (Vector2)serializedProperty.vector2IntValue;
                    break;
                case SerializedPropertyType.Vector3:
                    position = serializedProperty.vector3Value = DrawHandle(serializedProperty.vector3Value + reference, drawVectorRelativeToTransform.usePositionHandler) - reference;
                    break;
                case SerializedPropertyType.Vector3Int:
                    position = serializedProperty.vector3IntValue = DrawHandle(serializedProperty.vector3IntValue + reference.ToVector3Int(), drawVectorRelativeToTransform.usePositionHandler).ToVector3Int() - reference.ToVector3Int();
                    break;
                default:
                    Debug.LogError($"The attribute {nameof(DrawVectorRelativeToTransformAttribute)} is only allowed in types of {nameof(Vector2)}, {nameof(Vector2Int)}, {nameof(Vector3)} and {nameof(Vector3Int)}.");
                    return;
            }
            if (!string.IsNullOrEmpty(drawVectorRelativeToTransform.icon))
                Handles.Label(position, (Texture2D)EditorGUIUtility.Load(drawVectorRelativeToTransform.icon));

        }

        private static void RenderSceneGUI(SceneView sceneview)
        {
            foreach ((SerializedProperty serializedProperty, object field, DrawVectorRelativeToTransformAttribute drawVectorRelativeToTransform, Editor editor) in PropertyDrawerHelper.FindAllSerializePropertiesInActiveEditorWithTheAttribute<DrawVectorRelativeToTransformAttribute>())
            {
                serializedProperty.serializedObject.Update();
                Vector3 reference = GetReference(serializedProperty.serializedObject, drawVectorRelativeToTransform.reference);

                if (serializedProperty.isArray)
                {
                    for (int i = 0; i < serializedProperty.arraySize; i++)
                    {
                        SerializedProperty item = serializedProperty.GetArrayElementAtIndex(i);
                        RenderSingleSerializedProperty(item, drawVectorRelativeToTransform, reference);
                    }
                }
                else
                    RenderSingleSerializedProperty(serializedProperty, drawVectorRelativeToTransform, reference);

                serializedProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}