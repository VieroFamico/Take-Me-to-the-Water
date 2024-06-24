using System.Collections;
using System.Collections.Generic;
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
        NextButtonImage.sprite = GoButtonSprite;
    }
    void ChangePanel()
    {
        if (isMapPanelActive)
        {
            ShowLoadoutPanel();
            Debug.Log("b");
        }
        else
        {
            ChangeScene();
            Debug.Log("c");
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
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentBuildIndex + choosenIndex == currentBuildIndex || choosenIndex <= 0)
        {
            return;
        }
        SceneTransitionManager.Instance.TransitionToScene(currentBuildIndex + choosenIndex);
    }
}
