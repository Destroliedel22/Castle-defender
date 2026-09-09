using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<Transform> ladders = new List<Transform>();
    [SerializeField] private int startEnemyAmount;
    [SerializeField] private int minEnemyIncrease;
    [SerializeField] private int maxEnemyIncrease;
    [SerializeField] private float secondsBetweenSpawns;

    private List<GameObject> aliveEnemies = new List<GameObject>();
    private int currentWave = 0;

    private void OnEnable()
    {
        ButtonManager.OnStartGame += StartGame;
        Player.GameOver += GameOver;
    }

    private void OnDisable()
    {
        ButtonManager.OnStartGame -= StartGame;
        Player.GameOver -= GameOver;
    }

    private void StartGame()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        currentWave++;

        for (int i = 0; i < startEnemyAmount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemy = enemies[Random.Range(0, enemies.Count)];
            GameObject clone = Instantiate(enemy, spawnPoint);
            aliveEnemies.Add(clone);

            Enemy enemyScript = clone.GetComponent<Enemy>();
            enemyScript.Target = ladders[Random.Range(0, ladders.Count)];
            enemyScript.OnDeath += EnemyDeath;
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }
    }

    private void EnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= EnemyDeath;

        HighScore.Instance.EnemiesKilled++;

        aliveEnemies.Remove(enemy.gameObject);
        if (aliveEnemies.Count <= 0)
        {
            startEnemyAmount += Random.Range(minEnemyIncrease, maxEnemyIncrease);
            HighScore.Instance.WavesSurvived++;
            StartCoroutine(SpawnWave());
        }
    }

    private void GameOver()
    {
        foreach(GameObject enemy in aliveEnemies)
        {
            Destroy(enemy);
        }
    }
}
