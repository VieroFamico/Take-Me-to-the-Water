using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerInventory playerInventory; // Player's inventory including money
    public FishInventory shopFishInventory; // Shop's fish inventory

    public ShipBodySO baseShipBodySO;
    public ShipEngineSO baseShipEngineSO;
    public FishingRodSO baseFishingRodSO;

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

    void Start()
    {
        //LoadInventories();
    }

    private void OnApplicationQuit()
    {
        SaveInventories();
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

            FishInventory fishInventory = new FishInventory();
            fishInventory.SetFishList(playerData.fishList);
            playerInventory.SetPlayerFishInventory(fishInventory);

            TrashInventory trashInventory = new TrashInventory();
            trashInventory.SetTrashList(playerData.trashList);
            playerInventory.SetPlayerTrashInventory(trashInventory);

            playerInventory.GetPlayerTrashInventory().SetMaterials(playerData.plasticAmount, playerData.metalAmount,
                playerData.woodAmount, playerData.rubberAmount);

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

                ShipBodySO ship = baseShipBodySO;
                playerLoadout.SetCurrentShipBody(ship);
                playerLoadout.SetCurrentShipFuel(ship.shipTimeLimit);

                ShipEngineSO shipEngine = baseShipEngineSO;
                playerLoadout.SetCurrentShipEngine(shipEngine);

                FishingRodSO fishingRodSO = baseFishingRodSO;
                playerLoadout.SetCurrentFishingRod(fishingRodSO);

                playerInventory.SetPlayerLoadout(playerLoadout);
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

                ShipBodySO ship = loadedPlayerLoadout.currentShip;
                if (!ship)
                {
                    ShipBodySO emptyShip = baseShipBodySO;
                    playerLoadout.SetCurrentShipBody(emptyShip);
                    playerLoadout.SetCurrentShipFuel(emptyShip.shipTimeLimit);
                }
                else
                {
                    playerLoadout.SetCurrentShipBody(ship);
                    playerLoadout.SetCurrentShipFuel(loadedPlayerLoadout.currentShipCurrentFuel);
                }
                

                ShipEngineSO shipEngine = loadedPlayerLoadout.currentShipEngine;
                playerLoadout.SetCurrentShipEngine(shipEngine);

                FishingRodSO fishingRodSO = loadedPlayerLoadout.currentFishingRod;
                playerLoadout.SetCurrentFishingRod(fishingRodSO);

                playerInventory.SetPlayerLoadout(playerLoadout);
            }

            SaveManager.SavePlayerInventory(playerInventory);
            Debug.Log("Save If Not Null");
        }
        else
        {
            playerInventory.money = 100f;
            FishInventory fishInventory = new FishInventory();
            playerInventory.SetPlayerFishInventory(fishInventory);

            TrashInventory trashInventory = new TrashInventory();
            playerInventory.SetPlayerTrashInventory(trashInventory);

            PlayerLoadout newPlayerLoadOut = gameObject.AddComponent<PlayerLoadout>();
            // If loadedPlayerLoadout is null, initialize with basic data
            newPlayerLoadOut.SetCurrentBait(PlayerLoadout.Bait.Worm);

            Dictionary<PlayerLoadout.Bait, int> baitAmounts = new Dictionary<PlayerLoadout.Bait, int>();
            foreach (var bait in System.Enum.GetValues(typeof(PlayerLoadout.Bait)))
            {
                baitAmounts[(PlayerLoadout.Bait)bait] = 0;
            }
            newPlayerLoadOut.SetBaitAmounts(baitAmounts);

            ShipBodySO ship = baseShipBodySO;
            newPlayerLoadOut.SetCurrentShipBody(ship);
            newPlayerLoadOut.SetCurrentShipFuel(ship.shipTimeLimit);

            ShipEngineSO shipEngine = baseShipEngineSO;
            newPlayerLoadOut.SetCurrentShipEngine(shipEngine);

            FishingRodSO fishingRodSO = baseFishingRodSO;
            newPlayerLoadOut.SetCurrentFishingRod(fishingRodSO);

            playerInventory.SetPlayerLoadout(newPlayerLoadOut);

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
        SaveManager.SavePlayerInventory(playerInventory);
        SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json");
    }
}

