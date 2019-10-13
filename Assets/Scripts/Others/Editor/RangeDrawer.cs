using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RangeFloat)), CustomPropertyDrawer(typeof(RangeInt))]
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
        minProperty = property.FindPropertyRelative(MIN_FIELD_NAME);
        maxProperty = property.FindPropertyRelative(MAX_FIELD_NAME);
        rectBuilder = new HorizontalRectBuilder(position.position, position.width, position.height - errorHeight);

        Rect mainLabelRect = rectBuilder.GetRect(EditorGUIUtility.labelWidth);

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.LabelField(mainLabelRect, label);

        foreach ((SerializedProperty serializedProperty, Rect fieldRect) in GetFields())
        {
            GUIContent guiContent = new GUIContent(serializedProperty.displayName, property.tooltip);
            EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(guiContent).x; // Reduce size of label to only contain the necessary space to display the name
            EditorGUI.PropertyField(fieldRect, serializedProperty);
            EditorGUIUtility.labelWidth = 0; // Using 0 return it value to default
        }

        Validate(position);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Debug.Log(errors.Count);
        return EditorGUI.GetPropertyHeight(property, label) + errorHeight;
    }

    protected virtual (SerializedProperty serializedProperty, Rect fieldRect)[] GetFields()
    {
        Rect minRect = rectBuilder.GetRect(rectBuilder.RemainingWidth / 2);
        Rect maxRect = rectBuilder.GetRect(rectBuilder.RemainingWidth);

        return new (SerializedProperty serializedProperty, Rect fieldRect)[]
        {
            (minProperty, minRect),
            (maxProperty, maxRect)
        };
    }

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
}

[CustomPropertyDrawer(typeof(RangeFloatStep)), CustomPropertyDrawer(typeof(RangeIntStep))]
public class FloatRangeStepDrawer : FloatRangeDrawer
{
    private const string STEP_FIELD_NAME = "step";
    private SerializedProperty stepProperty;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        stepProperty = property.FindPropertyRelative(STEP_FIELD_NAME);
        base.OnGUI(position, property, label);
    }

    protected override (SerializedProperty serializedProperty, Rect fieldRect)[] GetFields()
    {
        float fieldWidth = rectBuilder.RemainingWidth / 3;
        Rect minRect = rectBuilder.GetRect(fieldWidth);
        Rect maxRect = rectBuilder.GetRect(fieldWidth);
        Rect stepRect = rectBuilder.GetRect(fieldWidth);

        return new (SerializedProperty serializedProperty, Rect fieldRect)[]
        {
            (minProperty, minRect),
            (maxProperty, maxRect),
            (stepProperty, stepRect)
        };
    }

    protected override void FindErrors(Rect position)
    {
        if (stepProperty.floatValue > (maxProperty.floatValue - minProperty.floatValue))
            errors.Add($"Value of {stepProperty.displayName} can't be higher than the difference between {minProperty.displayName} and {maxProperty.displayName}.");
        base.FindErrors(position);
    }
}