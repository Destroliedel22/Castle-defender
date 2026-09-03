using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject HitObject;
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

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

    private void FixedUpdate()
    {
        if(rb.linearVelocity.magnitude > 5)
        {
            Vector3 direction = rb.linearVelocity;
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
        }
    }

    public void SwitchSettings()
    {
        rb.useGravity = !rb.useGravity;
        rb.isKinematic = !rb.isKinematic;
        col.isTrigger = !col.isTrigger;
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
