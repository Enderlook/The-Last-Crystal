using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
#pragma warning disable CS0649
    [Header("Setup")]
    [SerializeField, Tooltip("Enemies to spawn.")]
    private GameObject[] enemies;
    [SerializeField, Tooltip("Maximum amount of enemies at the same time")]
    private int simultaneousEnemies;
    [SerializeField, Tooltip("Total enemies to spawn.")]
    private int enemiesToSpawn;
    [SerializeField, Tooltip("Time between spawn enemies.")]
    private float timeBtwSpawn;
    [SerializeField, Tooltip("Start time spawn")]
    private float startSpawn;
    [SerializeField, Tooltip("Boss spawned after all enemies die.")]
    private GameObject boss;
#pragma warning restore CS0649

    private int enemiesAlive = 0;

    [DrawVectorRelativeToTransform]
    public List<Vector2> points;

    private void Start() => StartCoroutine(SpawnEnemies());

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(startSpawn);
        for (; enemiesToSpawn > 0; enemiesToSpawn--, enemiesAlive++)
        {
            yield return new WaitUntil(() => enemiesAlive < simultaneousEnemies);
            yield return new WaitWhile(() => Menu.IsPause);

            int p = Random.Range(0, points.Count);
            int x = Random.Range(0, enemies.Length);

            GameObject enemy = Instantiate(enemies[x], points[p] + (Vector2)transform.position, Quaternion.identity);

            enemy.AddComponent<DestroyNotifier>().SetCallback(() => enemiesAlive--);
            SpawnEnemy(enemies[x], points[p]);

            yield return new WaitForSeconds(timeBtwSpawn);
        }

        SpawnEnemy(boss, points[Random.Range(0, points.Count)]);
        yield return new WaitWhile(() => enemiesAlive > 0);
        Global.menu.GameOver(true);
    }

    private void SpawnEnemy(GameObject gameObject, Vector2 position)
    {
        GameObject enemy = Instantiate(gameObject, position + (Vector2)transform.position, Quaternion.identity);
        enemy.AddComponent<DestroyNotifier>().SetCallback(() => enemiesAlive--);
    }
}
