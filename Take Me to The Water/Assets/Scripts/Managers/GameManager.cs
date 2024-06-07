using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FishInventory fishInventory; // Player's fish inventory
    public FishInventory shopFishInventory; // Shop's fish inventory
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadFishInventories();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveFishInventories();
    }

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void LoadFishInventories()
    {
        fishInventory = SaveManager.LoadFishInventory("PlayerInventory.json");
        if (fishInventory == null)
        {
            fishInventory = new FishInventory();
        }

        shopFishInventory = SaveManager.LoadFishInventory("ShopInventory.json");
        if (shopFishInventory == null)
        {
            shopFishInventory = new FishInventory();
        }
    }

    private void SaveFishInventories()
    {
        SaveManager.SaveFishInventory(fishInventory, "PlayerInventory.json");
        SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json");
    }
}
