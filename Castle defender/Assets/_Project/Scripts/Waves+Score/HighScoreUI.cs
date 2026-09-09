using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wavesText;
    [SerializeField] private TextMeshProUGUI enemiesText;

    [SerializeField] private string baseWaveText;
    [SerializeField] private string baseEnemiesText;

    private int wavesSurvived;
    private int enemiesKilled;

    private void Update()
    {
        if (HighScore.Instance.WavesSurvived > wavesSurvived)
        {
            wavesSurvived = HighScore.Instance.WavesSurvived;
            wavesText.text = baseWaveText + wavesSurvived;
        }

        if(HighScore.Instance.EnemiesKilled > enemiesKilled)
        {
            enemiesKilled = HighScore.Instance.EnemiesKilled;
            enemiesText.text = baseEnemiesText + enemiesKilled;
        }
    }
}
