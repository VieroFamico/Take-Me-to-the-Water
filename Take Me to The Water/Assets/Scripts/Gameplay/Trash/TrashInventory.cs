using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrashInventory : MonoBehaviour
{
    public List<TrashSO> trashInventory = new List<TrashSO>();
    public Dictionary<TrashSO, int> recycledMaterials = new Dictionary<TrashSO, int>();

    public List<TrashSO> GetTrashList()
    {
        Debug.Log("Get trash list");
        return trashInventory;
    }

    public void AddTrash(TrashSO newTrash)
    {
        trashInventory.Add(newTrash);
    }
    public void RemoveTrash(TrashSO trash)
    {
        trashInventory.Remove(trash);
    }
    public void SetTrashList(List<TrashSO> inventory)
    {
        trashInventory = inventory;
    }
}
