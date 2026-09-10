using TMPro;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI killsText;

    public void ChangeText(int rank, string name, int wave, int kills)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        waveText.text = wave.ToString();
        killsText.text = kills.ToString();
    }
}
