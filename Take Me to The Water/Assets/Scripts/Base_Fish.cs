using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Fish : MonoBehaviour
{
    public float swimSpeed = 2f;
    public float rotationSpeed = 2f;
    public float detectionRange = 5f;
    public float biteChance = 0.5f; // 50% chance to bite the hook
    public FishData fishData;
    public Transform fishSpriteGameObject;
    public Transform bitePoint;

    private Vector3 destination;
    private Transform hook;
    private FishingMechanic fishingMechanic;
    private HookManager hookManager;
    private bool isCaught = false;

    void Start()
    {
        SetNewDestination();
        hook = null;
    }

    void Update()
    {
        if (!hook)
        {
            Move();
            DetectHook();
        }
        else
        {
            if (!isCaught)
            {
                MoveTowardsHook();
            }
            // The rest of the fish's behavior if it is caught can be handled here
        }
    }

    void SetNewDestination()
    {
        float x = Random.Range(0f, 1f) < 0.5f ? Random.Range(-10f, -5f) : Random.Range(5f, 10f);
        float z = Random.Range(0f, 1f) < 0.5f ? Random.Range(-10f, -5f) : Random.Range(5f, 10f);
        destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, destination) < 2.5f)
        {
            SetNewDestination();
        }

        Vector3 direction = (destination - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * swimSpeed * Time.deltaTime;
    }

    void DetectHook()
    {
        if (!hook)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
            HookManager closestHookManager = null;
            float closestDistance = float.MaxValue;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Hook"))
                {
                    HookManager potentialHookManager = collider.GetComponentInParent<HookManager>();
                    FishingMechanic potentialFishingMechanic = collider.GetComponentInParent<FishingMechanic>();
                    float distance = Vector3.Distance(transform.position, collider.transform.position);

                    if (potentialFishingMechanic != null && potentialFishingMechanic.HookIsStopped() &&
                        potentialHookManager != null && !potentialHookManager.IsTargeted &&
                        distance < closestDistance)
                    {
                        closestHookManager = potentialHookManager;
                        closestDistance = distance;
                    }
                }
            }

            if (closestHookManager != null)
            {
                hook = closestHookManager.transform;
                destination = hook.position;
                fishingMechanic = hook.GetComponentInParent<FishingMechanic>();
                hookManager = closestHookManager;
                hookManager.TargetFish = this;
            }
        }
    }

    void MoveTowardsHook()
    {
        Vector3 direction = (hook.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * swimSpeed * Time.deltaTime;

        if (Vector3.Distance(bitePoint.position, hook.position) < 2.7f)
        {
            if (Random.value < biteChance)
            {
                BiteHook();
            }
            else
            {
                // The fish didn't bite, you can add additional behavior here if needed
            }
        }
    }

    void BiteHook()
    {
        fishingMechanic.StartFishing(this);
        transform.SetParent(hook);
        Vector3 target = hook.position;
        target.y = transform.position.y;
        transform.position = target - (bitePoint.position - transform.position);
        isCaught = true;
    }

    public void StopTargetingHook()
    {
        hook = null;
        fishingMechanic = null;
        hookManager = null;
        SetNewDestination();
    }

    public void Released()
    {
        if (hookManager != null)
        {
            hookManager.ReleaseTarget();
        }
        hook = null;
        fishingMechanic = null;
        isCaught = false;
        SetNewDestination();
    }

}