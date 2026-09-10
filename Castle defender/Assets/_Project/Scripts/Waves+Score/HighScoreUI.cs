using System.Collections;
using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI wavesText;
    [SerializeField] private TextMeshProUGUI enemiesText;

    [SerializeField] private GameObject keyboard;

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

    public void SetName()
    {
        if (inputField.text != "" && inputField.text.Length <= 10)
        {
            HighScore.Instance.Name = inputField.text;
            keyboard.SetActive(false);
        }
        else
            StartCoroutine(WarningText("Must contain 1-10 characters"));

    }

    IEnumerator WarningText(string warning)
    {
        string input = inputField.text;
        inputField.text = warning;
        yield return new WaitForSeconds(1f);
        inputField.text = input;
    }
}
