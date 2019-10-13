using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    private const string MIN_FIELD_NAME = "min";
    private const string MAX_FIELD_NAME = "max";
    private const float HELP_BOX_HEIGHT_MULTIPLIER = 1.5f;

    private static float HelpBoxHeight => EditorGUIUtility.singleLineHeight * HELP_BOX_HEIGHT_MULTIPLIER;

    private bool warning = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty minProperty = property.FindPropertyRelative(MIN_FIELD_NAME);
        SerializedProperty maxProperty = property.FindPropertyRelative(MAX_FIELD_NAME);

        HorizontalRectBuilder rectBuilder = new HorizontalRectBuilder(position.position, position.width, position.height - (warning ? HelpBoxHeight : 0));

        Rect labelRect = rectBuilder.GetRect(EditorGUIUtility.labelWidth);
        GUIContent minRectGUIContent = new GUIContent(minProperty.displayName, minProperty.tooltip);
        float minRectLabelWidth = GUI.skin.label.CalcSize(minRectGUIContent).x;
        GUIContent maxRectGUIContent = new GUIContent(maxProperty.displayName, maxProperty.tooltip);
        float maxRectLabelWidth = GUI.skin.label.CalcSize(maxRectGUIContent).x;
        Rect minRectLabel = rectBuilder.GetRect(minRectLabelWidth);
        Rect minRect = rectBuilder.GetRect((rectBuilder.RemainingWidth - maxRectLabelWidth) / 2);
        Rect maxRectLabel = rectBuilder.GetRect(maxRectLabelWidth);
        Rect maxRect = rectBuilder.GetRect(rectBuilder.RemainingWidth);

        EditorGUI.LabelField(labelRect, label);

        EditorGUI.LabelField(minRectLabel, minRectGUIContent);
        minProperty.floatValue = EditorGUI.FloatField(minRect, new GUIContent("", minProperty.tooltip), minProperty.floatValue);
        EditorGUI.LabelField(maxRectLabel, maxRectGUIContent);
        maxProperty.floatValue = EditorGUI.FloatField(maxRect, new GUIContent("", maxProperty.tooltip), maxProperty.floatValue);

        if (warning = minProperty.floatValue >= maxProperty.floatValue)
        {
            string message = $"Value of {minProperty.displayName} can't be higher or equal to {maxProperty.displayName}.";
            Debug.LogWarning(message);
            EditorGUI.HelpBox(new Rect(position.x, position.y + rectBuilder.BaseSize.y, position.width, HelpBoxHeight), message, MessageType.Error);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label) + (warning ? HelpBoxHeight : 0);
}
