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
        mapMenuAnimator.gameObject.SetActive(true);
        mapMenuAnimator.SetTrigger("Show");
        BlurEffectForPanel.ToggleBlur();
        ChangeState();
    }
    public override void CloseDisplay()
    {
        mapMenuAnimator.SetTrigger("Hide");
        BlurEffectForPanel.ToggleBlur();
        ChangeState();
    }
}
