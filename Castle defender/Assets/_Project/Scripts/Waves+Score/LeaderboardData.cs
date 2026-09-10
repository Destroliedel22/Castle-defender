using System.Collections.Generic;

[System.Serializable]
public class LeaderboardEntry
{
    public string Name;
    public int WavesSurvived;
    public int EnemiesKilled;
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> Entries;
}
