using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingDisplay;
    public float animationDuration = 1.0f;
    public Button closeButton;

    private Coroutine currentCoroutine;

    virtual public void OpenDisplay()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ScalePanel(buildingDisplay, Vector3.zero, Vector3.one, animationDuration));
        BlurEffectForPanel.ToggleBlur();
    }
    virtual public void CloseDisplay()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ScalePanel(buildingDisplay, Vector3.one, Vector3.zero, animationDuration));
        BlurEffectForPanel.ToggleBlur();
    }
    private IEnumerator ScalePanel(GameObject panel, Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsedTime = 0;
        panel.transform.localScale = startScale;
        panel.SetActive(true);

        while (elapsedTime < duration)
        {
            panel.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.transform.localScale = endScale;

        if (endScale == Vector3.zero)
        {
            panel.SetActive(false);
        }

        currentCoroutine = null;
    }

}
