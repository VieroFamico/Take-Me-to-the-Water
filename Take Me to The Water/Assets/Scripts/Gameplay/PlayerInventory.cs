using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public float money = 100f; // Starting money for the player
    private FishInventory fishInventory;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Prevent PlayerInventory from being destroyed on scene load
    }

    private void Start()
    {
        fishInventory = GameManager.Instance.fishInventory;
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        FindPlayerFishInventory();
    }

    public FishInventory GetPlayerFishInventory()
    {
        return fishInventory;
    }

    private void FindPlayerFishInventory()
    {
        fishInventory = GameManager.Instance.fishInventory;
    }

    public void CatchFish(FishData fish)
    {
        fishInventory.AddFish(fish); // Add the fish to the inventory
        SaveManager.SaveFishInventory(fishInventory, "PlayerInventory.json"); // Save the inventory
    }
    public void AddMoney(float amount)
    {
        money += amount;
    }

    public bool SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }

}
