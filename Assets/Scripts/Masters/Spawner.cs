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

    private int enemiesInGame = 0;

    [DrawVectorRelativeToTransform]
    public List<Vector2> points;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        if (enemiesInGame == count) yield break;

        yield return new WaitForSeconds(startSpawn);
        for (int n = 0; n < count; n++)
        {
            yield return new WaitUntil(() => Menu.IsPause);

            int p = Random.Range(0, points.Count);
            int x = Random.Range(0, enemies.Length);

            Instantiate(enemies[x], points[p] + (Vector2)transform.position, Quaternion.identity);
            enemiesInGame++;
            yield return new WaitForSeconds(timeBtwSpawn);
        }
    }
}
