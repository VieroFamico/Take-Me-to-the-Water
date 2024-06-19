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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadInventories();
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
        Debug.Log(playerData?.money);
        Debug.Log(playerData?.trashList);
        Debug.Log(playerData?.playerLoadout);

        if (playerData != null)
        {
            playerInventory.money = playerData.money;
            Debug.Log(playerData.money);
            Debug.Log(playerInventory.money);

            FishInventory fishInventory = new FishInventory();
            fishInventory.SetFishList(playerData.fishList);
            playerInventory.SetPlayerFishInventory(fishInventory);

            TrashInventory trashInventory = new TrashInventory();
            trashInventory.SetTrashList(playerData.trashList);
            playerInventory.SetPlayerTrashInventory(trashInventory);

            PlayerLoadoutWrapper loadedPlayerLoadout = playerData.playerLoadout;
            PlayerLoadout playerLoadout = playerInventory.GetPlayerLoadout();

            if (loadedPlayerLoadout == null)
            {
                // If loadedPlayerLoadout is null, initialize with basic data
                playerLoadout.SetCurrentBait(PlayerLoadout.Bait.Worm);

                Dictionary<PlayerLoadout.Bait, int> baitAmounts = new Dictionary<PlayerLoadout.Bait, int>();
                foreach (var bait in System.Enum.GetValues(typeof(PlayerLoadout.Bait)))
                {
                    baitAmounts[(PlayerLoadout.Bait)bait] = 0;
                }
                playerLoadout.SetBaitAmounts(baitAmounts);

                ShipSO ship = ScriptableObject.CreateInstance<ShipSO>();
                ship.shipName = "BaseShip";
                ship.shipSprite = Resources.Load<Sprite>("defaultSpritePath"); // Assuming default sprite
                ship.shipTimeLimit = 5;
                ship.currentTimeLimit = 0;

                playerLoadout.SetCurrentShip(ship);
            }
            else
            {
                // If loadedPlayerLoadout is not null, load its values
                playerLoadout.SetCurrentBait(loadedPlayerLoadout.currentBait);

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
                    // Initialize bait amounts if loadedPlayerLoadout.baitAmounts is null
                    foreach (var bait in System.Enum.GetValues(typeof(PlayerLoadout.Bait)))
                    {
                        baitAmounts[(PlayerLoadout.Bait)bait] = 0;
                    }
                }
                playerLoadout.SetBaitAmounts(baitAmounts);

                ShipSO baseShip = ScriptableObject.CreateInstance<ShipSO>();
                baseShip.shipName = loadedPlayerLoadout.currentShipName ?? "BaseShip";
                baseShip.shipSprite = loadedPlayerLoadout.currentShipSpriteData != null
                    ? SaveManager.ByteArrayToSprite(loadedPlayerLoadout.currentShipSpriteData) : Resources.Load<Sprite>("defaultSpritePath");
                baseShip.shipTimeLimit = loadedPlayerLoadout.currentShipTimeLimit != 0
                    ? loadedPlayerLoadout.currentShipTimeLimit : 5;
                baseShip.currentTimeLimit = (loadedPlayerLoadout.currentCurrentTimeLimit >= 0 && loadedPlayerLoadout.currentCurrentTimeLimit <= 100) 
                    ? loadedPlayerLoadout.currentCurrentTimeLimit : 0;

                playerLoadout.SetCurrentShip(baseShip);
            }

            SaveManager.SavePlayerInventory(playerInventory);
            Debug.Log("Save If Not Null");
        }
        else
        {
            SaveManager.SavePlayerInventory(playerInventory);
            Debug.Log("Save And Is Null");
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

