using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    

    [Header("Setup")]
    [Tooltip("Enemies to spawn.")]
    public GameObject[] enemies;
    [Tooltip("Count of enemies to spawn.")]
    public int count;
    [Tooltip("Time between spawn enemies.")]
    public float timeBtwSpawn;
    [Tooltip("Start time spawn")]
    public float startSpawn;

    [HideInInspector]
    public Point point;

    private int enemiesInGame = 0;
    private Vector2[] points;

    public void CreatePoint()
    {
        point = new Point(transform.position);
    }

    private void Start()
    {
        points = point.GetPositionsPoints();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        if (enemiesInGame == count) yield break;

        yield return new WaitForSeconds(startSpawn);
        for (int n = 0; n < count; n++)
        {
            int p = Random.Range(0, points.Length);
            int x = Random.Range(0, enemies.Length);

            Instantiate(enemies[x], points[p], Quaternion.identity);
            enemiesInGame++;
            yield return new WaitForSeconds(timeBtwSpawn);
        }
    }
}

[System.Serializable]
public class Point
{
    [SerializeField, HideInInspector]
    List<Vector2> points;

    public Point(Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre + Vector2.left,
            centre + Vector2.right
        };
    }

    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public void MovePoints(int i, Vector2 pos)
    {
        points[i] = pos;
    }

    public Vector2 GetPosition(int i)
    {
        return new Vector2(points[i].x, points[i].y);
    }

    public Vector2[] GetPositionsPoints()
    {
        return points.ToArray();
    }
}
