using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public List<GameObject> aliveEnemies = new List<GameObject>();
}

public class Waves : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<Transform> ladders = new List<Transform>();

    [SerializeField] private List<WaveData> waveList = new List<WaveData>();

    [SerializeField] private int startEnemyAmount;
    [SerializeField] private int minEnemyIncrease;
    [SerializeField] private int maxEnemyIncrease;

    [SerializeField] private float secondsBetweenSpawns;

    [SerializeField] private float waveSpawnTime;

    private float waveSpawnTimer;

    private bool gameStarted;

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

    private void Start()
    {
        waveSpawnTimer = waveSpawnTime;
    }

    private void StartGame()
    {
        StartCoroutine(SpawnWave());
        gameStarted = true;
    }

    private void Update()
    {
        if(gameStarted)
        {
            if (waveSpawnTimer > 0f)
                waveSpawnTimer -= Time.deltaTime;
            else
            {
                if (waveSpawnTime > 30f)
                    waveSpawnTime -= 2f;
                else if (waveSpawnTime > 15f)
                    waveSpawnTime -= 1f;
                waveSpawnTimer = waveSpawnTime;
                startEnemyAmount += Random.Range(minEnemyIncrease, maxEnemyIncrease);
                StartCoroutine(SpawnWave());
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        currentWave++;

        List<GameObject> aliveEnemies = new List<GameObject>();

        for (int i = 0; i < startEnemyAmount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemy = enemies[Random.Range(0, enemies.Count)];
            GameObject clone = Instantiate(enemy, spawnPoint);
            aliveEnemies.Add(clone);

            Enemy enemyScript = clone.GetComponent<Enemy>();
            enemyScript.Target = ladders[Random.Range(0, ladders.Count)];
            enemyScript.OnDeath += EnemyDeath;
            enemyScript.waveSpawned = currentWave;
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }

        AddWave(aliveEnemies);
    }

    private void EnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= EnemyDeath;

        HighScore.Instance.EnemiesKilled++;

        List<GameObject> aliveEnemies = waveList[enemy.waveSpawned - 1].aliveEnemies;
        aliveEnemies.Remove(enemy.gameObject);
        if (aliveEnemies.Count <= 0)
            HighScore.Instance.WavesSurvived++;
    }

    private void GameOver()
    {
        foreach(WaveData wave in waveList)
            foreach(GameObject enemy in wave.aliveEnemies)
                Destroy(enemy);
    }

    private void AddWave(List<GameObject> aliveEnemies)
    {
        WaveData wave = new WaveData();
        wave.aliveEnemies = aliveEnemies;
        waveList.Add(wave);
    }
}
