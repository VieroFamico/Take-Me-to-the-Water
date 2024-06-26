using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class FishInventory
{
    private List<FishSO> fishList = new List<FishSO>();

    public List<FishSO> GetFishList()
    {
        return fishList;
    }

    public void SetFishList(List<FishSO> inventory)
    {
        fishList = inventory;
    }

    public void AddFish(FishSO newFish)
    {
        fishList.Add(newFish);
    }
    public void RemoveFish(FishSO fish)
    {
        fishList.Remove(fish);
    }

    public FishSO GetRandomFish()
    {
        if (fishList.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, fishList.Count);
        return fishList[randomIndex];
    }
}
