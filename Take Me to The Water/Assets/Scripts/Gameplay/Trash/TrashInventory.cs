using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class TrashInventory
{
    public List<TrashSO> trashList = new List<TrashSO>();

    private int plasticAmount;
    private int metalAmount;
    private int woodAmount;
    private int rubberAmount;

    public List<TrashSO> GetTrashList()
    {
        return trashList;
    }

    public void AddTrash(TrashSO newTrash)
    {
        trashList.Add(newTrash);
    }
    public void RecycleTrash(TrashSO trash)
    {
        plasticAmount += trash.plasticAmount;
        metalAmount += trash.metalAmount;
        woodAmount += trash.woodAmount;
        rubberAmount += trash.rubberAmount;

        RemoveTrash(trash);
    }
    public void RemoveTrash(TrashSO trash)
    {
        trashList.Remove(trash);
    }
    public void SetTrashList(List<TrashSO> inventory)
    {
        trashList = inventory;
    }

    public int GetPlasticAmount()
    {
        return plasticAmount;
    }
    public int GetMetalAmount()
    {
        return metalAmount;
    }
    public int GetWoodAmount()
    {
        return woodAmount;
    }
    public int GetRubberAmount()
    {
        return rubberAmount;
    }

    public void SetMaterials(int plastic, int metal, int wood, int rubber)
    {
        plasticAmount = plastic;
        metalAmount = metal;
        woodAmount = wood;
        rubberAmount = rubber;
    }
    public void SpendPlastic(int amountSpent)
    {
        plasticAmount -= amountSpent;
    }
    public void SpendMetal (int amountSpent)
    {
        metalAmount -= amountSpent;
    }
    public void SpendWood(int amountSpent)
    {
        woodAmount -= amountSpent;
    }
    public void SpendRubber(int amountSpent)
    {
        rubberAmount -= amountSpent;
    }
}
