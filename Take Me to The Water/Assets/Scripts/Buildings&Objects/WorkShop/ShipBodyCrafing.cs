using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBodyCrafing : CraftingManager
{
    public List<ShipBodySO> shipBodyList;

    protected override void SetItemToCraft()
    {
        var currentShip = playerLoadout.GetCurrentShipBody();
        currentTier = currentShip != null ? currentShip.tier : 0;
        itemToCraft = shipBodyList.Find(body => body.tier == currentTier + 1);
        UpdateUI();
    }

    public void SetShipBodyList(List<ShipBodySO> bodyList)
    {
        shipBodyList = bodyList;
        Initialize();
    }

}
