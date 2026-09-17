using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject Bow;

    [HideInInspector] public bool IsShot;
    [HideInInspector] public bool HasHit;
    [HideInInspector] public bool CanPenetrate;
    [HideInInspector] public bool IsHoming;

    [SerializeField] private Collider tipCollider;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float killVelocity = 2;

    private Vector3 previousPosition;
    private float calculatedVelocity;

    private Collider[] enemiesInRange;

    private GameObject hitObject;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        Collider hitCollider = collision.GetContact(0).thisCollider;

        if (hitCollider == tipCollider)
            OnHit(collision);
    }

    public void OnHit(Collision collision)
    {
        if (!HasHit && calculatedVelocity > killVelocity)
        {
            HasHit = true;
            hitObject = collision.gameObject;
            switch (LayerMask.LayerToName(hitObject.layer))
            {
                case ("Enemy"):
                    EnemyHit();
                    Stuck();
                    break;

                case ("Shield"):
                    if (CanPenetrate)
                    {
                        EnemyHit();
                        Stuck();
                    }
                    else
                        Stuck();
                    break;

                default:
                    Stuck();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsShot)
        {
            Vector3 direction = rb.linearVelocity.normalized;
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            if (trailRenderer.enabled == false)
                trailRenderer.enabled = true;

            if (IsHoming)
                Homing();
        }

        Vector3 currentPosition = transform.position;
        calculatedVelocity = (currentPosition - previousPosition).magnitude / Time.deltaTime;
        previousPosition = currentPosition;
    }

    private void Homing()
    {
        Transform closestEnemyPos = null;

        if (enemiesInRange.Length < 1 && closestEnemyPos == null)
        {
            enemiesInRange = Physics.OverlapSphere(transform.position, 2f, 6);

            foreach (Collider collider in enemiesInRange)
            {
                if (closestEnemyPos == null || (closestEnemyPos.position.x + closestEnemyPos.position.z) > (collider.transform.position.x + collider.transform.position.z))
                    closestEnemyPos = collider.transform;
            }
        }
        else if (closestEnemyPos != null)
        {
            //coroutine
        }
    }

    private IEnumerator RotateTowardsEnemy(Transform enemyTransform)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(enemyTransform.position);
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime;
            float progress = time / 1f;

            transform.rotation = Quaternion.Slerp(startRot, endRot, progress);

            yield return null;
        }
    }

    public void SwitchSettings()
    {
        rb.useGravity = !rb.useGravity;
        rb.isKinematic = !rb.isKinematic;
    }

    private void EnemyHit()
    {
        print("Enemy hit");
        hitObject.GetComponentInParent<Enemy>().Death();
    }

    private void Stuck()
    {
        transform.parent = hitObject.transform;
        rb.isKinematic = true;
        IsShot = false;
        trailRenderer.enabled = false;
    }
}
