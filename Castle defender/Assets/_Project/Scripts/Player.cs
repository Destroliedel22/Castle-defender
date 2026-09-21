using UnityEngine;

public class Player : MonoBehaviour
{
    public static event System.Action GameOver;

    public PlayerSettings Settings;
    public int Health;

    [SerializeField] private CharacterController controller;

    private bool gameOverEventCalled;
    private bool posFixed;

    private void Awake()
    {
        Health = Settings.Health;
    }

    private void Update()
    {
        if (controller.center.x != 0 && !posFixed)
        {
            transform.position = new Vector3(-controller.center.x, transform.position.y, -controller.center.z);
            posFixed = true;
        }


        if (Health <= 0)
        {
            if (!gameOverEventCalled)
            {
                gameOverEventCalled = true;
                GameOver?.Invoke();
            }
        }
    }
}
