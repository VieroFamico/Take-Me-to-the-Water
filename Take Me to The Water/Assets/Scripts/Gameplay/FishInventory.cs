using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class FishInventory
{
    private List<FishData> fishList = new List<FishData>();

    public List<FishData> GetFishList()
    {
        return fishList;
    }

    public void SetFishList(List<FishData> inventory)
    {
        fishList = inventory;
    }

    public void AddFish(FishData newFish)
    {
        fishList.Add(newFish);
    }
    public void RemoveFish(FishData fish)
    {
        fishList.Remove(fish);
    }

    public FishData GetRandomFish()
    {
        if (fishList.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, fishList.Count);
        return fishList[randomIndex];
    }
}
