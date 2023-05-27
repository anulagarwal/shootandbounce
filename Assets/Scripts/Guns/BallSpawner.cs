using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public List<GameObject> ballPrefabs;
    public Transform spawnPoint;
    public float spawnRate = 1f;  // spawns per second
    public Vector2 valueRange = new Vector2(1, 10);

    private float nextSpawnTime;
    private int currentPrefabIndex = 0;

    private void Start()
    {
        spawnRate = UpgradeManager.instance.GetDropSpeed();
    }
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnBall();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    private void SpawnBall()
    {
        float ballValue = Random.Range(valueRange.x, valueRange.y);
        Vector3 v = new Vector3(spawnPoint.position.x + Random.Range(-1, 1), spawnPoint.position.y + Random.Range(-1, 1), spawnPoint.position.z);
        GameObject newBall = Instantiate(ballPrefabs[currentPrefabIndex], v, Quaternion.identity);
        newBall.GetComponent<Ball>().SetValue(UpgradeManager.instance.GetDropValue());
        spawnRate = UpgradeManager.instance.GetDropSpeed();

        // Move to the next prefab in the list, looping back to the start if necessary
        currentPrefabIndex++;
        if (currentPrefabIndex >= ballPrefabs.Count)
        {
            currentPrefabIndex = 0;
        }
    }
}
