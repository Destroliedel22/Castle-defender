using UnityEngine;

public class BowDeathZone : MonoBehaviour
{
    [SerializeField] private Transform tpTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bow"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.position = tpTransform.position;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            other.transform.position = tpTransform.position;
    }
}
