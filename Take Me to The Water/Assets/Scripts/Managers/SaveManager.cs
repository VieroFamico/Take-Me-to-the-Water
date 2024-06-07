using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class FishInventoryWrapper
{
    public List<FishData> fishInventory;
}

public static class SaveManager
{
    private static string savePath = Application.persistentDataPath + "/";

    public static void SaveFishInventory(FishInventory inventory, string fileName)
    {
        FishInventoryWrapper wrapper = new FishInventoryWrapper { fishInventory = inventory.GetFishInventory() };
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(savePath + fileName, json);
    }

    public static FishInventory LoadFishInventory(string fileName)
    {
        string path = savePath + fileName;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            FishInventoryWrapper wrapper = JsonUtility.FromJson<FishInventoryWrapper>(json);
            FishInventory inventory = new FishInventory();
            inventory.SetFishInventory(wrapper.fishInventory);
            return inventory;
        }
        return null;
    }
}