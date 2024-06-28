using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenuManager : MonoBehaviour
{
    [Header("Flag")]
    public Flag flag;

    [Header("UI References")]
    public GameObject settingsPanel;
    public GameObject startMenuPanel;
    public GameObject settingsSection;
    public GameObject fishDexSection;
    public Button settingsButton;
    public Button exitToStartMenuButton;
    public Button quitGameButton;
    public Button startGameButton;
    public Button closeSettingButton;
    public Button settingsTabButton;
    public Button fishDexTabButton;
    public Slider generalSoundSlider;
    public Slider musicSoundSlider;

    public Button TutorialSceneButton;

    [Header("To Turn Off")]
    public GameObject dissapearUI;

    [Header("Animator References")]
    public Animator settingsPanelAnimator;
    public Animator startMenuPanelAnimator;

    [Header("Sprites")]
    public Sprite settingsSelectedTabSprite;
    public Sprite settingsUnselectedTabSprite;
    public Sprite fishDexSelectedTabSprite;
    public Sprite fishDexUnselectedTabSprite;

    [Header("Player References")]
    private HomePlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;

    [Header("Camera References")]
    public CinemachineVirtualCamera virtualCamera;
    public Transform position1;
    public Transform position2;
    public Transform playerTransform;
    public float moveSpeed = 2f;

    private Transform cameraTarget;
    private Transform currentTarget;
    private bool isMoving = true;

    private void Start()
    {
        // Find the player movement and interaction scripts
        playerMovement = FindAnyObjectByType<HomePlayerMovement>();
        playerInteraction = FindAnyObjectByType<PlayerInteraction>();

        // Add listeners to buttons
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        closeSettingButton.onClick.AddListener(CloseSettingsPanel);
        exitToStartMenuButton.onClick.AddListener(OpenStartMenuPanel);
        startGameButton.onClick.AddListener(CloseStartMenuPanel);
        quitGameButton.onClick.AddListener(QuitGame);

        settingsTabButton.onClick.AddListener(ShowSettingsSection);
        fishDexTabButton.onClick.AddListener(ShowFishDexSection);

        //TutorialSceneButton.onClick.AddListener()

        // Set initial tab
        ShowSettingsSection();

        // Initially hide and disable settings panel
        settingsPanel.SetActive(false);
        startMenuPanel.SetActive(true);

        InitializeGame(flag) ;
    }
    private void InitializeGame(Flag flag)
    {
        if(flag.GetMainMenuFlag() != 0)
        {
            return;
        }
        OpenStartMenuPanel();
        playerMovement.ChangeMove();
        playerInteraction.enabled = false;
        BlurEffectForPanel.ToggleBlur();
        dissapearUI.SetActive(false);

        // Create and set up the camera target
        cameraTarget = new GameObject("CameraTarget").transform;
        cameraTarget.position = position1.position;
        virtualCamera.Follow = cameraTarget;
        currentTarget = position2;

        flag.ActivateMainMenuFlag();
    }
    private void Update()
    {
        if (isMoving)
        {
            MoveCameraTarget();
        }
    }

    private void MoveCameraTarget()
    {
        if (cameraTarget != null)
        {
            float step = moveSpeed * Time.deltaTime;
            cameraTarget.position = Vector3.MoveTowards(cameraTarget.position, currentTarget.position, step);

            if (Vector3.Distance(cameraTarget.position, currentTarget.position) < 0.1f)
            {
                currentTarget = (currentTarget == position1) ? position2 : position1;
            }
        }
    }

    public void OpenSettingsPanel()
    {
        // Enable the settings panel
        settingsPanel.SetActive(true);

        // Disable player movement and interaction
        playerMovement.ChangeMove();
        playerInteraction.enabled = false;

        // Show the settings panel
        settingsPanelAnimator.SetTrigger("Show");

        dissapearUI.SetActive(false);
        BlurEffectForPanel.ToggleBlur();
    }

    public void CloseSettingsPanel()
    {
        // Enable player movement and interaction
        playerMovement.ChangeMove();
        playerInteraction.enabled = true;

        // Hide the settings panel
        settingsPanelAnimator.SetTrigger("Hide");
        StartCoroutine(DisablePanelAfterAnimation(settingsPanelAnimator, settingsPanel));
        BlurEffectForPanel.ToggleBlur();

        dissapearUI.SetActive(true);
    }

    public void OpenStartMenuPanel()
    {
        // Hide and disable the settings panel if it's active
        if (settingsPanel.activeSelf)
        {
            settingsPanelAnimator.SetTrigger("Hide");
            StartCoroutine(DisablePanelAfterAnimation(settingsPanelAnimator, settingsPanel));
        }

        // Enable the start menu panel
        startMenuPanel.SetActive(true);

        // Show the start menu panel
        startMenuPanelAnimator.SetTrigger("Show");

        dissapearUI.SetActive(false);
    }

    public void CloseStartMenuPanel()
    {
        // Hide the start menu panel
        startMenuPanelAnimator.SetTrigger("Hide");
        StartCoroutine(DisablePanelAfterAnimation(startMenuPanelAnimator, startMenuPanel));

        // Enable player movement and interaction
        playerMovement.ChangeMove();
        playerInteraction.enabled = true;
        BlurEffectForPanel.ToggleBlur();

        dissapearUI.SetActive(true);

        // Stop camera movement and focus on the player
        isMoving = false;
        virtualCamera.Follow = playerTransform;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ShowSettingsSection()
    {
        settingsSection.SetActive(true);
        fishDexSection.SetActive(false);

        settingsTabButton.image.sprite = settingsSelectedTabSprite;
        fishDexTabButton.image.sprite = fishDexUnselectedTabSprite;
    }

    private void ShowFishDexSection()
    {
        settingsSection.SetActive(false);
        fishDexSection.SetActive(true);

        settingsTabButton.image.sprite = settingsUnselectedTabSprite;
        fishDexTabButton.image.sprite = fishDexSelectedTabSprite;
    }

    private IEnumerator DisablePanelAfterAnimation(Animator animator, GameObject panel)
    {
        // Wait until the animation has finished playing
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Disable the panel
        panel.SetActive(false);
    }

}
