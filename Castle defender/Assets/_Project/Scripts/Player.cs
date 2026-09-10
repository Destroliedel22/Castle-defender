using UnityEngine;

public class Player : MonoBehaviour
{
    public static event System.Action GameOver;

    public PlayerSettings Settings;
    public int Health;

    private bool gameOverEventCalled;

    private void Awake()
    {
        Health = Settings.Health;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            if(!gameOverEventCalled)
            {
                gameOverEventCalled = true;
                GameOver?.Invoke();
            }
        }
    }
}
