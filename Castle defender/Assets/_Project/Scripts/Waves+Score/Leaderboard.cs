using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
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
            //print(stats[i].WavesSurvived);
            //print(stats[i].EnemiesKilled);
            //print("_");
        }
    }
}
