using UnityEngine;

public class SpawnArrow : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;

    public void Spawn()
    {
        GameObject clone = Instantiate(arrowPrefab, transform);
    }
}
