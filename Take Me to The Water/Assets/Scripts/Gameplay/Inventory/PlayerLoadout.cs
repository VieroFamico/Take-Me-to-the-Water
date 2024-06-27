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
    public ShipBodySO currentShip;
    public FishingRodSO currentFishingRod;
    public ShipEngineSO currentShipEngine;
    private float currentShipFuel;

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
        currentShipFuel = fuel;
    }
    public void Refuel(float fuelPercentage)
    {
        currentShipFuel = currentShip.shipTimeLimit * (fuelPercentage / 100);
    }
    public float GetFuelPercentage()
    {
        return (currentShipFuel / currentShip.shipTimeLimit) * 100;
    }
    


    // Ship management methods
    public void SetCurrentShipBody(ShipBodySO ship)
    {
        currentShip = ship;
    }

    public ShipBodySO GetCurrentShipBody()
    {
        return currentShip;
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
        currentShipFuel = currentShip.shipTimeLimit;
    }

    public ShipEngineSO GetCurrentShipEngine()
    {
        return currentShipEngine;
    }
}

