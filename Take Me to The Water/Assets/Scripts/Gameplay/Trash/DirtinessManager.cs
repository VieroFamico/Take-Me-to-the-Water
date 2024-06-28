using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtinessManager : MonoBehaviour
{
    [Header("Sick Fish Settings")]
    [Range(0f, 1f)]
    private float sickFishChance = 0.1f; // 10% chance for a fish to be sick

    public bool GetSickFishChance()
    {
        sickFishChance = Random.value;

        return Random.value < sickFishChance;
    }
}
