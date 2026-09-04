using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public List<GameObject> aliveEnemies = new List<GameObject>();

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private int startEnemyAmount;

    private void Start()
    {
        SpawnWave();
    }
    
    private void SpawnWave()
    {
        for (int i = 0; i < startEnemyAmount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemy = enemies[Random.Range(0, enemies.Count)];
            GameObject clone = Instantiate(enemy, spawnPoint);
            aliveEnemies.Add(clone);
        }
    }
}
