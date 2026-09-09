using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LbBehaviour : MonoBehaviour
{
    public static LbBehaviour Instance;

    public LeaderboardData LbData;

    private string savePath;

    private void OnEnable()
    {
        Player.GameOver += GameOver;
    }

    private void OnDisable()
    {
        Player.GameOver -= GameOver;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");

        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(LbData);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            LbData = JsonUtility.FromJson<LeaderboardData>(json);
        }
        else
        {
            LbData = new LeaderboardData();
            LbData.Entries = new List<LeaderboardEntry>();
        }
    }

    public void AddEntry(int wavesSurvived, int enemiesKilled)
    {
        LeaderboardEntry entry = new LeaderboardEntry();
        entry.WavesSurvived = wavesSurvived;
        entry.EnemiesKilled = enemiesKilled;

        LbData.Entries.Add(entry);

        LbData.Entries.Sort((a, b) =>
        {
            if (b.WavesSurvived == a.WavesSurvived)
                return b.EnemiesKilled.CompareTo(a.EnemiesKilled);
            else
                return b.WavesSurvived.CompareTo(a.WavesSurvived);
        });

        if (LbData.Entries.Count > 10)
            LbData.Entries.RemoveRange(10, LbData.Entries.Count - 10);

        Save();
    }

    private void GameOver()
    {
        AddEntry(HighScore.Instance.WavesSurvived, HighScore.Instance.EnemiesKilled);
    }
}
