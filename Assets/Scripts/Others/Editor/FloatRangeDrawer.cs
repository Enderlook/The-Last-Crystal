using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    private const string MIN_FIELD_NAME = "min";
    private const string MAX_FIELD_NAME = "max";

    protected List<string> errors = new List<string>();
    protected SerializedProperty minProperty, maxProperty;
    protected HorizontalRectBuilder rectBuilder;
    private float errorHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FindSerializedProperties(property);
        MakeHorizontalRectBuilder(position);

        Rect mainLabelRect = rectBuilder.GetRect(EditorGUIUtility.labelWidth);

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.LabelField(mainLabelRect, label);

        foreach ((SerializedProperty serializedProperty, Rect fieldRect, Rect labelRect) in GetFields())
        {
            ShowField(serializedProperty, fieldRect, labelRect);
        }

        Validate(position);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Debug.Log(errors.Count);
        return EditorGUI.GetPropertyHeight(property, label) + errorHeight;
    }

    protected virtual (SerializedProperty serializedProperty, Rect fieldRect, Rect labelRect)[] GetFields()
    {
        float minRectLabelWidth = GetWidth(minProperty);
        float maxRectLabelWidth = GetWidth(maxProperty);
        Rect minRectLabel = rectBuilder.GetRect(minRectLabelWidth);
        Rect minRect = rectBuilder.GetRect((rectBuilder.RemainingWidth - maxRectLabelWidth) / 2);
        Rect maxRectLabel = rectBuilder.GetRect(maxRectLabelWidth);
        Rect maxRect = rectBuilder.GetRect(rectBuilder.RemainingWidth);

        return new (SerializedProperty serializedProperty, Rect fieldRect, Rect labelRect)[]
        {
            (minProperty, minRect, minRectLabel),
            (maxProperty, maxRect, maxRectLabel)
        };
    }

    protected void FindSerializedProperties(SerializedProperty property)
    {
        minProperty = property.FindPropertyRelative(MIN_FIELD_NAME);
        maxProperty = property.FindPropertyRelative(MAX_FIELD_NAME);
    }

    protected void MakeHorizontalRectBuilder(Rect position) => rectBuilder = new HorizontalRectBuilder(position.position, position.width, position.height - errorHeight);

    protected void Validate(Rect position)
    {
        errors.Clear();
        FindErrors(position);
    }

    protected virtual void FindErrors(Rect position)
    {
        if (minProperty.floatValue >= maxProperty.floatValue)
            errors.Add($"Value of {minProperty.displayName} can't be higher or equal to {maxProperty.displayName}.");
        if (errors.Count <= 0)
            return;
        foreach (string error in errors)
        {
            Debug.LogWarning(error);
        }
        string message = string.Join("\n", errors);
        errorHeight = GUI.skin.box.CalcHeight(new GUIContent(message), position.width);
        EditorGUI.HelpBox(new Rect(position.x, position.y + rectBuilder.BaseSize.y, position.width, errorHeight), message, MessageType.Error);
    }

    protected float GetWidth(SerializedProperty property)
    {
        GUIContent guiContent = new GUIContent(property.displayName, property.tooltip);
        float width = GUI.skin.label.CalcSize(guiContent).x;
        return width;
    }

    protected static void ShowField(SerializedProperty property, Rect field, Rect label)
    {
        EditorGUI.LabelField(label, new GUIContent(property.displayName, property.tooltip));
        property.floatValue = EditorGUI.FloatField(field, new GUIContent("", property.tooltip), property.floatValue);
    }
}

[CustomPropertyDrawer(typeof(FloatRangeStep))]
public class FloatRangeStepDrawer : FloatRangeDrawer
{
    private const string STEP_FIELD_NAME = "step";
    private SerializedProperty stepProperty;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        stepProperty = property.FindPropertyRelative(STEP_FIELD_NAME);
        base.OnGUI(position, property, label);
    }

    protected override (SerializedProperty serializedProperty, Rect fieldRect, Rect labelRect)[] GetFields()
    {
        float minRectLabelWidth = GetWidth(minProperty);
        float maxRectLabelWidth = GetWidth(maxProperty);
        float stepRectLabelWidth = GetWidth(stepProperty);
        Rect minRectLabel = rectBuilder.GetRect(minRectLabelWidth);
        float fieldWidth = (rectBuilder.RemainingWidth - maxRectLabelWidth - stepRectLabelWidth) / 3;
        Rect minRect = rectBuilder.GetRect(fieldWidth);
        Rect maxRectLabel = rectBuilder.GetRect(maxRectLabelWidth);
        Rect maxRect = rectBuilder.GetRect(fieldWidth);
        Rect stepRectLabel = rectBuilder.GetRect(stepRectLabelWidth);
        Rect stepRect = rectBuilder.GetRect(fieldWidth);

        return new (SerializedProperty serializedProperty, Rect fieldRect, Rect labelRect)[]
        {
            (minProperty, minRect, minRectLabel),
            (maxProperty, maxRect, maxRectLabel),
            (stepProperty, stepRect, stepRectLabel)
        };
    }

    protected override void FindErrors(Rect position)
    {
        if (stepProperty.floatValue > (maxProperty.floatValue - minProperty.floatValue))
            errors.Add($"Value of {stepProperty.displayName} can't be higher than the difference between {minProperty.displayName} and {maxProperty.displayName}.");
        base.FindErrors(position);
    }
}