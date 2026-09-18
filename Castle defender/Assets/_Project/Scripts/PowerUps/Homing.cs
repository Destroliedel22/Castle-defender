using UnityEngine;

public class Homing : PowerUp
{
    [SerializeField] private float homingTurnSpeed;

    private DrawString drawString;

    protected override void Apply(Arrow arrow)
    {
        drawString = arrow.Bow.GetComponentInChildren<DrawString>();
        drawString.IsHoming = true;
        drawString.HomingTurnSpeed = homingTurnSpeed;
    }

    protected override void Remove(Arrow arrow)
    {
        drawString.IsHoming = false;
    }
}
