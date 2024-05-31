using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerLoadout playerLoadout;
    private FishingMechanic fishingMechanic;

    [Header("Fish Catch UI")]
    public GameObject fishCatchUI; // Reference to the UI GameObject
    public Image fishImage;
    public TextMeshProUGUI fishName;
    public string leaveTrigger = "Leave"; // Trigger name to end the animation

    private Animator fishCaughtAnimator;
    private bool isFishCatchUIActive = false;
    private Animator baitPanelAnimator;
    private bool baitPanelIsOpen = false;

    [Header("Bait Choosing UI")]
    public GameObject[] cardOrder;
    public GameObject baitPanel;
    public GameObject worm;
    public GameObject caterpillar;
    public GameObject cricket;
    public GameObject shrimp;
    public GameObject pellet;
    public float transitionDuration = 0.5f;

    private GameObject[] baits;
    private int currentIndex;

    [Header("Menu UI")]
    public GameObject pausePanel;
    public GameObject dayHasEndedPanel;

    private Animator pausePanelAnimator;
    private Animator dayHasEndedAnimator;
    private bool pausePanelIsOpen = false;
    private bool dayHasEndedPanelIsOpen = false;
    void Start()
    {
        playerLoadout = FindAnyObjectByType<PlayerLoadout>();
        fishingMechanic = playerLoadout.GetComponent<FishingMechanic>();

        // Ensure the UI is disabled at start
        fishCaughtAnimator = fishCatchUI.GetComponent<Animator>();
        fishCatchUI.SetActive(false);

        baitPanelAnimator = baitPanel.GetComponent<Animator>();
        baitPanelIsOpen = false;

        baits = new GameObject[] { worm, caterpillar, cricket, shrimp, pellet };
        currentIndex = 2;
        UpdateCardOrder();
        UpdateBaitButtons();

        pausePanelAnimator = pausePanel.GetComponent<Animator>();
        dayHasEndedAnimator = dayHasEndedPanel.GetComponent<Animator>();

        pausePanelIsOpen = false;
        dayHasEndedPanelIsOpen = false;

        pausePanel.SetActive(false);
        dayHasEndedPanel.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        FindPlayerObject();
    }

    private void FindPlayerObject()
    {
        playerLoadout = FindAnyObjectByType<PlayerLoadout>();
        fishingMechanic = playerLoadout.GetComponent<FishingMechanic>();
    }

    void Update()
    {
        // Check for input if the UI is active
        if (isFishCatchUIActive && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(EndFishCaughtUI());
        }

        if (!isFishCatchUIActive && Input.GetKeyDown(KeyCode.Tab))
        {
            OpenBaitPanel();
        }

        if (Input.GetKeyUp(KeyCode.Escape)){
            OpenPauseMenu();
        }
        

    }


    private void OpenBaitPanel()
    {
        if (baitPanelIsOpen)
        {
            baitPanelAnimator.SetTrigger("Hide");

        }
        else
        {
            baitPanelAnimator.SetTrigger("Show");
        }
        baitPanelIsOpen = !baitPanelIsOpen;
    }
    private void OpenPauseMenu()
    {
        if (pausePanelIsOpen)
        {
            pausePanelAnimator.SetTrigger("Hide");

        }
        else
        {
            pausePanelAnimator.SetTrigger("Show");
        }
        baitPanelIsOpen = !baitPanelIsOpen;
    }
    public void SelectBait(GameObject selectedBait)
    {
        int selectedIndex = System.Array.IndexOf(baits, selectedBait);
        if (selectedIndex == -1)
        {
            Debug.LogError("Selected bait is not in the list!");
            return;
        }

        PlayerLoadout.Bait selectedBaitType = (PlayerLoadout.Bait)selectedIndex + 1;
        playerLoadout.SelectBait(selectedBaitType);
        currentIndex = selectedIndex;
        StartCoroutine(UpdateCardOrderWithAnimation());
        UpdateBaitButtons();
    }

    private void UpdateCardOrder()
    {
        int length = baits.Length;
        for (int i = 0; i < length; i++)
        {
            int index = (currentIndex + i - 2 + length) % length;
            baits[index].transform.SetParent(cardOrder[i].transform, false);
            baits[index].transform.localPosition = Vector3.zero; // Ensure they are centered
        }
    }

    private IEnumerator UpdateCardOrderWithAnimation()
    {
        int length = baits.Length;
        Vector3[] startPositions = new Vector3[length];
        Vector3[] targetPositions = new Vector3[length];

        for (int i = 0; i < length; i++)
        {
            int index = (currentIndex + i - 2 + length) % length;
            startPositions[index] = baits[index].transform.position;
            targetPositions[index] = cardOrder[i].transform.position;
        }

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            for (int i = 0; i < length; i++)
            {
                int index = (currentIndex + i - 2 + length) % length;
                baits[index].transform.position = Vector3.Lerp(startPositions[index], targetPositions[index], t);
            }

            yield return null;
        }

        UpdateCardOrder(); // Ensure final positions are set correctly
    }

    private void UpdateBaitButtons()
    {
        UpdateButton(worm);
        UpdateButton(caterpillar);
        UpdateButton(cricket);
        UpdateButton(shrimp);
        UpdateButton(pellet);
    }

    private void UpdateButton(GameObject bait)
    {
        Button button = bait.GetComponentInChildren<Button>();
        PlayerLoadout.Bait baitType = (PlayerLoadout.Bait)System.Array.IndexOf(baits, bait) + 1;

        int amount = playerLoadout.GetBaitAmount(baitType);
        //button.interactable = amount > 0;
        //button.GetComponentInChildren<Text>().text = $"{baitType} ({amount})";
    }

    public void ShowFishCaughtUI(Sprite fishImageNew, string fishNameNew)
    {
        // Enable the UI and start the show animation
        fishImage.sprite = fishImageNew;
        fishName.text = fishNameNew;
        fishCatchUI.SetActive(true);
        isFishCatchUIActive = true;
    }

    private IEnumerator EndFishCaughtUI()
    {
        // Start the leave animation
        fishCaughtAnimator.SetTrigger(leaveTrigger);
        isFishCatchUIActive = false;

        yield return new WaitForSeconds(0.1f);

        fishingMechanic.EnableThrowHook();

        // Wait for the animation to finish
        yield return new WaitForSeconds(5f);

        // Disable the UI
        fishCatchUI.SetActive(false);
    }
}
