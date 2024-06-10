using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public float money = 100f; // Starting money for the player
    public PlayerLoadout playerLoadout;
    private FishInventory fishInventory;
    private static PlayerInventory instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            playerLoadout = GetComponent<PlayerLoadout>(); // Initialize playerLoadout
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fishInventory = GameManager.Instance.playerInventory.GetPlayerFishInventory();
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
        Debug.Log("Get PlayerFishInventory");
        return fishInventory;
    }

    public void SetPlayerFishInventory(FishInventory inventory)
    {
        fishInventory = inventory;
    }
    public void SetPlayerLoadout(PlayerLoadout inventory)
    {
        playerLoadout = inventory;
    }
    private void FindPlayerFishInventory()
    {
        fishInventory = GameManager.Instance.playerInventory.GetPlayerFishInventory();
    }

    public void CatchFish(FishData fish)
    {
        fishInventory.AddFish(fish); // Add the fish to the inventory
        SaveManager.SavePlayerInventory(this); // Save the inventory
    }

    public void AddMoney(float amount)
    {
        money += amount;
        SaveManager.SavePlayerInventory(this.GetComponent<PlayerInventory>()); // Save after adding money
    }

    public bool SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
            SaveManager.SavePlayerInventory(this.GetComponent<PlayerInventory>()); // Save after spending money
            return true;
        }
        return false;
    }

    public float GetMoney()
    {
        return money;
    }

    public PlayerLoadout GetPlayerLoadout()
    {
        Debug.Log("Tried Saving");
        return playerLoadout;
    }
}

