using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public List<GameObject> ballPrefabs;
    public GameObject bomb;

    public Transform spawnPoint;
    public float spawnRate = 1f;  // spawns per second
    public Vector2 valueRange = new Vector2(1, 10);

    private float nextSpawnTime;
    private int currentPrefabIndex = 0;
    private int currentSpawnIndex = 0;

    public List<int> bombSpawnIndexes;

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
            int i = UpgradeManager.instance.GetDropValue() + GetSpawnLevel();
            newBall.GetComponent<Ball>().SetValue(i);
        
        spawnRate = UpgradeManager.instance.GetDropSpeed();

        currentSpawnIndex++;
        

        // Move to the next prefab in the list, looping back to the start if necessary
        currentPrefabIndex++;
        if (currentPrefabIndex >= ballPrefabs.Count)
        {
            currentPrefabIndex = 0;
        }
    }

    public int GetSpawnLevel()
    {
        int i = GunSelectionGridManager.Instance.GetHighestLevel();
        float random = Random.Range(0, 1);
        if (random < 0.33f)
        {
            i = i - 1;
            if (i < 0)
            {
                i = 0;
            }

        }
        if (random >= 0.33f && random < 0.66f)
        {
            

        }
        if (random >= 0.66f)
        {
            i++;
            if (i >= ballPrefabs.Count)
            {
                i = ballPrefabs.Count - 1;
            }
        }
        return i*2;
    }
}
