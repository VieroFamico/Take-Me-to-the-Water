using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadoutDisplayManager : MonoBehaviour
{
    public PlayerInventory playerInventory;

    // Image components to display sprites
    public Image shipImage;
    public Image fishingRodImage;
    public Image shipEngineImage;

    private PlayerLoadout playerLoadout;
    void Start()
    {
        playerLoadout = playerInventory.GetComponent<PlayerLoadout>();
        Debug.Log(playerLoadout);
        Debug.Log(playerLoadout.currentShip);
        Debug.Log(playerLoadout.currentShip.shipSprite != null);
        UpdateSprites();
    }

    public void UpdateSprites()
    {
        if (playerLoadout.currentShip != null)
        {
            shipImage.sprite = playerLoadout.currentShip.shipSprite;
        }

        if (playerLoadout.currentFishingRod != null)
        {
            fishingRodImage.sprite = playerLoadout.currentFishingRod.rodSprite;
        }

        if (playerLoadout.currentShipEngine != null)
        {
            shipEngineImage.sprite = playerLoadout.currentShipEngine.engineSprite;
        }
    }

}
