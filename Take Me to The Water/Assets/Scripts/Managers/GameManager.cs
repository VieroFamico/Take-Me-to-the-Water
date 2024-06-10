using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using UnityEngine;
using static PlayerLoadout;

public class GameManager : MonoBehaviour
{
    public PlayerInventory playerInventory; // Player's inventory including money
    public FishInventory shopFishInventory; // Shop's fish inventory
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventories();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        //SaveInventories();
    }

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void LoadInventories()
    {
        PlayerInventoryWrapper playerData = SaveManager.LoadPlayerInventory();
        
        if (playerData != null)
        {
            playerInventory.money = playerData.money;

            // Check and set fish inventory
            FishInventory fishInventory = new FishInventory();
            if (playerData.fishInventory != null)
            {
                fishInventory.SetFishList(playerData.fishInventory);
            }
            else
            {
                fishInventory.SetFishList(new List<FishData>());
            }
            playerInventory.SetPlayerFishInventory(fishInventory);

            // Check and set player loadout
            PlayerLoadoutWrapper loadedPlayerLoadout = playerData.playerLoadout;
            PlayerLoadout playerLoadout = playerInventory.GetPlayerLoadout();

            if (loadedPlayerLoadout != null)
            {
                // Set current bait
                playerLoadout.SetCurrentBait(Bait.Worm);

                // Set bait amounts
                Dictionary<PlayerLoadout.Bait, int> baitAmounts = new Dictionary<PlayerLoadout.Bait, int>();
                if (loadedPlayerLoadout.baitAmounts != null)
                {
                    foreach (var bait in loadedPlayerLoadout.baitAmounts)
                    {
                        baitAmounts[bait.Key] = bait.Value;
                    }
                }
                else
                {
                    baitAmounts[PlayerLoadout.Bait.Worm] = 0;
                    baitAmounts[PlayerLoadout.Bait.Ulat] = 0;
                    baitAmounts[PlayerLoadout.Bait.Cricket] = 0;
                    baitAmounts[PlayerLoadout.Bait.Shrimp] = 0;
                    baitAmounts[PlayerLoadout.Bait.Pelet] = 0;
                }
                playerLoadout.SetBaitAmounts(baitAmounts);

                // Set current ship
                ShipSO baseShip = ScriptableObject.CreateInstance<ShipSO>();
                if (!string.IsNullOrEmpty(loadedPlayerLoadout.currentShipName))
                {
                    baseShip.shipName = loadedPlayerLoadout.currentShipName;
                }
                else
                {
                    baseShip.shipName = "BaseShip";
                }

                if (!string.IsNullOrEmpty(loadedPlayerLoadout.currentShipSpritePath))
                {
                    baseShip.shipSprite = Resources.Load<Sprite>(loadedPlayerLoadout.currentShipSpritePath); // Assuming sprite is stored in Resources folder
                }
                else
                {
                    baseShip.shipSprite = Resources.Load<Sprite>("DefaultShipSprite"); // Load a default sprite
                }

                baseShip.shipTimeLimit = loadedPlayerLoadout.currentShipTimeLimit > 0 ? loadedPlayerLoadout.currentShipTimeLimit : 5;
                playerLoadout.SetCurrentShip(baseShip);
            }
            else
            {
                // Set default values if player loadout is null
                playerLoadout.SetCurrentBait(Bait.Worm);

                Dictionary<PlayerLoadout.Bait, int> baitAmounts = new Dictionary<PlayerLoadout.Bait, int>();
                baitAmounts[PlayerLoadout.Bait.Worm] = 0;
                baitAmounts[PlayerLoadout.Bait.Ulat] = 0;
                baitAmounts[PlayerLoadout.Bait.Cricket] = 0;
                baitAmounts[PlayerLoadout.Bait.Shrimp] = 0;
                baitAmounts[PlayerLoadout.Bait.Pelet] = 0;
                playerLoadout.SetBaitAmounts(baitAmounts);

                ShipSO baseShip = ScriptableObject.CreateInstance<ShipSO>();
                baseShip.shipName = "BaseShip";
                baseShip.shipSprite = Resources.Load<Sprite>("DefaultShipSprite"); // Load a default sprite
                baseShip.shipTimeLimit = 5;
                playerLoadout.SetCurrentShip(baseShip);
            }

            Debug.Log(playerData.money);
            Debug.Log(playerData.fishInventory);
            Debug.Log(playerData.playerLoadout);

            //SaveManager.SavePlayerInventory(playerInventory);
        }
        else
        {
            SaveManager.SavePlayerInventory(playerInventory);
        }

        shopFishInventory = SaveManager.LoadFishInventory("ShopInventory.json");
        if (shopFishInventory == null)
        {
            shopFishInventory = new FishInventory();
        }
    }

    private void SaveInventories()
    {
        Debug.Log("First Save");
        SaveManager.SavePlayerInventory(playerInventory);
        Debug.Log("Second Save");
        SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json");
    }
}

