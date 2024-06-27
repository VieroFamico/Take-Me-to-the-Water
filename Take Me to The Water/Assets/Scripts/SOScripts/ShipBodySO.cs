using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipSO", menuName = "ScriptableObjects/ShipBodySO", order = 1)]
public class ShipBodySO : ScriptableObject
{
    public string toolsName;
    public Sprite sprite;
    public int tier;
    public float shipTimeLimit; // In Minutes
    public int price;
    [Header("MaterialNeeded")]
    public int plasticNeeded;
    public int metalNeeded;
    public int woodNeeded;
    public int rubberNeeded;
}
