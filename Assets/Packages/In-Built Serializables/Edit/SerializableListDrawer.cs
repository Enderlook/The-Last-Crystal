using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowListAttribute), true)]
public class SerializableListDrawer : PropertyDrawer
{
    private bool foldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RectangleFactory rectangleFactory = new RectangleFactory(position, true);
        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty array = property.FindPropertyRelative("array");

        if (foldout = EditorGUI.Foldout(rectangleFactory.GetRect(), foldout, label, true))
        {
            array.arraySize = EditorGUI.IntField(rectangleFactory.GetRect(), "Size", array.arraySize);

            EditorGUI.indentLevel++;
            for (int i = 0; i < array.arraySize; i++)
            {
                EditorGUI.PropertyField(rectangleFactory.GetRect(), array.GetArrayElementAtIndex(i));
            }
            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty array = property.FindPropertyRelative("array");
        return EditorGUI.GetPropertyHeight(array, true) + (foldout ? (array.arraySize + 1) * EditorGUIUtility.singleLineHeight : 0);
    }

    private class RectangleFactory
    {
        private readonly float x;
        private readonly float y;
        private readonly bool autoIncrement;
        private int amount = 0;

        public RectangleFactory(Rect source, bool autoIncrement)
        {
            x = source.x;
            y = source.y;
            this.autoIncrement = autoIncrement;
        }

        public Rect GetRect() => new Rect(x, y + (autoIncrement ? EditorGUIUtility.singleLineHeight * amount++ : 0), EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);
    }
}
