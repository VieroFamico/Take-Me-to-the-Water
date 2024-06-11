using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipSO", menuName = "ScriptableObjects/ShipSO", order = 1)]
public class ShipSO : ScriptableObject
{
    public string shipName;
    public Sprite shipSprite;
    public float shipTimeLimit;
    public float currentTimeLimit; // Current fuel level in minutes

    public void Refuel(float fuelPercentage)
    {
        currentTimeLimit = shipTimeLimit * (fuelPercentage / 100);
    }
    public float GetFuelPercentage()
    {
        return (currentTimeLimit / shipTimeLimit) * 100;
    }
}
