using UnityEditor;
using UnityEngine;

namespace Navigation.UnityInspector
{
    [CustomEditor(typeof(Navigation))]
    public class NavigationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Navigation navigation = (Navigation)target;

            if (GUILayout.Button("Regenerate Grid"))
                navigation.GenerateGrid();
        }
    }
}