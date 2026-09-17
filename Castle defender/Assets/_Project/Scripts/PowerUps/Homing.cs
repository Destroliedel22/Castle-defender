using UnityEngine;

public class Homing : PowerUp
{
    private DrawString drawString;

    protected override void Apply(Arrow arrow)
    {
        drawString = arrow.Bow.GetComponentInChildren<DrawString>();
        drawString.IsHoming = true;
    }

    protected override void Remove(Arrow arrow)
    {
        drawString.IsHoming = false;
    }
}
