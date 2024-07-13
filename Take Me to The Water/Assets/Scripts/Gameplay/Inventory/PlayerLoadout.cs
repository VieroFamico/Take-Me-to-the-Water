using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    public enum Bait
    {
        None, Worm, Cricket, Pelet
    }

    private Bait currentBait = Bait.Worm;
    public Dictionary<Bait, int> baitAmounts = new();
    public ShipBodySO currentShipBody;
    public FishingRodSO currentFishingRod;
    public ShipEngineSO currentShipEngine;
    private float currentShipFuel = 0; // In Minutes

    void Start()
    {
        // Initialize bait amounts (example values, adjust as needed)
        baitAmounts[Bait.Worm] = 10;
        baitAmounts[Bait.Cricket] = 8;
        baitAmounts[Bait.Pelet] = 3;
    }

    public Bait GetCurrentBait()
    {
        return currentBait;
    }

    public void SelectBait(Bait baitType)
    {
        currentBait = baitType;
    }

    public int GetBaitAmount(Bait baitType)
    {
        return baitAmounts.ContainsKey(baitType) ? baitAmounts[baitType] : 0;
    }
    public Dictionary<Bait, int> GetBaitAmounts()
    {
        return baitAmounts;
    }

    public void IncreaseBaitAmount(Bait baitType, int amount)
    {
        if (baitAmounts.ContainsKey(baitType))
        {
            baitAmounts[baitType] += amount;
        }
    }

    public void DecreaseBaitAmount(Bait baitType, int amount)
    {
        if (baitAmounts.ContainsKey(baitType) && baitAmounts[baitType] > 0)
        {
            baitAmounts[baitType] = Mathf.Max(baitAmounts[baitType] - amount, 0);
        }
    }
    public void SetCurrentBait(Bait baitType)
    {
        currentBait = baitType;
    }

    public void SetBaitAmounts(Dictionary<Bait, int> baitAmounts)
    {
        this.baitAmounts = baitAmounts;
    }

    public float GetCurrentShipFuel()
    {
        return currentShipFuel;
    }
    public void SetCurrentShipFuel(float fuel)
    {
        if(fuel < 0)
        {
            currentShipFuel = 0;
            SaveManager.SavePlayerInventory(GetComponent<PlayerInventory>());
            return;
        }
        else if(fuel > currentShipBody.shipTimeLimit)
        {
            currentShipFuel = currentShipBody.shipTimeLimit;
            SaveManager.SavePlayerInventory(GetComponent<PlayerInventory>());
            return;

        }
        currentShipFuel = fuel;
        SaveManager.SavePlayerInventory(GetComponent<PlayerInventory>());
    }
    public void Refuel(float fuelPercentage)
    {
        currentShipFuel = currentShipBody.shipTimeLimit * (fuelPercentage / 100);
    }
    public float GetFuelPercentage()
    {
        Debug.Log(currentShipBody);
        Debug.Log(currentShipBody.shipTimeLimit);
        return (currentShipFuel / currentShipBody.shipTimeLimit) * 100;
    }

    // Ship management methods
    public void SetCurrentShipBody(ShipBodySO ship)
    {
        currentShipBody = ship;
    }

    public ShipBodySO GetCurrentShipBody()
    {
        return currentShipBody;
    }

    // Fishing rod management methods
    public void SetCurrentFishingRod(FishingRodSO fishingRod)
    {
        currentFishingRod = fishingRod;
    }

    public FishingRodSO GetCurrentFishingRod()
    {
        return currentFishingRod;
    }

    // Ship engine management methods
    public void SetCurrentShipEngine(ShipEngineSO shipEngine)
    {
        currentShipEngine = shipEngine;
    }

    public ShipEngineSO GetCurrentShipEngine()
    {
        return currentShipEngine;
    }
}

