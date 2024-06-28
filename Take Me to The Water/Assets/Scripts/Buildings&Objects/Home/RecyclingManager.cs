using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecyclingManager : BuildingManager
{
    public TrashSO temp;
    [Header("UI References")]
    public GameObject trashButtonPrefab;
    public Transform trashButtonContainer;
    public Image selectedTrashImage;
    public TextMeshProUGUI selectedTrashName;
    public Button recycleButton;
    public Button recycleAllButton;

    [Header("Materials UI")]
    public Image plasticImage;
    public Image metalImage;
    public Image woodImage;
    public Image rubberImage;
    public TextMeshProUGUI plasticAmountText;
    public TextMeshProUGUI metalAmountText;
    public TextMeshProUGUI woodAmountText;
    public TextMeshProUGUI rubberAmountText;

    [Header("Player Inventory")]
    public PlayerInventory inventory;

    private List<TrashSO> playerTrashInventory = new List<TrashSO>(); // Populate this in the inspector or through another script
    private TrashSO selectedTrash;
    private Button selectedButton;
    private int flag = 0;

    private void Update()
    {
        if(flag > 0)
        {
            return;
        }
        inventory = FindAnyObjectByType<PlayerInventory>();
        Debug.Log(inventory);
        Debug.Log(inventory.GetPlayerTrashInventory());
        playerTrashInventory = inventory.GetPlayerTrashInventory().GetTrashList();

        PopulateTrashButtons();
        DeactivateImage();

        recycleButton.onClick.AddListener(RecycleSelectedTrash);
        recycleAllButton.onClick.AddListener(RecycleAllTrash);
        closeButton.onClick.AddListener(CloseDisplay);
        flag++;
    }

    void PopulateTrashButtons()
    {
        foreach (TrashSO trash in playerTrashInventory)
        {
            GameObject buttonObj = Instantiate(trashButtonPrefab, trashButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            button.image.sprite = trash.trashImage;
            button.GetComponentInChildren<TextMeshProUGUI>().text = trash.trashName; // Assuming there is a Text component to display the name
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
        selectedTrashImage.gameObject.SetActive(true);
        selectedTrashImage.sprite = selectedTrash.trashImage; // Assuming your TrashSO has an image to display
        selectedTrashName.text = selectedTrash.trashName;

        DisplayMaterialInfo();
    }
    void DisplayMaterialInfo()
    {
        plasticAmountText.text = selectedTrash.plasticAmount.ToString();
        metalAmountText.text = selectedTrash.metalAmount.ToString();
        woodAmountText.text = selectedTrash.woodAmount.ToString();
        rubberAmountText.text = selectedTrash.rubberAmount.ToString();

        plasticImage.color = selectedTrash.plasticAmount > 0 ? Color.white : Color.gray;
        metalImage.color = selectedTrash.metalAmount > 0 ? Color.white : Color.gray;
        woodImage.color = selectedTrash.woodAmount > 0 ? Color.white : Color.gray;
        rubberImage.color = selectedTrash.rubberAmount > 0 ? Color.white : Color.gray;
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
            selectedTrashImage.gameObject.SetActive(false);
            selectedTrashName.text = "";

            ClearMaterialInfo();
        }
    }

    private void DeactivateImage()
    {
        selectedTrashImage.gameObject.SetActive(false);
        selectedTrashName.text = "";
    }

    void RecycleAllTrash()
    {
        int temp = playerTrashInventory.Count - 1;
        while(temp >=0)
        {
            RecycleTrash(playerTrashInventory[temp]);
            temp --;
        }

        playerTrashInventory.Clear();
        foreach (Transform child in trashButtonContainer)
        {
            Destroy(child.gameObject);
        }

        selectedTrash = null;
        selectedButton = null;
        selectedTrashImage.gameObject.SetActive(false);
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

    void ClearMaterialInfo()
    {
        plasticAmountText.text = "";
        metalAmountText.text = "";
        woodAmountText.text = "";
        rubberAmountText.text = "";

        plasticImage.color = Color.gray;
        metalImage.color = Color.gray;
        woodImage.color = Color.gray;
        rubberImage.color = Color.gray;
    }

}
