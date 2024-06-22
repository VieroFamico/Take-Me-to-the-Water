using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "ScriptableObjects/FishSO", order = 51)]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite fishSprite;
    public Sprite cardSprite;
    public int price;
    // More stuff to come
}
