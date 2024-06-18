using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public int plasticAmount;
    public int metalAmount;
    public int woodAmount;
    public int rubberAmount;

    public enum MaterialType
    {
        Plastic,
        Metal,
        Wood,
        Rubber
    }

    public Dictionary<MaterialType, int> recyclableMaterials = new Dictionary<MaterialType, int>();

    // Example initialization of the trash object with materials
    void Start()
    {
        // Example values, these can be set dynamically as needed
        recyclableMaterials[MaterialType.Plastic] = plasticAmount;
        recyclableMaterials[MaterialType.Metal] = metalAmount;
        recyclableMaterials[MaterialType.Wood] = woodAmount;
        recyclableMaterials[MaterialType.Rubber] = rubberAmount;
    }
}
