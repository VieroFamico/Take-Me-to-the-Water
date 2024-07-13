using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStateManager : MonoBehaviour
{
    public BoatMovement boatMovement;
    public SpriteRenderer fishingRodSpriteRenderer;
    public Sprite trashCatchingToolSprite;

    public enum Modes
    {
        Moving,
        Fishing,
        TrashCollecting
    }

    public Modes currentMode { get; private set; } = Modes.Moving;
    private float lastModeChangeTime;
    private float modeChangeDelay = 0.5f;

    private FishingMechanic fishingMechanic;
    private TrashCollectingMechanic trashCollectingMechanic;

    private void Start()
    {
        fishingMechanic = GetComponent<FishingMechanic>();
        trashCollectingMechanic = GetComponent<TrashCollectingMechanic>();
        SetMode(Modes.Moving);
    }

    private void Update()
    {
        if (Time.time - lastModeChangeTime >= modeChangeDelay)
        {
            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                if (currentMode == Modes.Moving)
                {
                    SetMode(Modes.Fishing);
                }
                else
                {
                    SetMode(Modes.Moving);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E)) // 'E' key
            {
                if (currentMode == Modes.Fishing)
                {
                    SetMode(Modes.TrashCollecting);
                }
                else if (currentMode == Modes.TrashCollecting)
                {
                    SetMode(Modes.Fishing);
                }
            }
        }
    }

    public void SetMode(Modes mode)
    {
        currentMode = mode;
        lastModeChangeTime = Time.time;

        if (fishingMechanic != null)
        {
            fishingMechanic.enabled = (mode == Modes.Fishing);
            if (mode == Modes.Fishing)
            {
                fishingMechanic.EnterFishingMode();
                fishingRodSpriteRenderer.sprite = FindAnyObjectByType<PlayerLoadout>().GetCurrentFishingRod().topDownSprite;
                StopBoat();
            }
            else
            {
                fishingMechanic.ExitFishingMode();
            }
        }

        if (trashCollectingMechanic != null)
        {
            trashCollectingMechanic.enabled = (mode == Modes.TrashCollecting);
            if (mode == Modes.TrashCollecting)
            {
                trashCollectingMechanic.StartTrashCollecting();
                fishingRodSpriteRenderer.sprite = trashCatchingToolSprite;
                StopBoat();
            }
            else
            {
                trashCollectingMechanic.ExitTrashCollectingMode();
            }
        }
        if(currentMode == Modes.Moving)
        {
            ResumeBoat();
        }
    }

    public Modes GetCurrentMode()
    {
        return currentMode;
    }
    void StopBoat()
    {
        if (boatMovement != null)
        {
            boatMovement.StopBoat();
        }
    }

    void ResumeBoat()
    {
        if (boatMovement != null)
        {
            boatMovement.StartBoat();
        }
    }
}
