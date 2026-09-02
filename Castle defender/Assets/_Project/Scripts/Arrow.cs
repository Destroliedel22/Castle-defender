using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject HitObject; 

    public void OnHit(Collision collision)
    {
        HitObject = collision.gameObject;
        switch (HitObject.tag)
        {
            case ("Enemy"):
                EnemyHit();
                break;
            case ("Ground"):
                GroundHit();
                break;
            case ("Ricochet"):
                Ricochet();
                break;
        }
    }

    private void EnemyHit()
    {
        print("Enemy hit");
    }

    private void GroundHit()
    {
        print("Ground hit");
    }

    private void Ricochet()
    {
        print("Ricochet");
    }
}
