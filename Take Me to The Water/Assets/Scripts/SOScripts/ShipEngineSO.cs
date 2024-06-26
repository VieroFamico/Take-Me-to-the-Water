using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Engine", menuName = "ScriptableObjects/ShipEngineSO", order = 1)]
public class ShipEngineSO : ScriptableObject
{
    public string engineName;
    public int engineTier; 
    public Sprite engineSprite;
    public float movementSpeed;
    public int price;
    [Header("MaterialNeeded")]
    public int plasticNeeded;
    public int metalNeeded;
    public int woodNeeded;
    public int rubberNeeded;
}