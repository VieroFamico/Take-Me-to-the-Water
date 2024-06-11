using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentShopManager : BuildingManager
{
    private PlayerInventory playerInventory;
    private PlayerLoadout playerLoadout;

    // UI Elements
    public Slider fuelAmountSlider; // Slider for selecting fuel amount
    public TextMeshProUGUI fuelPercentageText; // Text to display the current value of the slider
    public TextMeshProUGUI minFuelText; // Text to display the minimum fuel percentage
    public TextMeshProUGUI maxFuelText; // Text to display the maximum fuel percentage
    public TextMeshProUGUI feedbackText; // Optional: To show feedback messages
    public Button refuelButton; 
    public Button closeButton; 


    private void Start()
    {
        playerInventory = GameManager.Instance.playerInventory;
        playerLoadout = playerInventory.GetPlayerLoadout();

        closeButton.onClick.AddListener(CloseDisplay);

        // Initialize the slider
        InitializeSlider();

        // Add listeners
        fuelAmountSlider.onValueChanged.AddListener(OnSliderValueChanged);
        refuelButton.onClick.AddListener(OnRefuelButtonClick);

        // Update UI initially
        UpdateSliderUI();
    }

    private void InitializeSlider()
    {
        ShipSO currentShip = playerLoadout.GetCurrentShip();
        fuelAmountSlider.minValue = 0;
        fuelAmountSlider.maxValue = 100;
        fuelAmountSlider.wholeNumbers = true;

        // Set initial slider value to the current fuel percentage
        fuelAmountSlider.value = currentShip.currentTimeLimit / currentShip.shipTimeLimit * 100;

        // Set min and max fuel texts
        minFuelText.text = "0%";
        maxFuelText.text = "100%";
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateSliderUI();
    }

    private void OnRefuelButtonClick()
    {
        RefuelShip();
        UpdateSliderUI();
    }

    private void UpdateSliderUI()
    {
        int fuelPercentage = Mathf.RoundToInt(fuelAmountSlider.value);
        fuelPercentageText.text = $"{fuelPercentage}%";

        // Check if the player has enough money
        ShipSO currentShip = playerLoadout.GetCurrentShip();
        float currentFuelPercentage = currentShip.currentTimeLimit / currentShip.shipTimeLimit * 100;
        float fuelCost = fuelPercentage - currentFuelPercentage;
        refuelButton.interactable = playerInventory.money >= fuelCost;
    }

    public void RefuelShip()
    {
        int fuelPercentage = Mathf.RoundToInt(fuelAmountSlider.value);
        ShipSO currentShip = playerLoadout.GetCurrentShip();
        float currentFuelPercentage = currentShip.currentTimeLimit / currentShip.shipTimeLimit * 100;
        float fuelToBuy = fuelPercentage - currentFuelPercentage;

        if (fuelToBuy <= 0)
        {
            ShowFeedback("Fuel amount must be greater than the current fuel level.");
            return;
        }

        bool success = playerInventory.PurchaseFuel((int)fuelToBuy);
        if (success)
        {
            ShowFeedback("Ship refueled successfully.");
            currentShip.Refuel((int)fuelPercentage); // Refuel the ship
            InitializeSlider(); // Reinitialize slider values
            UpdateSliderUI(); // Update UI after refueling
        }
        else
        {
            ShowFeedback("Not enough money to refuel.");
        }
        playerInventory.GetDisplayManager().UpdateDisplay();
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }
    }

}