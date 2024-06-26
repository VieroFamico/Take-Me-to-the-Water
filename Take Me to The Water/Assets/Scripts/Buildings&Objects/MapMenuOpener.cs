using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuOpener : BuildingManager
{
    public Animator mapMenuAnimator;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseDisplay);
    }
    public override void OpenDisplay()
    {
        Debug.Log("Open");
        Debug.Log(mapMenuAnimator.parameterCount);
        mapMenuAnimator.SetTrigger("Show");
        BlurEffectForPanel.ToggleBlur();
        ChangeState();
    }
    public override void CloseDisplay()
    {
        Debug.Log("Close");
        mapMenuAnimator.SetTrigger("Hide");
        BlurEffectForPanel.ToggleBlur();
        ChangeState();
    }
}
