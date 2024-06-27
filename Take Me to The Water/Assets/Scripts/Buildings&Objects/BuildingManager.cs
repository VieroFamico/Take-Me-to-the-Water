using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingDisplay;
    public float animationDuration = 0.2f;
    public Button closeButton;

    private Coroutine currentCoroutine;
    private HomePlayerMovement playerMovement;
    private bool isOpen = false;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<HomePlayerMovement>();
    }
    void Start()
    {
        closeButton.onClick.AddListener(CloseDisplay);
    }
    public bool GetState()
    {
        return isOpen;
    }
    public void ChangeState()
    {
        isOpen = !isOpen;
    }
    public virtual void OpenDisplay()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ScalePanel(buildingDisplay, Vector3.zero, Vector3.one, animationDuration));
        BlurEffectForPanel.ToggleBlur();
        ChangeState();
        playerMovement.ChangeMove();
        playerMovement.enabled = false;
    }
    public virtual void CloseDisplay()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ScalePanel(buildingDisplay, Vector3.one, Vector3.zero, animationDuration));
        BlurEffectForPanel.ToggleBlur();
        ChangeState();
        playerMovement.ChangeMove();
        playerMovement.enabled = true;
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
