using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
#pragma warning disable CS0649
    [Header("Setup")]
    [SerializeField, Tooltip("Enemies to spawn.")]
    private GameObject[] enemies;
    [SerializeField, Tooltip("Count of enemies to spawn.")]
    private int count;
    [SerializeField, Tooltip("Time between spawn enemies.")]
    private float timeBtwSpawn;
    [SerializeField, Tooltip("Start time spawn")]
    private float startSpawn;
#pragma warning restore CS0649

    private int enemiesInGame = 0;

    [DrawVectorRelativeToTransform]
    public List<Vector2> points;

    private void Start() => StartCoroutine(SpawnEnemies());

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
