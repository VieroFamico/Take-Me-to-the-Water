using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public abstract class CraftingManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public PlayerLoadout playerLoadout;
    [Header("Craft Button")]
    public Button craftButton;
    public TextMeshProUGUI priceText;
    [Header("Item Display")]
    public TextMeshProUGUI nameText;
    public Image itemImage;
    [Header("Materials")]
    public Image plasticImage;
    public Image metalImage;
    public Image woodImage;
    public Image rubberImage;

    private TextMeshProUGUI plasticText;
    private TextMeshProUGUI metalText;
    private TextMeshProUGUI woodText;
    private TextMeshProUGUI rubberText;
    protected ScriptableObject itemToCraft;
    protected int currentTier;

    void Start()
    {
        StartCoroutine(InitializeStart());
    }

    private IEnumerator InitializeStart()
    {
        yield return new WaitForSeconds(0.05f);
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        playerLoadout = playerInventory.GetPlayerLoadout();

        plasticText = plasticImage.GetComponentInChildren<TextMeshProUGUI>(true);
        metalText = metalImage.GetComponentInChildren<TextMeshProUGUI>(true);
        woodText = woodImage.GetComponentInChildren<TextMeshProUGUI>(true);
        rubberText = rubberImage.GetComponentInChildren<TextMeshProUGUI>(true);

        Initialize();
        UpdateUI();
        craftButton.onClick.AddListener(CraftItem);
    }

    public void Initialize()
    {
        SetItemToCraft();
    }

    protected abstract void SetItemToCraft();

    public virtual void UpdateUI()
    {
        if (itemToCraft == null) return;

        int price = 0;
        int plasticNeeded = 0;
        int metalNeeded = 0;
        int woodNeeded = 0;
        int rubberNeeded = 0;

        if (itemToCraft is FishingRodSO rod)
        {
            nameText.text = rod.toolsName;
            itemImage.sprite = rod.sprite;
            price = rod.price;
            plasticNeeded = rod.plasticNeeded;
            metalNeeded = rod.metalNeeded;
            woodNeeded = rod.woodNeeded;
            rubberNeeded = rod.rubberNeeded;
        }
        else if (itemToCraft is ShipEngineSO engine)
        {
            nameText.text = engine.toolsName;
            itemImage.sprite = engine.sprite;
            price = engine.price;
            plasticNeeded = engine.plasticNeeded;
            metalNeeded = engine.metalNeeded;
            woodNeeded = engine.woodNeeded;
            rubberNeeded = engine.rubberNeeded;
        }
        else if (itemToCraft is ShipBodySO body)
        {
            nameText.text = body.toolsName;
            itemImage.sprite = body.sprite;
            price = body.price;
            plasticNeeded = body.plasticNeeded;
            metalNeeded = body.metalNeeded;
            woodNeeded = body.woodNeeded;
            rubberNeeded = body.rubberNeeded;
        }

        int playerPlastic = playerInventory.GetPlayerTrashInventory().GetPlasticAmount();
        int playerMetal = playerInventory.GetPlayerTrashInventory().GetMetalAmount();
        int playerWood = playerInventory.GetPlayerTrashInventory().GetWoodAmount();
        int playerRubber = playerInventory.GetPlayerTrashInventory().GetRubberAmount();

        priceText.text = $"{price}";
        plasticText.text = $"{playerPlastic}/{plasticNeeded}";
        metalText.text = $"{playerMetal}/{metalNeeded}";
        woodText.text = $"{playerWood}/{woodNeeded}";
        rubberText.text = $"{playerRubber}/{rubberNeeded}";

        // Check if player has enough resources
        bool hasEnoughResources = playerInventory.money >= price &&
                                  playerInventory.GetPlayerTrashInventory().GetPlasticAmount() >= plasticNeeded &&
                                  playerInventory.GetPlayerTrashInventory().GetMetalAmount() >= metalNeeded &&
                                  playerInventory.GetPlayerTrashInventory().GetWoodAmount() >= woodNeeded &&
                                  playerInventory.GetPlayerTrashInventory().GetRubberAmount() >= rubberNeeded;

        craftButton.interactable = hasEnoughResources;

        plasticImage.color = playerInventory.GetPlayerTrashInventory().GetPlasticAmount() >= plasticNeeded ? Color.white : Color.gray;
        metalImage.color = playerInventory.GetPlayerTrashInventory().GetMetalAmount() >= metalNeeded ? Color.white : Color.gray;
        woodImage.color = playerInventory.GetPlayerTrashInventory().GetWoodAmount() >= woodNeeded ? Color.white : Color.gray;
        rubberImage.color = playerInventory.GetPlayerTrashInventory().GetRubberAmount() >= rubberNeeded ? Color.white : Color.gray;
    }

    public void CraftItem()
    {
        if (itemToCraft == null) return;

        int price = 0;
        int plasticNeeded = 0;
        int metalNeeded = 0;
        int woodNeeded = 0;
        int rubberNeeded = 0;

        if (itemToCraft is FishingRodSO rod)
        {
            price = rod.price;
            plasticNeeded = rod.plasticNeeded;
            metalNeeded = rod.metalNeeded;
            woodNeeded = rod.woodNeeded;
            rubberNeeded = rod.rubberNeeded;

            playerLoadout.SetCurrentFishingRod(rod);
        }
        else if (itemToCraft is ShipEngineSO engine)
        {
            price = engine.price;
            plasticNeeded = engine.plasticNeeded;
            metalNeeded = engine.metalNeeded;
            woodNeeded = engine.woodNeeded;
            rubberNeeded = engine.rubberNeeded;

            playerLoadout.SetCurrentShipEngine(engine);
        }
        else if (itemToCraft is ShipBodySO body)
        {
            price = body.price;
            plasticNeeded = body.plasticNeeded;
            metalNeeded = body.metalNeeded;
            woodNeeded = body.woodNeeded;
            rubberNeeded = body.rubberNeeded;

            playerLoadout.SetCurrentShipBody(body);
            playerLoadout.SetCurrentShipFuel(body.shipTimeLimit);
        }

        // Deduct resources
        playerInventory.SpendMoney(price);
        playerInventory.GetPlayerTrashInventory().UseMaterials(plasticNeeded, metalNeeded, woodNeeded, rubberNeeded);

        // Save the result of the crafting
        SaveManager.SavePlayerInventory(playerInventory);

        UpdateUI();

        // Re-initialize to the next tier
        Initialize();
    }

}
