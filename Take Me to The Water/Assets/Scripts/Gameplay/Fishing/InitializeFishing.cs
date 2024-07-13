using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShipStateManager;

public class InitializeFishing : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public ShipStateManager shipStateManager;
    public BoatMovement boatMovement;
    public FishingMechanic fishingMechanic;
    public GameObject finishFishingPanel;
    public GameObject pausePanel;
    public TextMeshProUGUI timeLeft;

    [Header("Sprites")]
    public SpriteRenderer shipBodySprite;
    public SpriteRenderer shipEngineSprite;
    public SpriteRenderer fishingRodSprite;

    [Header("References")]
    public Button returnHomeButton;
    public Button abortFishingButton;


    private PlayerLoadout playerLoadout;

    private bool findPlayerInventoryFlag = false;
    private float originalShipFuel = 5f;
    private float currTime = 0f;
    private bool isPausing = false;
    private bool isOutOfFuel = false;
    private void Start()
    {
        finishFishingPanel.SetActive(false);

        returnHomeButton.onClick.AddListener(ReturnToHome);
        abortFishingButton.onClick.AddListener(ReturnToHome);
    }

    private void Update()
    {
        if (!findPlayerInventoryFlag)
        {
            playerInventory = FindAnyObjectByType<PlayerInventory>();
            playerLoadout = playerInventory.GetPlayerLoadout();

            shipBodySprite.sprite = playerLoadout.GetCurrentShipBody().sprite;
            shipEngineSprite.sprite = playerLoadout.GetCurrentShipEngine().sprite;
            fishingRodSprite.sprite = playerLoadout.GetCurrentFishingRod().topDownSprite;

            boatMovement.SetSpeed(playerLoadout.GetCurrentShipEngine().movementSpeed);
            fishingMechanic.SetMaxTension(playerLoadout.GetCurrentFishingRod().maxTension);
            originalShipFuel = playerLoadout.GetCurrentShipFuel();

            findPlayerInventoryFlag = true;
        }

        if (currTime >= originalShipFuel && !isOutOfFuel)
        {
            timeLeft.text = 0f.ToString();
            EndFishing();
            isOutOfFuel = true;
        }
        else
        {
            if (isOutOfFuel) return;
            currTime += Time.deltaTime / 60f;
            timeLeft.text = ((int)((originalShipFuel - currTime) * 60f)).ToString();
            playerLoadout.SetCurrentShipFuel(originalShipFuel - currTime);
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
