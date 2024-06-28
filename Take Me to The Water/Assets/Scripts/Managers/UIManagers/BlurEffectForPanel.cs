using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;

public class BlurEffectForPanel : MonoBehaviour
{
    private VolumeProfile profile;
    private DepthOfField depthOfField;

    private static BlurEffectForPanel _instance;

    private void Awake()
    {
        _instance = this;
        profile = GetComponent<Volume>().profile;
    }
    public static void ToggleBlur()
    {
        if (_instance != null)
        {
            _instance.ChangeBlur();
        }
        else
        {
            Debug.LogError("BlurEffectForPanel instance is not set. Make sure the script is attached to a GameObject in the scene.");
        }
    }

    public void ChangeBlur()
    {
        if (profile.TryGet(out depthOfField))
        {
            if (!depthOfField.active)
            {
                ActivateBlur();
            }
            else
            {
                DeactivateBlur();
            }
        }
    }

    private void ActivateBlur()
    {
        if (profile.TryGet(out depthOfField))
        {
            depthOfField.active = true;
        }
    }

    private void DeactivateBlur()
    {
        if (profile.TryGet(out depthOfField))
        {
            depthOfField.active = false;
        }
    }
}