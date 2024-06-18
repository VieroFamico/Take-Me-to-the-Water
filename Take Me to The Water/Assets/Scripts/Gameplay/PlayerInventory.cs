using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public float money = 100f; // Starting money for the player
    public PlayerLoadout playerLoadout;
    public List<Trash> trashInventory = new List<Trash>();
    public Dictionary<Trash.MaterialType, int> recycledMaterials = new Dictionary<Trash.MaterialType, int>();

    private FishInventory fishInventory;
    
    private DisplayManager displayManager;

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
        FindPlayerFishInventory();
        FindDisplayManager();
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
        FindDisplayManager();
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
    private void FindDisplayManager()
    {
        displayManager = FindAnyObjectByType<DisplayManager>();
    }
    public void CatchFish(FishData fish)
    {
        fishInventory.AddFish(fish); // Add the fish to the inventory
        SaveManager.SavePlayerInventory(this); // Save the inventory
    }

    public void AddMoney(float amount)
    {
        money += amount;
        displayManager.UpdateDisplay();
        displayManager.ShowMoneyChange(amount);
        SaveManager.SavePlayerInventory(this); // Save after adding money
    }

    public bool SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
            displayManager.UpdateDisplay();
            displayManager.ShowMoneyChange(-amount);
            SaveManager.SavePlayerInventory(this); // Save after spending money
            return true;
        }
        return false;
    }
    // Method to purchase fuel
    public bool PurchaseFuel(float fuelPercentage)
    {
        float cost = fuelPercentage; // 1% fuel costs $1
        if (SpendMoney(cost))
        {
            SaveManager.SavePlayerInventory(this);
            return true;
        }
        return false;
    }
    public void AddTrash(Trash newTrash)
    {
        trashInventory.Add(newTrash);
        SaveManager.SavePlayerInventory(this); // Save the inventory
    }

    // Recycle trash into materials
    public void RecycleTrash(Trash trash)
    {
        foreach (var material in trash.recyclableMaterials)
        {
            recycledMaterials[material.Key] += material.Value;
        }
        trashInventory.Remove(trash);
        Destroy(trash.gameObject); // Optionally destroy the trash game object after recycling
        SaveManager.SavePlayerInventory(this); // Save the inventory
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
    public DisplayManager GetDisplayManager()
    {
        return displayManager;
    }
}

