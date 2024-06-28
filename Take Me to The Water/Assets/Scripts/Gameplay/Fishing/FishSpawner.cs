using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> fishPrefabs;
    public List<FishSO> fishDataList;
    public Transform playerTransform;
    public float spawnRate = 5f;
    public int fishCountPerSpawn = 3;
    public float spawnDistance = 20f;

    private DirtinessManager dirtinessManager;

    private void Start()
    {
        dirtinessManager = FindObjectOfType<DirtinessManager>();
        playerTransform = FindAnyObjectByType<BoatMovement>().transform;
        StartCoroutine(SpawnFishRoutine());
    }

    private IEnumerator SpawnFishRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnFish();
        }
    }

    private void SpawnFish()
    {
        int actualFishCountPerSpawn = Random.Range(fishCountPerSpawn, fishCountPerSpawn + 2);
        Vector3 fishGroupSpawnPosition = GetSpawnPosition();
        fishGroupSpawnPosition.y = -3f;

        for (int i = 0; i < actualFishCountPerSpawn; i++)
        {
            Vector3 spawnPosition = fishGroupSpawnPosition + new Vector3(Random.Range(-4, 4), 0f, Random.Range(-4, 4));

            GameObject fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Count)];
            GameObject fishInstance = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);

            Fish fish = fishInstance.GetComponent<Fish>();
            if (fish != null)
            {
                FishSO fishData = fishDataList[Random.Range(0, fishDataList.Count)];
                fish.fishData = fishData;

                fish.fishData.isSick = dirtinessManager.GetSickFishChance();
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0;
        return playerTransform.position + randomDirection.normalized * spawnDistance;
    }
}