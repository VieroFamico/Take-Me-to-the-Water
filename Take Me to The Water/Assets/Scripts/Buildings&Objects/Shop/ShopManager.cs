using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : BuildingManager
{
    public FishSO temp;
    public FishSO empty;

    public Transform showFishPanel;
    public Button buyButton;
    public TextMeshProUGUI selectedFishName;
    public Image selectedFishImage;
    public TextMeshProUGUI selectedFishPrice;
    public GameObject fishCardPrefab;
    public PlayerInventory playerInventory;
    public FishInventory shopFishInventory; // Separate inventory for the shop

    private FishSO selectedFish;

    void Start()
    {
        playerInventory = GameManager.Instance.playerInventory;
        closeButton.onClick.AddListener(CloseDisplay);
        buyButton.onClick.AddListener(BuyOrSellSelectedFish);
    }

    void PopulateInventory()
    {
        foreach (Transform child in showFishPanel)
        {
            Destroy(child.gameObject);
        }

        List<FishSO> fishInventory = playerInventory.GetPlayerFishInventory().GetFishList();

        foreach (var fish in fishInventory)
        {
            GameObject fishButton = new(fish.name);
            fishButton.transform.SetParent(showFishPanel);
            Button button = fishButton.AddComponent<Button>();
            Image image = fishButton.AddComponent<Image>();
            image.sprite = fish.fishSprite;
            button.onClick.AddListener(() => SelectFish(fish));
            fishButton.transform.localScale = Vector3.one;

            //fishButton.GetComponentInChildren<TextMeshProUGUI>().text = fish.fishName;
            //fishButton.GetComponentInChildren<SpriteRenderer>().sprite = fish.cardSprite;
            //fishButton.GetComponent<Button>().onClick.AddListener(() => SelectFish(fish));
        }
    }

    void SelectFish(FishSO fish)
    {
        selectedFish = fish;
        selectedFishName.text = fish.fishName;
        selectedFishImage.sprite = fish.fishSprite;
        selectedFishPrice.text = $"${fish.price}";

        if(fish == empty)
        {
            selectedFishImage.enabled = false;
            selectedFishPrice.text = "";
        }
        else
        {
            selectedFishImage.enabled = true;
        }
    }

    public void BuyOrSellSelectedFish()
    {
        if (selectedFish != null && selectedFish != empty)
        {
            playerInventory.AddMoney(selectedFish.price);
            playerInventory.GetPlayerFishInventory().RemoveFish(selectedFish);
            shopFishInventory.AddFish(selectedFish);
            SelectFish(empty);

            //SaveManager.SaveFishInventory(playerInventory.GetPlayerFishInventory(), "PlayerInventory.json"); // Save the player's inventory
            SaveManager.SavePlayerInventory(playerInventory); // Save the player's inventory
            SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json"); // Save the shop's inventory

            PopulateInventory(); // Refresh the inventory display
        }
    }

    public override void OpenDisplay()
    {
        base.OpenDisplay();
        PopulateInventory();
    }
    public override void CloseDisplay()
    {
        base.CloseDisplay();
        playerInventory.GetPlayerFishInventory().AddFish(temp);
        SaveManager.SavePlayerInventory(playerInventory); // Save the player's inventory
        SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json"); // Save the shop's inventory
    }
}
