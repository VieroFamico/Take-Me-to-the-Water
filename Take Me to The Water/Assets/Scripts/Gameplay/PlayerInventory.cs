using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public float money = 100f; // Starting money for the player
    public PlayerLoadout playerLoadout;

    private FishInventory fishInventory;
    private TrashInventory trashInventory;

    private DisplayManager displayManager;
    private int flag;

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
        FindPlayerInventory();
        FindDisplayManager();
    }
    private void Update()
    {
        if(flag == 0)
        {
            FindPlayerInventory();
            FindDisplayManager();
            flag++;
        }
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
        FindPlayerInventory();
        FindDisplayManager();
    }

    public FishInventory GetPlayerFishInventory()
    {
        return fishInventory;
    }
    public TrashInventory GetPlayerTrashInventory()
    {
        return trashInventory;
    }

    public void SetPlayerFishInventory(FishInventory inventory)
    {
        fishInventory = inventory;
    }
    public void SetPlayerTrashInventory(TrashInventory inventory)
    {
        trashInventory = inventory;
    }
    public void SetPlayerLoadout(PlayerLoadout inventory)
    {
        playerLoadout = inventory;
    }
    private void FindPlayerInventory()
    {
        fishInventory = GameManager.Instance.playerInventory.GetPlayerFishInventory();
        trashInventory = GameManager.Instance.playerInventory.GetPlayerTrashInventory();
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
    public void AddTrash(TrashSO newTrash)
    {
        trashInventory.AddTrash(newTrash);
        Debug.Log("AddTrash");
        Debug.Log(newTrash);
        SaveManager.SavePlayerInventory(this); // Save the inventory
    }

    // Recycle trash into materials
    public void RecycleTrash(TrashSO trash)
    {
        trashInventory.RecycleTrash(trash);
        SaveManager.SavePlayerInventory(this); // Save the inventory
    }

    public float GetMoney()
    {
        return money;
    }

    public PlayerLoadout GetPlayerLoadout()
    {
        return playerLoadout;
    }
    public DisplayManager GetDisplayManager()
    {
        return displayManager;
    }
}

