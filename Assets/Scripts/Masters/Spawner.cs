using System.Collections;
using System.Collections.Generic;
using AdditionalAttributes;
using AdditionalComponents;
using Master;
using Navigation;
using UnityEngine;

public class Spawner : MonoBehaviour
{
#pragma warning disable CS0649
    [Header("Configuration")]
    [SerializeField, Tooltip("Enemies to spawn.")]
    private Enemy[] enemies;
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
    [SerializeField, Tooltip("Particle effect.")]
    private GameObject particle;

    [Header("Setup")]
    [SerializeField, Tooltip("Navigation Graph used to produce enemy movement.")]
    private NavigationGraph navigationGraph;
#pragma warning restore CS0649

    private int enemiesAlive;
    private GameObject particleInstantiated;
    private int random;
    private GameObject enemyToSpawn;

    [DrawVectorRelativeToTransform]
    public List<Vector2> points;

    private void Start() => StartCoroutine(SpawnEnemies());

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(startSpawn);
        for (; enemiesToSpawn > 0; enemiesToSpawn--)
        {
            yield return new WaitUntil(() => enemiesAlive < simultaneousEnemies);
            yield return new WaitWhile(() => Settings.IsPause);

            int p = Random.Range(0, points.Count);
            enemyToSpawn = CumulativeProbability();

            particleInstantiated = Instantiate(particle, new Vector2(points[p].x, points[p].y), 
                Quaternion.identity);
            SpawnEnemy(enemyToSpawn, points[p]);

            yield return new WaitForSeconds(timeBtwSpawn);
        }
        SpawnEnemy(boss, points[Random.Range(0, points.Count)]);
        yield return new WaitWhile(() => enemiesAlive > 0);
        Global.menu.GameOver(true);
    }

    private void SpawnEnemy(GameObject gameObject, Vector2 position)
    {
        GameObject enemy = Instantiate(gameObject, position + (Vector2)transform.position, Quaternion.identity);

        // Enemy counter
        enemiesAlive++;
        DestroyNotifier.ExecuteOnDeath(enemy, () => enemiesAlive--);

        // Enemy movement
        NavigationAgent.InjectNavigationGraph(enemy, navigationGraph);
    }

    // For more information http://www.vcskicks.com/random-element.php
    private GameObject CumulativeProbability()
    {
        random = Random.Range(0, 100);
        int cumulative = 0;

        foreach (Enemy enemy in enemies)
        {
            cumulative += enemy.probability;
            if (random <= cumulative) return enemy.prefabEnemy;
        }

        return null;
    }
}

[System.Serializable]
public class Enemy
{
    [Tooltip("Prefab of the enemy.")]
    public GameObject prefabEnemy;
    [Tooltip("Probability that the enemy has to be spawned.")]
    public int probability;
}
