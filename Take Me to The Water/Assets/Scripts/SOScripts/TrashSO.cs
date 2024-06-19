using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrashSO", menuName = "ScriptableObjects/TrashSO", order = 2)]
public class TrashSO : ScriptableObject
{
    public int plasticAmount;
    public int metalAmount;
    public int woodAmount;
    public int rubberAmount;
}
