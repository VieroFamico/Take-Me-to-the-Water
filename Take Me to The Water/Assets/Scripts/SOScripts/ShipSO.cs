using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipSO", menuName = "ScriptableObjects/ShipSO", order = 1)]
public class ShipSO : ScriptableObject
{
    public string shipName;
    public Sprite shipSprite;
    public float shipTimeLimit;
    // More to come
}
