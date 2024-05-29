using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static FishingMechanic;

public class PlayerLoadout : MonoBehaviour
{
    public enum Bait
    {
        None, Worm, Ulat, Cricket, Shrimp, Pelet
    }

    private Bait currentBait = Bait.Worm;
    private Dictionary<Bait, int> baitAmounts = new Dictionary<Bait, int>();

    void Start()
    {
        // Initialize bait amounts (example values, adjust as needed)
        baitAmounts[Bait.Worm] = 10;
        baitAmounts[Bait.Ulat] = 5;
        baitAmounts[Bait.Cricket] = 8;
        baitAmounts[Bait.Shrimp] = 7;
        baitAmounts[Bait.Pelet] = 3;
    }

    void Update()
    {
        // Update logic if needed
    }

    public Bait CurrentBait()
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
}

