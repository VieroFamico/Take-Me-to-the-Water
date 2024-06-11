using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class FishInventory
{
    private List<FishData> fishInventory = new List<FishData>();

    public List<FishData> GetFishList()
    {
        return fishInventory;
    }

    public void SetFishList(List<FishData> inventory)
    {
        fishInventory = inventory;
    }

    public void AddFish(FishData newFish)
    {
        fishInventory.Add(newFish);
    }
    public void RemoveFish(FishData fish)
    {
        fishInventory.Remove(fish);
    }

    public FishData GetRandomFish()
    {
        if (fishInventory.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, fishInventory.Count);
        return fishInventory[randomIndex];
    }
}
