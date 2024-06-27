using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodCrafting : CraftingManager
{
    public List<FishingRodSO> fishingRodList;

    protected override void SetItemToCraft()
    {
        var currentRod = playerLoadout.GetCurrentFishingRod();
        currentTier = currentRod != null ? currentRod.tier : 0;
        itemToCraft = fishingRodList.Find(rod => rod.tier == currentTier + 1);
        UpdateUI();
    }

    public void SetFishingRodList(List<FishingRodSO> rodList)
    {
        fishingRodList = rodList;
        Initialize();
    }
}
