using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] private float powerUpTime;

    private Collider[] colliders;
    private Image image;
    private int arrowLayer = 10;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        colliders = GetComponents<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == arrowLayer)
        {
            Arrow arrow = other.GetComponent<Arrow>();
            if (arrow.IsShot)
            {
                StartCoroutine(PowerUpTimer(arrow));
                image.enabled = false;
                foreach (Collider col in colliders)
                    col.enabled = false;
            }
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
        Destroy(gameObject);
    }
}
