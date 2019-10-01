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
    [Tooltip("Spawn points")]
    public Transform[] points;
    [Tooltip("Time between spawn enemies.")]
    public float timeBtwSpawn;
    [Tooltip("Start time spawn")]
    public float startSpawn;

    private int enemiesInGame = 0;

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
            int p = Random.Range(0, points.Length);
            int x = Random.Range(0, enemies.Length);

            Instantiate(enemies[x], points[p].position, points[p].rotation);
            enemiesInGame++;
            yield return new WaitForSeconds(timeBtwSpawn);
        }
    }
}
