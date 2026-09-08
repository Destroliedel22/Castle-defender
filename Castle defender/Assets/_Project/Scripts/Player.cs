using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSettings Settings;
    public int Health;

    [SerializeField] private MeshRenderer fadeOutRend;

    private Material fadeOutMat;
    private Color color;

    private void Awake()
    {
        Health = Settings.Health;
        fadeOutMat = fadeOutRend.material;
        color = fadeOutMat.color;
    }

    private void Update()
    {
        if(Health <= 0)
        {
            if (color.a < 1)
            {
                color.a += 0.01f;
                fadeOutMat.color = color;
            }
            else
            {
                //GameOver
            }
        }
    }
}
