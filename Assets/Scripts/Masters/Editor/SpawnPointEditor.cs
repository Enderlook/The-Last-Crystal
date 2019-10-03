using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnPointEditor : Editor
{
    private Spawner spawner;
    private Point point;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Create New Points"))
        {
            Undo.RecordObject(spawner, "Create new points");
            spawner.CreatePoint();
            point = spawner.point;
        }

        if (EditorGUI.EndChangeCheck())
            SceneView.RepaintAll();
    }

    private void OnSceneGUI()
    {
        Draw();
        Input();
    }

    private void Draw()
    {
        Handles.color = Color.red;
        for (int i = 0; i < point.NumPoints; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(point[i], Quaternion.identity, .1f, Vector2.zero,
                Handles.CylinderHandleCap);
            if (point[i] != newPos)
            {
                Undo.RecordObject(spawner, "Move point");
                point.MovePoints(i, newPos);
            }
        }
    }

    private void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(spawner, "Add point");
            point.AddPoint(mousePos);
        }
    }

    private void OnEnable()
    {
        spawner = (Spawner)target;

        if (spawner.point == null)
            spawner.CreatePoint();

        point = spawner.point;
    }
}
