using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static PlayerLoadout;

[System.Serializable]
public class FishInventoryWrapper
{
    public List<FishData> fishInventory;
}
[System.Serializable]
public class PlayerInventoryWrapper
{
    public float money;
    public List<FishData> fishInventory;
    public PlayerLoadoutWrapper playerLoadout;
}

[System.Serializable]
public class PlayerLoadoutWrapper
{
    public Bait currentBait;
    public Dictionary<Bait, int> baitAmounts;
    public string currentShipName;
    public string currentShipSpritePath;
    public float currentShipTimeLimit;
}

public static class SaveManager
{
    private static string playerSavePath = Application.persistentDataPath + "/PlayerInventory.json";
    private static string shopSavePath = Application.persistentDataPath + "/ShopInventory.json";

    public static void SavePlayerInventory(PlayerInventory playerInventory)
    {
        Debug.Log("Tried Saving");
        PlayerInventoryWrapper wrapper = new PlayerInventoryWrapper
        {
            money = playerInventory.money,
            fishInventory = playerInventory.GetPlayerFishInventory().GetFishInventory(),
            playerLoadout = new PlayerLoadoutWrapper
            {
                currentBait = playerInventory.GetPlayerLoadout().GetCurrentBait(),
                baitAmounts = new Dictionary<Bait, int>(),
                currentShipName = playerInventory.GetPlayerLoadout().currentShip.shipName,
                currentShipSpritePath = playerInventory.GetPlayerLoadout().currentShip.shipSprite.name,
                currentShipTimeLimit = playerInventory.GetPlayerLoadout().currentShip.shipTimeLimit
            }
        };

        foreach (var bait in playerInventory.GetPlayerLoadout().baitAmounts)
        {
            wrapper.playerLoadout.baitAmounts[bait.Key] = bait.Value;
        }

        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(playerSavePath, json);
    }

    public static PlayerInventoryWrapper LoadPlayerInventory()
    {
        if (File.Exists(playerSavePath))
        {
            string json = File.ReadAllText(playerSavePath);
            PlayerInventoryWrapper wrapper = JsonUtility.FromJson<PlayerInventoryWrapper>(json);
            return wrapper;
        }
        return null;
    }

    public static void SaveFishInventory(FishInventory inventory, string fileName)
    {
        FishInventoryWrapper wrapper = new FishInventoryWrapper { fishInventory = inventory.GetFishInventory() };
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }

    public static FishInventory LoadFishInventory(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            FishInventoryWrapper wrapper = JsonUtility.FromJson<FishInventoryWrapper>(json);
            FishInventory inventory = new FishInventory();
            inventory.SetFishList(wrapper.fishInventory);
            return inventory;
        }
        return null;
    }
}
