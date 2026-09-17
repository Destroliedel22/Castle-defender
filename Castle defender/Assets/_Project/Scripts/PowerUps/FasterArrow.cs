using UnityEngine;

public class FasterArrow : PowerUp
{
    [SerializeField] private PlayerSettings normalSettings;
    [SerializeField] private PlayerSettings fastSettings;

    private DrawString drawString;

    protected override void Apply(Arrow arrow)
    {
        drawString = arrow.Bow.GetComponent<DrawString>();
        drawString.Settings = fastSettings;
    }

    protected override void Remove(Arrow arrow)
    {
        drawString.Settings = normalSettings;
    }
}
