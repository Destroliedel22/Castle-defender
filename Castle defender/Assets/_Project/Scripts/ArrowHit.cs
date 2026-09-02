using UnityEngine;

public class ArrowHit : MonoBehaviour
{
    [SerializeField] private Arrow arrow;

    private void OnCollisionEnter(Collision collision)
    {
        arrow.OnHit(collision);
    }
}
