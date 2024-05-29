using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject fishCaughtUI; // Reference to the UI GameObject
    public Animator fishCaughtAnimator; // Reference to the Animator component
    public Image fishImage;
    public TextMeshProUGUI fishName;
    public string leaveTrigger = "Leave"; // Trigger name to end the animation

    private bool isUIActive = false;
    private FishingMechanic fishingMechanic;

    void Start()
    {
        fishingMechanic = GetComponent<FishingMechanic>();
        // Ensure the UI is disabled at start
        fishCaughtUI.SetActive(false);
    }

    void Update()
    {
        // Check for input if the UI is active
        if (isUIActive && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(EndFishCaughtUI());
        }
    }

    public void ShowFishCaughtUI(Sprite fishImageNew, string fishNameNew)
    {
        // Enable the UI and start the show animation
        fishImage.sprite = fishImageNew;
        fishName.text = fishNameNew;
        fishCaughtUI.SetActive(true);
        isUIActive = true;
    }

    private IEnumerator EndFishCaughtUI()
    {
        // Start the leave animation
        fishCaughtAnimator.SetTrigger(leaveTrigger);
        isUIActive = false;
        fishingMechanic.EnableThrowHook();
        // Wait for the animation to finish
        yield return new WaitForSeconds(5f);

        // Disable the UI
        fishCaughtUI.SetActive(false);
    }

    private float GetAnimationClipLength(string clipName)
    {
        foreach (AnimationClip clip in fishCaughtAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0f;
    }
}
