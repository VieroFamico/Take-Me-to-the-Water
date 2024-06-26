using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuOpener : MonoBehaviour
{
    public GameObject player;
    public GameObject goFishingPanel;

    public Animator mapMenuAnimator;
    public float proximityDistance = 5f;

    private bool isNearShip = false;
    private bool mapIsShowing = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); // Assuming the player has the tag "Player"
        goFishingPanel.SetActive(false); // Ensure the button is initially inactive
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= proximityDistance)
            {
                if (!isNearShip)
                {
                    isNearShip = true;
                    goFishingPanel.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!mapIsShowing)
                    {
                        ShowMapMenu();
                    }
                    else
                    {
                        HideMapMenu();
                    }
                    mapIsShowing = !mapIsShowing;
                }
            }
            else
            {
                if (isNearShip)
                {
                    isNearShip = false;
                    goFishingPanel.SetActive(false);
                }
            }
        }
    }

    public void ShowMapMenu()
    {
        mapMenuAnimator.SetTrigger("Show");
        BlurEffectForPanel.ToggleBlur();
    }
    public void HideMapMenu()
    {
        mapMenuAnimator.SetTrigger("Hide");
        BlurEffectForPanel.ToggleBlur();
    }
}
