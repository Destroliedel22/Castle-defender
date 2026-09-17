using UnityEngine;

public class MultiShot : PowerUp
{
    [SerializeField] private int angle;

    private DrawString drawString;

    protected override void Apply(Arrow arrow)
    {
        drawString = arrow.Bow.GetComponentInChildren<DrawString>();
        drawString.MultiShotAngle = angle;
        drawString.UseMultiShot = true;
    }

    protected override void Remove(Arrow arrow)
    {
        drawString.UseMultiShot = true;
    }
}
