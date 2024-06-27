using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fishing Rod", menuName = "ScriptableObjects/FishingRodSO", order = 1)]
public class FishingRodSO : ScriptableObject
{
    public string rodName;
    public int tier;
    public Sprite rodSprite;
    public float maxTension;
    public int price;
    [Header("MaterialNeeded")]
    public int plasticNeeded;
    public int metalNeeded;
    public int woodNeeded;
    public int rubberNeeded;
}