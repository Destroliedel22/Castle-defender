using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Collider tipCollider;

    private GameObject HitObject;
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider hitCollider = collision.GetContact(0).thisCollider;

        if (hitCollider == tipCollider)
            OnHit(collision);
    }


    public void OnHit(Collision collision)
    {
        HitObject = collision.gameObject;
        switch (LayerMask.LayerToName(HitObject.layer))
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
        HitObject.GetComponentInParent<Enemy>().Death();
        transform.parent = HitObject.transform;
        rb.isKinematic = true;
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
