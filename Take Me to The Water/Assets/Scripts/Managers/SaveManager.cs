using System;
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
    public List<FishData> fishList;
    public PlayerLoadoutWrapper playerLoadout;
    public List<TrashSO> trashList;
    public int plasticAmount;
    public int metalAmount;
    public int woodAmount;
    public int rubberAmount;

}

[System.Serializable]
public class PlayerLoadoutWrapper
{
    public PlayerLoadout.Bait currentBait;
    public Dictionary<PlayerLoadout.Bait, int> baitAmounts;
    public string currentShipName;
    public byte[] currentShipSpriteData;
    public float currentShipTimeLimit;
    public float currentCurrentTimeLimit;
}

public static class SaveManager
{
    private static string playerSavePath = Application.persistentDataPath + "/PlayerInventory.json";
    private static string shopSavePath = Application.persistentDataPath + "/ShopInventory.json";

    public static void SavePlayerInventory(PlayerInventory playerInventory)
    {
        PlayerLoadout playerLoadout = playerInventory.GetPlayerLoadout();
        ShipSO currentShip = playerLoadout.GetCurrentShip();

        // Create a PlayerLoadoutWrapper instance
        PlayerLoadoutWrapper playerLoadoutWrapper = new PlayerLoadoutWrapper
        {
            currentBait = playerLoadout.GetCurrentBait(),
            baitAmounts = new Dictionary<Bait, int>(playerLoadout.GetBaitAmounts()),
            currentShipName = currentShip != null ? currentShip.shipName : "BaseShip",
            currentShipSpriteData = currentShip != null && currentShip.shipSprite != null ? SpriteToByteArray(currentShip.shipSprite) : null,
            currentShipTimeLimit = currentShip != null ? currentShip.shipTimeLimit : 5,
            currentCurrentTimeLimit = currentShip!= null ? currentShip.currentTimeLimit : 0
        };

        // Create a PlayerInventoryWrapper instance
        PlayerInventoryWrapper playerInventoryWrapper = new PlayerInventoryWrapper
        {
            money = playerInventory.money,
            fishList = playerInventory.GetPlayerFishInventory().GetFishList(),
            playerLoadout = playerLoadoutWrapper,
            trashList = playerInventory.GetPlayerTrashInventory().GetTrashList(),
            plasticAmount = playerInventory.GetPlayerTrashInventory().GetPlasticAmount(),
            metalAmount = playerInventory.GetPlayerTrashInventory().GetMetalAmount(),
            woodAmount = playerInventory.GetPlayerTrashInventory().GetWoodAmount(),
            rubberAmount = playerInventory.GetPlayerTrashInventory().GetRubberAmount(),
        };

        // Serialize the PlayerInventoryWrapper to JSON
        string json = JsonUtility.ToJson(playerInventoryWrapper);
        Debug.Log(json);
        File.WriteAllText(playerSavePath, json);
    }


    public static PlayerInventoryWrapper LoadPlayerInventory()
    {
        string path = playerSavePath;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log(json);
            return JsonUtility.FromJson<PlayerInventoryWrapper>(json);
        }
        return null;
    }

    public static void SaveFishInventory(FishInventory inventory, string fileName)
    {
        FishInventoryWrapper wrapper = new FishInventoryWrapper { fishInventory = inventory.GetFishList() };
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
    public static byte[] SpriteToByteArray(Sprite sprite)
    {
        if (sprite == null) return null;

        try
        {
            Texture2D texture = sprite.texture;
            byte[] bytes = texture.EncodeToPNG(); // Encode the texture to a PNG byte array
            return bytes;
        }
        catch (Exception e)
        {
            Debug.LogError("Error converting sprite to byte array: " + e);
            return null;
        }
    }

    public static Sprite ByteArrayToSprite(byte[] data)
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(data);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
/*    private static List<TrashWrapper> GetTrashWrappers(List<Trash> trashList)
    {
        List<TrashWrapper> wrappers = new List<TrashWrapper>();
        foreach (var trash in trashList)
        {
            TrashWrapper wrapper = new TrashWrapper
            {
                recyclableMaterials = new Dictionary<Trash.MaterialType, int>(trash.recyclableMaterials)
            };
            wrappers.Add(wrapper);
        }
        return wrappers;
    }*/

}
