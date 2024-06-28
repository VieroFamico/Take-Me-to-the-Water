using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMenuManager : MonoBehaviour
{
    [Header("References")]
    public GameObject MapPanel;
    public GameObject LoadoutPanel;
    public Button MapButton;
    public Button LoadoutButton;
    public Button nextButton;
    public Image NextButtonImage;
    public TextMeshProUGUI warningText;

    [Header("Buttons Sprite")]
    public Sprite MapButtonActiveSprite;
    public Sprite MapButtonInactiveSprite;
    public Sprite LoadoutButtonActiveSprite;
    public Sprite LoadoutButtonInactiveSprite;
    public Sprite NextButtonSprite;
    public Sprite GoButtonSprite;

    [Header("Map Buttons")]
    public Button[] navigationButtons;

    private bool isMapPanelActive = true;
    private int choosenIndex = 0;

    void Start()
    {
        for (int i = 0; i < navigationButtons.Length; i++)
        {
            int index = i + 1;
            navigationButtons[i].onClick.AddListener(() => SelectLocation(index));
        }

        ShowMapPanel();
        MapButton.onClick.AddListener(ShowMapPanel);
        LoadoutButton.onClick.AddListener(ShowLoadoutPanel);
        nextButton.onClick.AddListener(ChangePanel);
        warningText.enabled = false;
        UpdateButtonSprites();
    }

    void ShowMapPanel()
    {
        MapPanel.SetActive(true);
        LoadoutPanel.SetActive(false);
        isMapPanelActive = true;
        UpdateButtonSprites();
        NextButtonImage.sprite = NextButtonSprite;
    }

    void ShowLoadoutPanel()
    {
        MapPanel.SetActive(false);
        LoadoutPanel.SetActive(true);
        isMapPanelActive = false;
        UpdateButtonSprites();
        GetComponent<PlayerLoadoutDisplayManager>().UpdateSprites();
        NextButtonImage.sprite = GoButtonSprite;
    }
    void ChangePanel()
    {
        if (isMapPanelActive)
        {
            ShowLoadoutPanel();
        }
        else
        {
            ChangeScene();
        }
    }
    void UpdateButtonSprites()
    {
        MapButton.image.sprite = isMapPanelActive ? MapButtonActiveSprite : MapButtonInactiveSprite;
        LoadoutButton.image.sprite = isMapPanelActive ? LoadoutButtonInactiveSprite : LoadoutButtonActiveSprite;
    }

    public void SelectLocation(int index)
    {
        choosenIndex = index;
    }
    public void ChangeScene()
    {
        if (FindAnyObjectByType<PlayerLoadout>().GetCurrentShipFuel() <= 1)
        {
            StartCoroutine(ShowWarning("Not Enough Fuel, \n Refuel and Then Cobe Back"));
            return;
        }
        
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentBuildIndex + choosenIndex == currentBuildIndex || choosenIndex <= 0)
        {
            return;
        }
        SceneTransitionManager.Instance.TransitionToScene(currentBuildIndex + choosenIndex);
    }
    IEnumerator ShowWarning(string message)
    {
        // Enable the text and set properties
        warningText.enabled = true;
        warningText.text = message;
        warningText.fontSize = 80;
        warningText.fontStyle = FontStyles.Bold;
        yield return new WaitForSeconds(2);

        warningText.gameObject.SetActive(false);
    }
}
