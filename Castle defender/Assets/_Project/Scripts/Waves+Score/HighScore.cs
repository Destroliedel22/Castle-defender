using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public static HighScore Instance { get; private set; }

    public string Name;
    public int WavesSurvived;
    public int EnemiesKilled;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }
}
