using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipSO", menuName = "ScriptableObjects/ShipBodySO", order = 1)]
public class ShipBodySO : ScriptableObject
{
    public string shipName;
    public Sprite shipSprite;
    public int shipTier;
    public float shipTimeLimit; // In Minutes
    public float currentTimeLimit; // Current fuel level in minutes
    public int price;
    [Header("MaterialNeeded")]
    public int plasticNeeded;
    public int metalNeeded;
    public int woodNeeded;
    public int rubberNeeded;

    public void Refuel(float fuelPercentage)
    {
        currentTimeLimit = shipTimeLimit * (fuelPercentage / 100);
    }
    public float GetFuelPercentage()
    {
        return (currentTimeLimit / shipTimeLimit) * 100;
    }
}
