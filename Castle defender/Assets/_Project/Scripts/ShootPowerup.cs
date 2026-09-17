using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUps
{
    public GameObject PowerUpPrefab;
    public float SpawnWeight;
}

public class ShootPowerup : MonoBehaviour
{
    [SerializeField] private List<PowerUps> powerUps = new List<PowerUps>();

    [SerializeField] private float nothingWeight;

    [SerializeField] private int forceAmount;

    private GameObject PowerUp()
    {
        float totalChances = 0;
        foreach (PowerUps powerUp in powerUps)
            totalChances += powerUp.SpawnWeight;

        totalChances += nothingWeight;
        float randomNumb = Random.Range(0, totalChances);

        float chance = 0;
        foreach (PowerUps powerUp in powerUps)
        {
            chance += powerUp.SpawnWeight;
            if (chance > randomNumb)
                return powerUp.PowerUpPrefab;
        }

        return null;
    }

    public void LaunchPowerUp(Transform pos)
    {
        GameObject powerUp = PowerUp();

        if (powerUp)
        {
            GameObject clone = Instantiate(powerUp, pos.position, powerUp.transform.rotation);

            int tiltAngle = Random.Range(0, 30);
            int randomYRotation = Random.Range(0, 360);

            Vector3 direction = Quaternion.Euler(tiltAngle, randomYRotation, 0) * Vector3.up;

            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.AddForce(direction * forceAmount);
        }
    }
}
