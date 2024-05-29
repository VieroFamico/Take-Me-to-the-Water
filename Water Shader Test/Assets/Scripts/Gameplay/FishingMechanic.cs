using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingMechanic : MonoBehaviour
{
    [Header("Dependencies")]
    public UIManager uiManager;

    [Header("Fishing Hook")]
    public LineRenderer throwLineRenderer;
    public float maxThrowDistance = 30f;
    public float throwDistanceIncreaseSpeed = 5f;
    public float maxThrowForce = 20f;
    public Transform player;
    public GameObject hookPrefab;
    private GameObject hookInstance;
    private Rigidbody hookRigidbody;
    private LineRenderer fishingLineRenderer;

    [Header("Fishing Mechanics")]
    public Slider tensionSlider;
    public Image tensionSliderImage;
    public Color lowTensionColor;
    public Color highTensionColor;
    public float tensionIncreaseRate = 10f;
    public float tensionDecreaseRate = 5f;
    public float maxTension = 100f;
    public float startingTension = 50f; // Starting amount of tension
    public float pullSpeed = 2f;
    public float driftSpeed = 0.5f;
    public float catchDistance = 1.5f;
    public float catchCheckDelay = 0.5f; // Delay before checking for catch
    public float stopSpeedThreshold = 0.1f; // Threshold speed to consider the hook as stopped

    [Header("Cinemachine")]
    public CinemachineVirtualCamera vCam;
    public float zoomOutMaxDistance = 15f;
    private float originalCameraSize;

    private bool canThrowHook = true;
    private float hookThrowDelay = 0.2f;
    private float boatDecelerationRate = 2f; // Rate at which the boat slows down
    private bool isThrowing = false;
    private bool isFishing = false;
    private bool fishCaught = false;
    private bool canCheckCatch = false;
    private bool fishIsEatingTheBait = false; // Is a fish eating the bait
    private Vector3 hookStartPosition;
    private Vector3 hookTargetPosition;
    private float currentThrowDistance = 0f;
    private Camera mainCamera;
    private Fish fish;// Reference to the fish caught
    private BoatMovement boatMovement;
    private Animator tensionSliderAnimator;

    private float modeChangeDelay = 0.5f;
    private float lastModeChangeTime;
    public enum Modes
    {
        Moving,
        Fishing
    }
    private Modes currentMode = Modes.Moving;

    void Start()
    {
        boatMovement = GetComponent<BoatMovement>();


        throwLineRenderer.positionCount = 2;
        throwLineRenderer.enabled = false; // Initially disable the throw line renderer

        tensionSlider.gameObject.SetActive(true);
        tensionSlider.maxValue = maxTension;
        tensionSliderAnimator = tensionSlider.GetComponent<Animator>();
        SetMode(Modes.Moving);

        mainCamera = Camera.main;
        originalCameraSize = vCam.m_Lens.OrthographicSize;
    }

    void Update()
    {
        if (Time.time - lastModeChangeTime >= modeChangeDelay && Input.GetMouseButtonDown(1))
        {
            if (currentMode == Modes.Moving)
            {
                SetMode(Modes.Fishing);
            }
            else
            {
                SetMode(Modes.Moving);
            }
        }

        if (currentMode == Modes.Fishing)
        {
            if (!hookInstance && canThrowHook)
            {
                HandleThrowingInput();
            }
            Debug.Log(canThrowHook);
            if (isFishing && hookInstance && !fishCaught)
            {
                if (isFishing || isThrowing)
                {
                    UpdateFishingLine();
                    UpdateCameraZoom();
                }

                if (fishIsEatingTheBait)
                {
                    HandleFishingInput();
                }
                if (canCheckCatch)
                {
                    CheckCatch();

                    tensionSliderImage.color = Color.Lerp(lowTensionColor, highTensionColor, tensionSlider.value / maxTension);
                }
            }
        }

        

        if (!isFishing && boatMovement != null)
        {
            boatMovement.GraduallyStopBoat(boatDecelerationRate);
        }
    }

    void SetMode(Modes mode)
    {
        currentMode = mode;
        isFishing = (mode == Modes.Fishing);
        lastModeChangeTime = Time.time;

        if (mode == Modes.Moving)
        {
            ResumeBoat();
            tensionSliderAnimator.SetTrigger("Hide");

            if (hookInstance)
            {
                canCheckCatch = false;
                Destroy(hookInstance);
                if (fish)
                {
                    // Release the fish
                    fish.Released();
                    fish.transform.SetParent(null);
                    fish = null;
                }
                vCam.Follow = player.transform;
                vCam.m_Lens.OrthographicSize = originalCameraSize;
            }
            throwLineRenderer.SetPosition(0, player.position);
            throwLineRenderer.SetPosition(1, player.position);
            throwLineRenderer.enabled = false;
        }
        else
        {
            StopBoat();
            tensionSliderAnimator.SetTrigger("Show");
            StartCoroutine(EnableHookThrowAfterDelay());
        }
    }

    void StopBoat()
    {
        if (boatMovement != null)
        {
            boatMovement.StopBoat();
        }
    }

    void ResumeBoat()
    {
        if (boatMovement != null)
        {
            boatMovement.StartBoat();
        }
    }
    IEnumerator EnableHookThrowAfterDelay()
    {
        canThrowHook = false;
        yield return new WaitForSeconds(hookThrowDelay);
        canThrowHook = true;
    }

    void HandleThrowingInput()
    {
        if (Input.GetMouseButtonDown(0) && !isThrowing)
        {
            StartThrow();
        }

        if (Input.GetMouseButton(0) && isThrowing)
        {
            ContinueThrow();
        }

        if (Input.GetMouseButtonUp(0) && isThrowing)
        {
            FinishThrow();
        }
    }

    void HandleFishingInput()
    {
        if (Input.GetMouseButton(0))
        {
            IncreaseTension();
            PullHookTowardsPlayer();
        }
        else
        {
            DecreaseTension();
            DriftHookAway();
        }

        if (tensionSlider.value >= maxTension)
        {
            BreakLine();
        }
    }

    void StartThrow()
    {
        hookStartPosition = player.position;
        isThrowing = true;
        currentThrowDistance = 0f;

        // Enable the throw line renderer to show the throwing line
        throwLineRenderer.enabled = true;
    }

    void ContinueThrow()
    {
        currentThrowDistance += throwDistanceIncreaseSpeed * Time.deltaTime;
        currentThrowDistance = Mathf.Clamp(currentThrowDistance, 0, maxThrowDistance);

        DrawThrowLine();
    }

    void DrawThrowLine()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.y = player.position.y; // Keep the hook on the same horizontal plane

        Vector3 direction = (worldPosition - player.position).normalized;
        hookTargetPosition = player.position + direction * currentThrowDistance;

        throwLineRenderer.SetPosition(0, player.position); // Always start from the player's current position
        throwLineRenderer.SetPosition(1, hookTargetPosition);
    }

    void FinishThrow()
    {
        if(currentThrowDistance < 1f)
        {
            return;
        }

        isThrowing = false;
        canThrowHook = false;

        // Instantiate the hook at the player's position
        hookInstance = Instantiate(hookPrefab, player.position, Quaternion.identity);
        hookRigidbody = hookInstance.GetComponent<Rigidbody>();
        fishingLineRenderer = hookInstance.GetComponent<LineRenderer>();
        hookInstance.transform.rotation = transform.rotation;
        hookInstance.transform.parent = this.transform;

        // Calculate the throw speed based on the throw distance
        float throwForce = (currentThrowDistance / maxThrowDistance) * maxThrowForce;

        // Apply force to the hook to simulate the throw 
        Vector3 direction = (hookTargetPosition - player.position).normalized;
        hookRigidbody.velocity = direction * throwForce;

        // Set the Cinemachine VCam to follow the hook
        vCam.Follow = hookInstance.transform;

        // Disable the throw line renderer after the hook is thrown
        throwLineRenderer.enabled = false;

        // Enable the fishing line renderer
        fishingLineRenderer.enabled = true;

        Invoke("EnableCatchCheck", catchCheckDelay); // Enable catch check after a delay
    }

    void EnableCatchCheck()
    {
        canCheckCatch = true;
    }

    void UpdateFishingLine()
    {
        if (hookInstance && fishingLineRenderer)
        {
            fishingLineRenderer.SetPosition(0, player.position);
            fishingLineRenderer.SetPosition(1, hookInstance.transform.position);
        }
    }

    void UpdateCameraZoom()
    {
        if (hookInstance)
        {
            float distance = Vector3.Distance(player.position, hookInstance.transform.position);
            float targetSize = Mathf.Lerp(originalCameraSize, zoomOutMaxDistance, distance / maxThrowDistance);
            vCam.m_Lens.OrthographicSize = targetSize;
        }
    }

    void IncreaseTension()
    {
        tensionSlider.value += tensionIncreaseRate * Time.deltaTime;
    }

    void DecreaseTension()
    {
        tensionSlider.value -= tensionDecreaseRate * Time.deltaTime;
    }

    void PullHookTowardsPlayer()
    {
        Vector3 direction = (player.position - hookInstance.transform.position).normalized;
        hookRigidbody.velocity = direction * pullSpeed;
    }

    void DriftHookAway()
    {
        Vector3 direction = (hookInstance.transform.position - player.position).normalized;
        hookRigidbody.velocity = direction * driftSpeed;
    }

    void BreakLine()
    {
        Debug.Log("The line broke! The fish escaped.");
        isFishing = false;
        canThrowHook = true;
        tensionSlider.value = startingTension;
        vCam.Follow = player.transform;
        vCam.m_Lens.OrthographicSize = originalCameraSize;
        tensionSlider.gameObject.SetActive(false);

        fish.Released();

        if (fishingLineRenderer)
        {
            fishingLineRenderer.SetPosition(0, player.position);
            fishingLineRenderer.SetPosition(1, player.position);
            fishingLineRenderer.enabled = false; // Disable the fishing line renderer
        }
        Destroy(hookInstance); // Destroy the hook instance
    }

    void CheckCatch()
    {
        if (!hookInstance)
        {
            return;
        }
        if (Vector3.Distance(hookInstance.transform.position, player.position) <= catchDistance)
        {
            CatchFish();
        }
    }

    void CatchFish()
    {
        Debug.Log("Fish caught!");
        fishCaught = true;
        isFishing = false;
        fishIsEatingTheBait = false; // Reset the fishIsEatingTheBait flag
        vCam.Follow = player.transform;
        vCam.m_Lens.OrthographicSize = originalCameraSize;
        canCheckCatch = false;
        uiManager.ShowFishCaughtUI(fish.fishImage, fish.fishName);

        if (fishingLineRenderer)
        {
            fishingLineRenderer.SetPosition(0, player.position);
            fishingLineRenderer.SetPosition(1, player.position);
            fishingLineRenderer.enabled = false; // Disable the fishing line renderer
        }
        
        Destroy(hookInstance); // Destroy the hook instance

        if (fish)
        {
            Destroy(fish.gameObject);
        }
    }

    public void EnableThrowHook()
    {
        canThrowHook = true;
    }

    public void StartFishing(Fish fishTransform)
    {
        isFishing = true;
        fishCaught = false;
        tensionSlider.value = startingTension;
        fishIsEatingTheBait = true;
        canCheckCatch = true;
        fish = fishTransform;
    }

    bool HookIsStopped()
    {
        return hookRigidbody.velocity.magnitude <= stopSpeedThreshold;
    }
}
