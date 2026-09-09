using System.Collections.Generic;

[System.Serializable]
public class LeaderboardEntry
{
    public int WavesSurvived;
    public int EnemiesKilled;
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> Entries;
}
