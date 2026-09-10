using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Transform rowParent;
    [SerializeField] private GameObject rowPrefab;

    private List<LeaderboardEntry> stats = new List<LeaderboardEntry>();

    private void Start()
    {
        stats = LbBehaviour.Instance.LbData.Entries;
        ShowLeaderboard();
    }

    private void ShowLeaderboard()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            GameObject clone = Instantiate(rowPrefab, rowParent);
            LeaderboardRow row = clone.GetComponent<LeaderboardRow>();
            row.ChangeText(i + 1, stats[i].Name, stats[i].WavesSurvived, stats[i].EnemiesKilled);
        }
    }
}
