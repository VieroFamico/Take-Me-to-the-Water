using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "ScriptableObjects/FishSO", order = 11)]
public class FishSO : ScriptableObject
{
    public string fishName;
    public Sprite fishSprite;
    public bool isSick;
    public int price;
}
