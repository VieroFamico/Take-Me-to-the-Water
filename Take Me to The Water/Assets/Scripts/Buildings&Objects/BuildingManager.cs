using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingDisplay;

    virtual public void OpenDisplay()
    {
        buildingDisplay.SetActive(true);
    }
    virtual public void CloseDisplay()
    {
        buildingDisplay.SetActive(false);
    }
}
