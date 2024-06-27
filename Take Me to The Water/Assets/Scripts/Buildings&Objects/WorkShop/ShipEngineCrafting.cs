using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngineCrafting : CraftingManager
{
    public List<ShipEngineSO> shipEngineList;

    protected override void SetItemToCraft()
    {
        var currentEngine = playerLoadout.GetCurrentShipEngine();
        currentTier = currentEngine != null ? currentEngine.tier : 0;
        itemToCraft = shipEngineList.Find(engine => engine.tier == currentTier + 1);
        UpdateUI();
    }

    public void SetShipEngineList(List<ShipEngineSO> engineList)
    {
        shipEngineList = engineList;
        Initialize();
    }
}
