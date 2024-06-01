using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeShip : MonoBehaviour
{
    public Animator mapMenuAnimator;
    public float proximityDistance = 5f;
    private bool isNearShip = false;
    private GameObject player;
    private GameObject goFishingPanel;

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); // Assuming the player has the tag "Player"
        goFishingPanel = transform.Find("GoFishingPanel").gameObject; // Get the child named "GoFishingButton"
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
                    ShowMapMenu();
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
    }

}
