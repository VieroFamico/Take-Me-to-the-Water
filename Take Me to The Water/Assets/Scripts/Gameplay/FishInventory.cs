using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    public List<FishData> fishInventory;

    public List<FishData> GetFishInventory()
    {
        return fishInventory;
    }
    public void AddFish(FishData newFish)
    {
        fishInventory.Add(newFish);
    }
}
