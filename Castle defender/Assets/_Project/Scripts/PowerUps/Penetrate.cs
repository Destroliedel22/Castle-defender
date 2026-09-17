using UnityEngine;

public class Penetrate : PowerUp
{
    DrawString drawString;

    protected override void Apply(Arrow arrow)
    {
        drawString = arrow.Bow.GetComponentInChildren<DrawString>();
        drawString.CanPenetrate = true;
    }

    protected override void Remove(Arrow arrow)
    {
        drawString.CanPenetrate = false;
    }
}
