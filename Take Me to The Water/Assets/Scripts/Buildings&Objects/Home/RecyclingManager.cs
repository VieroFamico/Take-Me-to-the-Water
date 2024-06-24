using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecyclingManager : BuildingManager
{
    [Header("UI References")]
    public GameObject trashButtonPrefab;
    public Transform trashButtonContainer;
    public Image selectedTrashImage;
    public TextMeshProUGUI selectedTrashName;
    public Button recycleButton;
    public Button recycleAllButton;

    [Header("Player Inventory")]
    public PlayerInventory inventory;

    private List<TrashSO> playerTrashInventory = new List<TrashSO>(); // Populate this in the inspector or through another script
    private TrashSO selectedTrash;
    private Button selectedButton;

    void Start()
    {
        inventory = FindAnyObjectByType<PlayerInventory>();
        Debug.Log(inventory);
        Debug.Log(inventory.GetPlayerTrashInventory());
        playerTrashInventory = inventory.GetPlayerTrashInventory().GetTrashList();
        PopulateTrashButtons();

        recycleButton.onClick.AddListener(RecycleSelectedTrash);
        recycleAllButton.onClick.AddListener(RecycleAllTrash);
        closeButton.onClick.AddListener(CloseDisplay);
    }

    void PopulateTrashButtons()
    {
        foreach (TrashSO trash in playerTrashInventory)
        {
            GameObject buttonObj = Instantiate(trashButtonPrefab, trashButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            button.image.sprite = trash.trashImage;
            button.GetComponentInChildren<TextMeshProUGUI>().text = trash.name; // Assuming there is a Text component to display the name
            button.onClick.AddListener(() => SelectTrash(trash, button));
        }
    }

    public void SelectTrash(TrashSO trash, Button button)
    {
        if (selectedButton != null)
        {
            SetButtonSelected(selectedButton, false);
        }

        selectedTrash = trash;
        selectedButton = button;
        selectedTrashImage.sprite = trash.trashImage;
        SetButtonSelected(selectedButton, true);

        DisplaySelectedTrash();
    }

    void DisplaySelectedTrash()
    {
        selectedTrashImage.sprite = selectedTrash.trashImage; // Assuming your TrashSO has an image to display
        selectedTrashName.text = selectedTrash.trashName;
    }

    string FormatMaterialsText(Dictionary<string, int> materials)
    {
        string formattedText = "";
        foreach (KeyValuePair<string, int> material in materials)
        {
            formattedText += $"{material.Key}: {material.Value}\n";
        }
        return formattedText;
    }

    void RecycleSelectedTrash()
    {
        if (selectedTrash != null)
        {
            RecycleTrash(selectedTrash);
            playerTrashInventory.Remove(selectedTrash);
            Destroy(selectedButton.gameObject);

            selectedTrash = null;
            selectedButton = null;
            selectedTrashImage.sprite = null;
            selectedTrashName.text = "";
        }
    }

    void RecycleAllTrash()
    {
        foreach (TrashSO trash in playerTrashInventory)
        {
            RecycleTrash(trash);
        }

        playerTrashInventory.Clear();
        foreach (Transform child in trashButtonContainer)
        {
            Destroy(child.gameObject);
        }

        selectedTrash = null;
        selectedButton = null;
        selectedTrashImage.sprite = null;
        selectedTrashName.text = "";
    }

    void RecycleTrash(TrashSO trash)
    {
        inventory.RecycleTrash(trash);
    }

    void SetButtonSelected(Button button, bool isSelected)
    {
        button.interactable = !isSelected;
    }
}
