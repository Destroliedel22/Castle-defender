using UnityEngine;

[CreateAssetMenu(fileName = "BowSettings", menuName = "Scriptable Objects/BowSettings")]
public class BowSettings : ScriptableObject
{
    public string BowName;

    public float MinDrawDistance;
    public float MaxDrawDistance;

    public float MinArrowSpeed;
    public float MaxArrowSpeed;

    public float ArrowLoadSpeed;
}
