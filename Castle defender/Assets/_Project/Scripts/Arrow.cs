using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool IsShot;

    [SerializeField] private Collider tipCollider;
    [SerializeField] private TrailRenderer trailRenderer;

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
                Stuck();
                break;

            case ("Ground"):
                if(IsShot)
                {
                    GroundHit();
                    Stuck();
                }
                break;

            case ("Ricochet"):
                Ricochet();
                break;
        }
    }

    private void FixedUpdate()
    {
        if(IsShot)
        {
            Vector3 direction = rb.linearVelocity;
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            if(trailRenderer.enabled == false)
                trailRenderer.enabled = true;
        }
        else
        {
            if (trailRenderer.enabled == true)
                trailRenderer.enabled = false;
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
    }

    private void GroundHit()
    {
        print("Ground hit");
    }

    private void Ricochet()
    {
        print("Ricochet");
    }

    private void Stuck()
    {
        transform.parent = HitObject.transform;
        rb.isKinematic = true;
        IsShot = false;
    }
}
