using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public List<GameObject> aliveEnemies = new List<GameObject>();

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private int startEnemyAmount;
    [SerializeField] private int minEnemyIncrease;
    [SerializeField] private int maxEnemyIncrease;
    [SerializeField] private Transform target;

    private int currentWave = 0;

    private void Start()
    {
        SpawnWave();
    }
    
    private void SpawnWave()
    {
        currentWave++;

        for (int i = 0; i < startEnemyAmount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemy = enemies[Random.Range(0, enemies.Count)];
            GameObject clone = Instantiate(enemy, spawnPoint);
            aliveEnemies.Add(clone);
            Enemy enemyScript = clone.GetComponent<Enemy>();
            enemyScript.target = target;
            enemyScript.OnDeath += EnemyDeath;
        }
    }

    private void EnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= EnemyDeath;
        aliveEnemies.Remove(enemy.gameObject);
        if(aliveEnemies.Count <= 0)
        {
            startEnemyAmount += Random.Range(minEnemyIncrease, maxEnemyIncrease);
            SpawnWave();
        }
    }
}
