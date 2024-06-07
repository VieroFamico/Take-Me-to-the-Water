using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerInventory playerInventory; // Player's inventory including money
    public FishInventory shopFishInventory; // Shop's fish inventory
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventories();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveInventories();
    }

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void LoadInventories()
    {
        PlayerInventoryWrapper playerData = SaveManager.LoadPlayerInventory();
        if (playerData != null)
        {
            playerInventory.money = playerData.money;
            FishInventory fishInventory = new FishInventory();
            playerInventory.SetFishInventory(fishInventory);
            SaveManager.SavePlayerInventory(playerInventory);
        }
        else
        {
            SaveManager.SavePlayerInventory(playerInventory);
        }

        shopFishInventory = SaveManager.LoadFishInventory("ShopInventory.json");
        if (shopFishInventory == null)
        {
            shopFishInventory = new FishInventory();
        }
    }

    private void SaveInventories()
    {
        SaveManager.SavePlayerInventory(playerInventory);
        SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json");
    }
}

