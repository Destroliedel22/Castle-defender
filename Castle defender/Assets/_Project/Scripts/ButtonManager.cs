using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static event System.Action OnStartGame;

    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject RestartButton;
    [SerializeField] private GameObject QuitButton;

    private void OnEnable()
    {
        Player.GameOver += GameOver;
    }

    private void OnDisable()
    {
        Player.GameOver -= GameOver;
    }

    public void StartGame()
    {
        if(HighScore.Instance.Name != null)
        {
            OnStartGame?.Invoke();
            StartButton.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GameOver()
    {
        RestartButton.SetActive(true);
        QuitButton.SetActive(true);
    }
}
