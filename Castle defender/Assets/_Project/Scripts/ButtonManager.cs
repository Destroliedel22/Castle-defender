using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static event System.Action OnStartGame;

    private bool gameStarted;

    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject RestartButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject AreYouSure;

    public void StartGame()
    {
        gameStarted = true;
        StartButton.SetActive(false);
    }

    public void EnterPressed()
    {
        if (HighScore.Instance.Name != "")
        {
            RestartButton.SetActive(true);
            OnStartGame?.Invoke();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitPressed()
    {
        AreYouSure.SetActive(true);
        QuitButton.SetActive(false);
    }

    public async Task QuitGame()
    {
        if (gameStarted)
            await LbBehaviour.Instance.AddEntry(HighScore.Instance.Name, HighScore.Instance.WavesSurvived, HighScore.Instance.EnemiesKilled);

        Application.Quit();
    }

    public void Yes()
    {
        QuitGame();
    }

    public void No()
    {
        AreYouSure.SetActive(false);
        QuitButton.SetActive(true);
    }
}
