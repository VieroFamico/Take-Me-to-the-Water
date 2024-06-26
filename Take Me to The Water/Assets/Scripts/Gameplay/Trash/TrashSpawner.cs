using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("Trash Prefabs By Type")]
    public List<GameObject> canTrashPrefabs; // List of plastic trash prefabs
    public List<GameObject> bottleTrashPrefabs; // List of metal trash prefabs
    public List<GameObject> tireTrashPrefabs; // List of wood trash prefabs
    public List<GameObject> branchTrashPrefabs; // List of rubber trash prefabs
    public List<GameObject> treeTrunkTrashPrefabs; // List of glass trash prefabs

    private List<List<GameObject>> trashPrefabsByType = new List<List<GameObject>>(); // Nested list of trash prefabs by type

    [Header("Spawn Settings")]
    public Transform player; // Reference to the player
    public float spawnDistance = 10f; // Distance from the player to spawn trash
    public Vector3 moveDirection = Vector3.forward; // Direction for all trash objects to move
    public float spawnInterval = 2f; // Interval between spawns
    public float trashMoveSpeed = 2f; // Speed of the trash objects

    private float timeUntilSpawn;

    private void Start()
    {
        player = FindAnyObjectByType<BoatMovement>().transform;
        InitializeTrashPrefabs();
    }

    private void InitializeTrashPrefabs()
    {
        trashPrefabsByType.Add(canTrashPrefabs);
        trashPrefabsByType.Add(bottleTrashPrefabs);
        trashPrefabsByType.Add(tireTrashPrefabs);
        trashPrefabsByType.Add(branchTrashPrefabs);
        trashPrefabsByType.Add(treeTrunkTrashPrefabs);

        // Ensure you have 5 types of trash
        if (trashPrefabsByType.Count < 5)
        {
            Debug.LogError("Please add all 5 types of trash prefabs to trashPrefabsByType.");
        }
    }

    private void Update()
    {
        if (trashPrefabsByType.Count == 0) return;

        timeUntilSpawn += Time.deltaTime;
        if (timeUntilSpawn >= spawnInterval)
        {
            SpawnTrash();
            timeUntilSpawn = 0;
        }
    }

    private void SpawnTrash()
    {
        // Select a random type of trash (20% chance for each type)
        int trashTypeIndex = Random.Range(0, trashPrefabsByType.Count);

        // Select a random prefab within the chosen type
        List<GameObject> selectedTrashType = trashPrefabsByType[trashTypeIndex];
        if (selectedTrashType.Count == 0) return;
        GameObject selectedTrashPrefab = selectedTrashType[Random.Range(0, selectedTrashType.Count)];

        // Calculate spawn position
        Vector3 spawnPosition = player.position - (moveDirection.normalized * spawnDistance);
        Vector2 randomize = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
        spawnPosition += new Vector3(randomize.x, 0f, randomize.y);

        // Instantiate the selected trash prefab
        GameObject trashInstance = Instantiate(selectedTrashPrefab, spawnPosition, Quaternion.identity);
        trashInstance.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        trashInstance.transform.parent = this.transform;

        // Configure the trash movement
        Trash trashScript = trashInstance.GetComponent<Trash>();
        if (trashScript != null)
        {
            trashScript.SetMoveDirection(moveDirection);
            trashScript.SetSpeed(trashMoveSpeed);
            trashScript.SetPlayer(player);
        }
    }

    public void ReturnToSpawn(Trash trash)
    {
        Vector3 spawnPosition = player.position - (moveDirection.normalized * spawnDistance);
        Vector2 randomize = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
        spawnPosition += new Vector3(randomize.x, 0f, randomize.y);

        trash.transform.position = spawnPosition;
    }
}
