using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashCollectingMechanic : MonoBehaviour
{
    [Header("Dependencies")]
    public LineRenderer aimLineRenderer;
    public GameObject trashCatchingHookPrefab;
    public float hookSpeed = 1f;
    public float fixedDistance = 10f;
    public ShipStateManager shipStateManager;

    private Camera mainCamera;
    private TrashCatchingHook trashCatchingHookGO;

    void Start()
    {
        mainCamera = Camera.main;
        aimLineRenderer.enabled = false;
        trashCatchingHookGO = null;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Only handle input if in TrashCollecting mode
        if (trashCatchingHookGO == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartTrashCollecting();
            }

            if (Input.GetMouseButton(0))
            {
                DisplayAimLine();
            }

            if (Input.GetMouseButtonUp(0))
            {
                LaunchTrashCatchingHook();
            }
        }
    }

    private void DisplayAimLine()
    {
        aimLineRenderer.enabled = true;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.y = transform.position.y;

        Vector3 playerPosition = transform.position;
        Vector3 direction = (worldPosition - playerPosition).normalized;
        Vector3 endPoint = playerPosition + (direction * fixedDistance);

        aimLineRenderer.SetPosition(0, playerPosition);
        aimLineRenderer.SetPosition(1, endPoint);

    }

    private void LaunchTrashCatchingHook()
    {
        aimLineRenderer.enabled = false;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.y = transform.position.y;

        Vector3 playerPosition = transform.position;
        Vector3 direction = (worldPosition - playerPosition).normalized;
        Vector3 targetPosition = playerPosition + (direction * fixedDistance);

        Quaternion rot = Quaternion.LookRotation(targetPosition);
        GameObject hook = Instantiate(trashCatchingHookPrefab, playerPosition, Quaternion.identity);
        trashCatchingHookGO = hook.GetComponent<TrashCatchingHook>();
        trashCatchingHookGO.Initialize(targetPosition * 1.2f, hookSpeed);
        trashCatchingHookGO.transform.parent = this.transform;
        trashCatchingHookGO.transform.rotation = Quaternion.Euler(90f, 0f, rot.y);
        ExitTrashCollectingMode();
    }

    public void StartTrashCollecting()
    {
        
        // Custom logic to enter trash collecting mode, if any
    }

    public void ExitTrashCollectingMode()
    {
        aimLineRenderer.enabled = false;
        // Custom logic to exit trash collecting mode, if any
    }
}
