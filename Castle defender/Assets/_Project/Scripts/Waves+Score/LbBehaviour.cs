using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.CloudSave;
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
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
    }

    private async void Start()
    {
        await InitializeServices();
        await LoadFromCloud();
    }

    private async Task InitializeServices()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async Task SaveToCloud()
    {
        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
        dataToSave.Add("Leaderboard", LbData);
        await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
    }

    private async Task LoadFromCloud()
    {
        HashSet<string> keys = new HashSet<string>();
        keys.Add("Leaderboard");
        var result = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if(result.ContainsKey("Leaderboard"))
            LbData = result["Leaderboard"].Value.GetAs<LeaderboardData>();
        else
        {
            LbData = new LeaderboardData();
            LbData.Entries = new List<LeaderboardEntry>();
        }
    }

    public async Task AddEntry(string name, int wavesSurvived, int enemiesKilled)
    {
        LeaderboardEntry entry = new LeaderboardEntry();
        entry.Name = name;
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

        await SaveToCloud();
    }

    private void GameOver()
    {
        HighScore highScore = HighScore.Instance;
        AddEntry(highScore.Name, highScore.WavesSurvived, highScore.EnemiesKilled);
    }
}
