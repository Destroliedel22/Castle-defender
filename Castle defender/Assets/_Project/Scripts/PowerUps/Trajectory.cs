using UnityEngine;

public class Trajectory : PowerUp
{
    private DrawString drawString;

    protected override void Apply(Arrow arrow)
    {
        drawString = arrow.Bow.GetComponentInChildren<DrawString>();
        drawString.UseTrajectory = true;
    }

    protected override void Remove(Arrow arrow)
    {
        drawString.UseTrajectory = false;
    }
}
