using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : BuildingManager
{
    public FishData temp;
    public FishData empty;

    public Transform showFishPanel;
    public Button buyPanelButton;
    public Button sellPanelButton;
    public Button buyButton;
    public Button doneButton;
    public TextMeshProUGUI selectedFishName;
    public Image selectedFishImage;
    public TextMeshProUGUI selectedFishPrice;
    public GameObject fishCardPrefab;
    public PlayerInventory playerInventory;
    public FishInventory shopFishInventory; // Separate inventory for the shop

    private FishData selectedFish;
    private bool isBuying;

    void Start()
    {
        playerInventory = GameManager.Instance.playerInventory;
        buyPanelButton.onClick.AddListener(OpenBuyPanel);
        sellPanelButton.onClick.AddListener(OpenSellPanel);
        doneButton.onClick.AddListener(CloseDisplay);
        buyButton.onClick.AddListener(BuyOrSellSelectedFish);
    }

    void OpenBuyPanel()
    {
        shopFishInventory.GetFishInventory().Add(temp);
        isBuying = true;
        ChangeBuySellText();
        PopulateInventory();
    }

    void OpenSellPanel()
    {
        playerInventory.GetPlayerFishInventory().GetFishInventory().Add(temp);
        isBuying = false;
        ChangeBuySellText();
        PopulateInventory();
    }

    private void ChangeBuySellText()
    {
        buyButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = isBuying ? "Buy" : "Sell";
    }

    void PopulateInventory()
    {
        foreach (Transform child in showFishPanel)
        {
            Destroy(child.gameObject);
        }

        List<FishData> fishInventory = isBuying ? shopFishInventory.GetFishInventory() : playerInventory.GetPlayerFishInventory().GetFishInventory();

        foreach (var fish in fishInventory)
        {
            GameObject fishButton = new GameObject(fish.name);
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

    void SelectFish(FishData fish)
    {
        selectedFish = fish;
        selectedFishName.text = fish.fishName;
        selectedFishImage.sprite = fish.fishSprite;
        selectedFishPrice.text = isBuying ? $"BUY ${fish.price}" : $"SELL ${fish.price}";
    }

    public void BuyOrSellSelectedFish()
    {
        if (selectedFish != null)
        {
            if (isBuying)
            {
                if (playerInventory.SpendMoney(selectedFish.price))
                {
                    playerInventory.GetPlayerFishInventory().AddFish(selectedFish);
                    shopFishInventory.RemoveFish(selectedFish);
                    SelectFish(empty);
                }
            }
            else
            {
                playerInventory.AddMoney(selectedFish.price);
                playerInventory.GetPlayerFishInventory().RemoveFish(selectedFish);
                shopFishInventory.AddFish(selectedFish);
                SelectFish(empty);
            }
            //SaveManager.SaveFishInventory(playerInventory.GetPlayerFishInventory(), "PlayerInventory.json"); // Save the player's inventory
            SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json"); // Save the shop's inventory

            PopulateInventory(); // Refresh the inventory display
        }
    }

    public override void OpenDisplay()
    {
        base.OpenDisplay();
        isBuying = true;
        PopulateInventory();
    }
    public override void CloseDisplay()
    {
        base.CloseDisplay();
        SaveManager.SaveFishInventory(playerInventory.GetPlayerFishInventory(), "PlayerInventory.json"); // Save the player's inventory
        SaveManager.SaveFishInventory(shopFishInventory, "ShopInventory.json"); // Save the shop's inventory
    }
}
