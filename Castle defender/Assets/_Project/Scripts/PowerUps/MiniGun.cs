using System;
using UnityEngine;

public class MiniGun : PowerUp
{
    [SerializeField] private float timeBetweenArrows;

    private DrawString drawString;

    protected override void Apply(Arrow arrow)
    {
        drawString = arrow.Bow.GetComponentInChildren<DrawString>();
        drawString.MiniGunTimeBetween = timeBetweenArrows;
        drawString.UseMinigun = true;
    }

    protected override void Remove(Arrow arrow)
    {
        drawString.UseMinigun = false;
    }
}
