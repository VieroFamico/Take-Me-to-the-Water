using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Fish : MonoBehaviour
{
    public float swimSpeed = 2f;
    public float rotationSpeed = 2f;
    public float detectionRange = 5f;
    public float biteChance = 0.5f; // 50% chance to bite the hook
    public Transform fishSprite;
    public Transform bitePoint;

    private Vector3 destination;
    private Transform hook;
    private FishingMechanic fishingMechanic;
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
            else
            {

            }
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
        if (Vector3.Distance(transform.position, destination) < 1f)
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
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Hook"))
                {
                    hook = collider.transform;
                    destination = hook.transform.position;
                    fishingMechanic = hook.GetComponentInParent<FishingMechanic>();
                    break;
                }
            }
        }
    }

    void MoveTowardsHook()
    {
        Vector3 direction = (hook.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * swimSpeed * Time.deltaTime;

        if (Vector3.Distance(bitePoint.position, hook.position) < 2f)
        {
            if (Random.value < biteChance)
            {
                BiteHook();
            }
            else
            {
                ResetBehavior();
            }
        }
    }

    void BiteHook()
    {
        fishingMechanic.StartFishing(transform);
        transform.SetParent(hook);
        Vector3 target = hook.position;
        target.y = transform.position.y;
        transform.position = target - (bitePoint.position - transform.position);
        isCaught = true;
    }

    void ResetBehavior()
    {
        hook = null;
        fishingMechanic = null;
        isCaught = false;
        SetNewDestination();
    }
}