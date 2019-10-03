using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DrawVectorRelativeToTransformAttribute)), InitializeOnLoad]
public class DrawVectorRelativeToTransformEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
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

    private static void RenderSceneGUI(SceneView sceneview)
    {
        foreach ((SerializedProperty serializedProperty, object field, DrawVectorRelativeToTransformAttribute drawVectorRelativeToTransform, Editor editor) in PropertyDrawerHelper.FindAllSerializePropertiesInActiveEditorWithTheAttribute<DrawVectorRelativeToTransformAttribute>())
        {
            serializedProperty.serializedObject.Update();
            Transform transform = ((Component)serializedProperty.serializedObject.targetObject).transform;
            Vector3 position;
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    position = serializedProperty.vector2Value = DrawHandle(serializedProperty.vector2Value + (Vector2)transform.position, drawVectorRelativeToTransform.UsePositionHandler) - transform.position;
                    break;
                case SerializedPropertyType.Vector2Int:
                    serializedProperty.vector2IntValue = VectorExtensions.ToVector2Int(DrawHandle((Vector2)(serializedProperty.vector2IntValue + VectorExtensions.ToVector2Int(transform.position)), drawVectorRelativeToTransform.UsePositionHandler)) - VectorExtensions.ToVector2Int(transform.position);
                    position = (Vector2)serializedProperty.vector2IntValue;
                    break;
                case SerializedPropertyType.Vector3:
                    position = serializedProperty.vector3Value = DrawHandle(serializedProperty.vector3Value + transform.position, drawVectorRelativeToTransform.UsePositionHandler) - transform.position;
                    break;
                case SerializedPropertyType.Vector3Int:
                    position = serializedProperty.vector3IntValue = VectorExtensions.ToVector3Int(DrawHandle(serializedProperty.vector3IntValue + VectorExtensions.ToVector3Int(transform.position), drawVectorRelativeToTransform.UsePositionHandler)) - VectorExtensions.ToVector3Int(transform.position);
                    break;
                default:
                    Debug.LogError($"The attribute {nameof(DrawVectorRelativeToTransformAttribute)} is only allowed in types of {nameof(Vector2)}, {nameof(Vector2Int)}, {nameof(Vector3)} and {nameof(Vector3Int)}.");
                    continue;
            }
            if (!string.IsNullOrEmpty(drawVectorRelativeToTransform.Icon))
                Handles.Label(position, (Texture2D)EditorGUIUtility.Load(drawVectorRelativeToTransform.Icon));
            serializedProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}
