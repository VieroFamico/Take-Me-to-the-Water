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
        Debug.Log(playerData.money);
        Debug.Log(playerData.fishInventory);
        if (playerData != null)
        {
            playerInventory.money = playerData.money;

            FishInventory fishInventory = new FishInventory();
            fishInventory.SetFishList(playerData.fishInventory);
            playerInventory.SetPlayerFishInventory(fishInventory);

            SaveManager.SavePlayerInventory(playerInventory);
            Debug.Log("Save If Not Null");
        }
        else
        {
            SaveManager.SavePlayerInventory(playerInventory);
            Debug.Log("Save And Is Null");
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

