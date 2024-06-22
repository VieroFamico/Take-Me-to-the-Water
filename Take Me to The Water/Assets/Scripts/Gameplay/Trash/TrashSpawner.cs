using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public List<GameObject> trashPrefabs; // List of trash prefabs
    public Transform player; // Reference to the player
    public float spawnDistance = 10f; // Distance from the player to spawn trash
    public Vector3 moveDirection = Vector3.forward; // Direction for all trash objects to move
    public float spawnInterval = 2f; // Interval between spawns
    public float trashMoveSpeed = 2f;

    private float timeUntilSpawn;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (trashPrefabs.Count == 0) return;
        timeUntilSpawn += Time.deltaTime;
        if (timeUntilSpawn >= spawnInterval)
        {
            Vector3 spawnPosition = player.position - (moveDirection.normalized * spawnDistance);
            Vector2 randomize = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
            spawnPosition += new Vector3(randomize.x, 0f, randomize.y);

            GameObject selectedTrashPrefab = trashPrefabs[Random.Range(0, trashPrefabs.Count)];
            GameObject trashInstance = Instantiate(selectedTrashPrefab, spawnPosition, Quaternion.identity);

            trashInstance.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            Trash trashScript = trashInstance.GetComponent<Trash>();
            if (trashScript != null)
            {
                trashScript.SetMoveDirection(moveDirection);
                trashScript.SetSpeed(trashMoveSpeed);
            }

            timeUntilSpawn = 0;
        }
    }
}
