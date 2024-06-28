using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtinessManager : MonoBehaviour
{
    [Header("Sick Fish Settings")]
    [Range(0f, 1f)]
    public float sickFishChance = 0.1f; // 10% chance for a fish to be sick

    public bool GetSickFishChance()
    {
        return Random.value < sickFishChance;
    }
}
