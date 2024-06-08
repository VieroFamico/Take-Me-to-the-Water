using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aquarium_Manager : BuildingManager
{
    public GameObject fishPrefab;
    public GameObject fishFoodPrefab;
    public Transform inventoryPanel;
    public Transform fishContainer; // Container for spawned fish
    public Button addButton;
    public Button feedButton;
    public Button doneButton;
    public GameObject aquariumDisplay; // Reference to the display panel
    public float fishSize = 1f;

    private FishInventory fishInventoryList;
    private FishData selectedFish;
    private List<GameObject> aquariumFishList = new List<GameObject>();

    void Start()
    {
        fishInventoryList = FindAnyObjectByType<PlayerInventory>().GetPlayerFishInventory();
        PopulateInventory();
        addButton.onClick.AddListener(AddSelectedFishToAquarium);
        feedButton.onClick.AddListener(SpawnFishFood);
        doneButton.onClick.AddListener(CloseDisplay);
    }

    void PopulateInventory()
    {
        if (fishInventoryList == null) return;

        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        List<FishData> fishInventory = fishInventoryList.GetFishInventory();

        foreach (var fish in fishInventory)
        {
            GameObject fishButton = new GameObject(fish.name);
            fishButton.transform.SetParent(inventoryPanel);
            Button button = fishButton.AddComponent<Button>();
            Image image = fishButton.AddComponent<Image>();
            image.sprite = fish.fishSprite;
            button.onClick.AddListener(() => SelectFish(fish));
            fishButton.transform.localScale = Vector3.one;
        }
    }

    void SelectFish(FishData fish)
    {
        selectedFish = fish;
    }

    void AddSelectedFishToAquarium()
    {
        if (selectedFish != null)
        {
            GameObject newFish = Instantiate(fishPrefab, transform.position, Quaternion.identity);
            newFish.GetComponent<AquariumFish>().fishData = selectedFish;
            newFish.GetComponent<AquariumFish>().fishContainer = fishContainer.GetComponent<RectTransform>();
            newFish.GetComponent<SpriteRenderer>().sprite = newFish.GetComponent<AquariumFish>().fishData.fishSprite;
            newFish.GetComponent<SpriteRenderer>().sortingOrder = 110;
            newFish.transform.position = fishContainer.transform.position;
            newFish.transform.localScale = Vector3.one * fishSize;

            aquariumFishList.Add(newFish);
        }
    }

    void SpawnFishFood()
    {
        Instantiate(fishFoodPrefab, fishContainer.GetComponent<RectTransform>().transform.position, Quaternion.identity);
    }
    override public void OpenDisplay()
    {
        PopulateInventory();
        aquariumDisplay.SetActive(true);
        SetFishActiveState(true); // Enable fish when display is opened
    }
    void SetFishActiveState(bool state)
    {
        foreach (var fish in aquariumFishList)
        {
            fish.SetActive(state);
        }
    }
    override public void CloseDisplay()
    {
        aquariumDisplay.SetActive(false);
        SetFishActiveState(false);
    }
}
