using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookManager : MonoBehaviour
{
    private Fish targetFish;

    public Fish TargetFish
    {
        get { return targetFish; }
        set
        {
            if (targetFish != null)
            {
                targetFish.StopTargetingHook();
            }
            targetFish = value;
        }
    }

    public bool IsTargeted
    {
        get { return targetFish != null; }
    }

    public void ReleaseTarget()
    {
        targetFish = null;
    }

}
