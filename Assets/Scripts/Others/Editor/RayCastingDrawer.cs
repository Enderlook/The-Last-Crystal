using System;
using System.Reflection;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(RayCasting)), InitializeOnLoad]
public class RayCastingDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property);

    static RayCastingDrawer() => SceneView.onSceneGUIDelegate += RenderSceneGUI;

    private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    private static void RenderSceneGUI(SceneView sceneview)
    {
        foreach (Editor editor in ActiveEditorTracker.sharedTracker.activeEditors)
        {
            SerializedProperty serializedProperty = editor.serializedObject.GetIterator();
            while (serializedProperty.Next(true))
            {
                UnityEngine.Object targetObject = serializedProperty.serializedObject.targetObject;
                // Used to skip missing components
                if (targetObject == null)
                    continue;
                Type targetObjectClassType = targetObject.GetType();
                FieldInfo field = targetObjectClassType.GetField(serializedProperty.propertyPath, bindingFlags);
                // If the field exist, it's the class type we want, and has draw enabled
                if (field != null && field.FieldType == typeof(RayCasting) && serializedProperty.FindPropertyRelative("draw").boolValue)
                {
                    Color color = serializedProperty.FindPropertyRelative("color").colorValue;

                    SerializedProperty sourceProperty = serializedProperty.FindPropertyRelative("source");
                    Vector2 source = sourceProperty.vector2Value;

                    RayCasting rayCasting = (RayCasting)field.GetValue(targetObject);
                    Vector2 reference = (Vector2)typeof(RayCasting).GetProperty("Reference", bindingFlags).GetValue(rayCasting);

                    serializedProperty.serializedObject.Update();

                    sourceProperty.vector2Value = (Vector2)Handles.PositionHandle(source + reference, Quaternion.identity) - reference;

                    Handles.color = color;
                    if (serializedProperty.FindPropertyRelative("distance").floatValue == Mathf.Infinity)
                    {
                        SerializedProperty directionProperty = serializedProperty.FindPropertyRelative("direction");
                        Vector2 direction = directionProperty.vector2Value;

                        // We can't use the WorldEnd property because that will give infinity
                        Vector2 worldEndPosition = source + reference + direction;

                        directionProperty.vector2Value = ((Vector2)Handles.PositionHandle(worldEndPosition, Quaternion.identity) - source - reference).normalized;

                        Vector2 sourcePositionWorld = source + reference;

                        Handles.DrawLine(sourcePositionWorld, worldEndPosition);
                        Handles.DrawDottedLine(sourcePositionWorld, worldEndPosition + direction, 1);
                    }
                    else
                    {
                        PropertyInfo worldEndProperty = typeof(RayCasting).GetProperty("WorldEnd", bindingFlags);
                        Vector2 worldEndPosition = (Vector2)worldEndProperty.GetValue(rayCasting);
                        worldEndProperty.SetValue(rayCasting, (Vector2)Handles.PositionHandle(worldEndPosition, Quaternion.identity));
                        Handles.DrawLine(reference + source, worldEndPosition);
                    }

                    rayCasting.DrawLine();

                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
