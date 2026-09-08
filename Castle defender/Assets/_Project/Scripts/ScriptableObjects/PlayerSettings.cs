using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public int Health;

    public float MinDrawDistance;
    public float MaxDrawDistance;

    public float MinArrowSpeed;
    public float MaxArrowSpeed;

    public float ArrowLoadSpeed;
}
