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

    private FishInventory fishInventoryScript;
    private FishData selectedFish;

    void Start()
    {
        fishInventoryScript = FindObjectOfType<FishInventory>();
        PopulateInventory();
        addButton.onClick.AddListener(AddSelectedFishToAquarium);
        feedButton.onClick.AddListener(SpawnFishFood);
        doneButton.onClick.AddListener(CloseAquariumDisplay);
    }

    void PopulateInventory()
    {
        if (fishInventoryScript == null) return;

        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        List<FishData> fishInventory = fishInventoryScript.GetFishInventory();

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
            
            // Set the fish data here if needed, such as setting a sprite or name
            // newFish.GetComponent<Fish>().Initialize(selectedFish);
        }
    }

    void SpawnFishFood()
    {
        Instantiate(fishFoodPrefab, fishContainer.GetComponent<RectTransform>().transform.position, Quaternion.identity);
    }

    void CloseAquariumDisplay()
    {
        aquariumDisplay.SetActive(false);
    }
}
