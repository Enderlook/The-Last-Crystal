using Additions.Utils;

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnPointEditor : Editor
{
    private static List<Vector2> points;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("Use Ctrl + Right Click to add points.\n" +
                                "Use Alt + Right Click to remove closest point to mouse.", MessageType.Info);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
    private void OnEnable() => points = ((Spawner)target).points;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "It's an instance method made by Unity.")]
    public void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            Vector2 mousePosition = MouseHelper.GetMouseWorldPositionInEditor();
            if (Event.current.control)
                points.Add(mousePosition);
            else if (Event.current.alt)
            {
                float closestDistance = Mathf.Infinity;
                int? closestPoint = null;
                for (int i = 0; i < points.Count; i++)
                {
                    float newDistance = Vector2.Distance(points[i], mousePosition);
                    if (newDistance < closestDistance)
                    {
                        closestDistance = newDistance;
                        closestPoint = i;
                    }
                }
                if (closestPoint != null)
                    points.RemoveAt((int)closestPoint);
            }
        }
    }
}
