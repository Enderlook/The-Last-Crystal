using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnPointEditor : Editor
{
    Spawner spawner;
    Point point;

    void OnSceneGUI()
    {
        Draw();
    }

    void Draw()
    {
        Handles.color = Color.red;
        for (int i = 0; i < point.NumPoints; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(point[i], Quaternion.identity, .1f, Vector2.zero,
                Handles.CylinderHandleCap);
            if (point[i] != newPos)
            {
                Undo.RecordObject(spawner, "MovePoint");
                point.MovePoints(i, newPos);
            }
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
