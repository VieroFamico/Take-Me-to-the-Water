using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 5f;
    public GameObject interactPrompt; // Panel with "Press E to Interact" text
    public Vector3 promptOffset; // Offset for the prompt relative to the player
    public Vector3 buildingDisplayOffset;

    private BuildingManager nearestBuilding;
    private HomePlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<HomePlayerMovement>();
    }

    void Update()
    {
        CheckForBuildingInteraction();
        UpdateInteractPrompt();
    }

    void CheckForBuildingInteraction()
    {
        nearestBuilding = null;
        float nearestDistance = interactionDistance;

        foreach (BuildingManager building in FindObjectsOfType<BuildingManager>())
        {
            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance <= nearestDistance)
            {
                nearestBuilding = building;
                nearestDistance = distance;
            }
        }

        if (nearestBuilding != null && Input.GetKeyDown(KeyCode.E))
        {
            ToggleBuildingDisplay(nearestBuilding);
        }

    }

    void ToggleBuildingDisplay(BuildingManager building)
    {
        bool isActive = building.GetState();

        if (!isActive)
        {
            building.OpenDisplay();
            playerMovement.enabled = false;
        }
        else
        {
            building.CloseDisplay();
            playerMovement.enabled = true;
        }
        //Vector3 temp = transform.position;
        //temp.y += buildingDisplayOffset.y;
        //building.buildingDisplay.transform.position = transform.position;

    }

    void UpdateInteractPrompt()
    {
        if (nearestBuilding != null)
        {
            interactPrompt.SetActive(true);
        }
        else
        {
            interactPrompt.SetActive(false);
        }
        Vector3 temp = transform.position + promptOffset;
        interactPrompt.transform.position = temp;
    }
}