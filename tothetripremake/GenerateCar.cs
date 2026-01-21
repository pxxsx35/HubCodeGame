using System.Collections.Generic;
using UnityEngine;

public class GenerateCar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject[] carPrefabs;

    [Header("Pool Configuration")]
    [SerializeField] private int poolSize = 15; 
    [SerializeField] private float[] lanes;

    [Header("Recycle Settings")]
    [SerializeField] private float spawnForwardDistance = 80f;
    [SerializeField] private float despawnBackwardDistance = 25f;

    private List<GameObject> carPool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)]);
            car.SetActive(false);
            carPool.Add(car);
        }

        InitialSpawn();
    }

    void Update()
    {
        MonitorAndRecycle();
    }

    private void InitialSpawn()
    {
        foreach (var car in carPool)
        {
            float randomX = playerTransform.position.x + Random.Range(30, spawnForwardDistance);
            SpawnCar(car, randomX);
        }
    }

    private void MonitorAndRecycle()
    {
        foreach (var car in carPool)
        {
            if (car.activeSelf)
            {
                float distFromPlayer = playerTransform.position.x - car.transform.position.x;
                if (distFromPlayer > despawnBackwardDistance)
                {
                    float nextX = playerTransform.position.x + spawnForwardDistance + Random.Range(0, 20f);
                    SpawnCar(car, nextX);
                }
            }
            else
            {
                float nextX = playerTransform.position.x + spawnForwardDistance + Random.Range(10, 40f);
                SpawnCar(car, nextX);
            }
        }
    }

    private void SpawnCar(GameObject car, float positionX)
    {
        float targetZ = lanes[Random.Range(0, lanes.Length)];
        car.transform.position = new Vector3(positionX, playerTransform.position.y, targetZ);
        car.SetActive(true);

        if (car.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}