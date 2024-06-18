using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    public enum Bait
    {
        None, Worm, Ulat, Cricket, Shrimp, Pelet
    }

    private Bait currentBait = Bait.Worm;
    public Dictionary<Bait, int> baitAmounts = new Dictionary<Bait, int>();
    public ShipSO currentShip;

    void Start()
    {
        // Initialize bait amounts (example values, adjust as needed)
        baitAmounts[Bait.Worm] = 10;
        baitAmounts[Bait.Ulat] = 5;
        baitAmounts[Bait.Cricket] = 8;
        baitAmounts[Bait.Shrimp] = 7;
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

    public void SetCurrentShip(ShipSO ship)
    {
        currentShip = ship;
    }
    
    public ShipSO GetCurrentShip()
    {
        return currentShip;
    }
}

