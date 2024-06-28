using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ShipStateManager;

public class InitializeFishing : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public ShipStateManager shipStateManager;
    public BoatMovement boatMovement;
    public FishingMechanic fishingMechanic;
    public GameObject finishFishingPanel;
    public GameObject pausePanel;

    public Button returnHomeButton;
    public Button abortFishingButton;
    private PlayerLoadout playerLoadout;

    private float fishingLenght = 5f;
    private float currTime = 0f;
    private bool isPausing = false;
    private bool isOutOfFuel = false;
    private void Start()
    {
        finishFishingPanel.SetActive(false);
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        playerLoadout  = playerInventory.GetPlayerLoadout();

        boatMovement.SetSpeed(playerLoadout.GetCurrentShipEngine().movementSpeed);
        fishingMechanic.SetMaxTension(playerLoadout.GetCurrentFishingRod().maxTension);
        fishingLenght = playerLoadout.GetCurrentShipFuel();

        returnHomeButton.onClick.AddListener(ReturnToHome);
        abortFishingButton.onClick.AddListener(ReturnToHome);
    }

    private void Update()
    {
        if(currTime >= fishingLenght && !isOutOfFuel)
        {
            EndFishing();
            isOutOfFuel = true;
        }
        else
        {
            currTime += Time.deltaTime;
            playerLoadout.SetCurrentShipFuel(fishingLenght - currTime);
        }

        if(!isOutOfFuel && Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPausing)
            {
                StartPause();
            }
            else
            {
                EndPause();
            }
        }
    }

    private void StartPause()
    {
        finishFishingPanel.SetActive(true);

        shipStateManager.SetMode(Modes.Moving);
        shipStateManager.enabled = false;
        boatMovement.enabled = false;
        FindAnyObjectByType<FishingMechanic>().enabled = false;
        FindAnyObjectByType<TrashCollectingMechanic>().enabled = false;
        
    }

    private void EndPause()
    {
        finishFishingPanel.SetActive(false);

        shipStateManager.enabled = true;
        boatMovement.enabled = true;
        fishingMechanic.enabled = true;
        FindAnyObjectByType<TrashCollectingMechanic>().enabled = true;
        
    }
    private void EndFishing()
    {
        finishFishingPanel.SetActive(true);

        shipStateManager.SetMode(Modes.Moving);
        FindAnyObjectByType<ShipStateManager>().enabled = false;
        boatMovement.enabled = false;
        fishingMechanic.enabled = false;
        FindAnyObjectByType<TrashCollectingMechanic>().enabled = false;
    }

    private void ReturnToHome()
    {
        FindAnyObjectByType<ReturnHomeManager>().ReturnHome();
    }
}
