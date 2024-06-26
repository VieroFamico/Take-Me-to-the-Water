using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrashSO", menuName = "ScriptableObjects/TrashSO", order = 1)]
public class TrashSO : ScriptableObject
{
    public string trashName;
    public int plasticAmount;
    public int metalAmount;
    public int woodAmount;
    public int rubberAmount;
    public Sprite trashImage;
}
