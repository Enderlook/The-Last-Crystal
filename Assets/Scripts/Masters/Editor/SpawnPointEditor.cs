using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

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

    private void OnEnable() => points = ((Spawner)target).points;

    public void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            if (Event.current.button == 1)
            {
                Vector2 mousePosition = MouseHelper.GetMousePositionInEditor();
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
}
