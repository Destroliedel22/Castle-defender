using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] private float powerUpTime;
    [SerializeField] private int arrowLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == arrowLayer)
        {
            print("Arrow");
            StartCoroutine(PowerUpTimer(other.GetComponent<Arrow>()));
        }
    }

    protected abstract void Apply(Arrow arrow);

    protected abstract void Remove(Arrow arrow);

    private IEnumerator PowerUpTimer(Arrow arrow)
    {
        Apply(arrow);

        float time = 0;
        while (time < powerUpTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Remove(arrow);
    }
}
