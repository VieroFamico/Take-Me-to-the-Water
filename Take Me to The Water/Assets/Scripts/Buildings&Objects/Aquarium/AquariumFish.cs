using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AquariumFish : MonoBehaviour
{
    public FishSO fishData;
    public float moveSpeed = 2f;
    public float verticalMoveLimit = 2f; // Half the horizontal move distance
    public float foodDetectionRadius = 5f; // Radius to detect food
    public float eatingDistance = 0.5f; // Distance at which the fish eats the food
    public Transform bitePosition; // Position to check for food
    public RectTransform fishContainer; // Container for the fish to move within
    public float padding = 10f; // Padding within the fish container

    private Vector3 targetPosition;
    private GameObject nearestFood;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        SetBounds();
        StartCoroutine(MoveRandomly());
    }

    void Update()
    {
        CheckForFood();
        MoveTowardsTarget();
    }

    void SetBounds()
    {
        if (fishContainer != null)
        {
            Vector3[] corners = new Vector3[4];
            fishContainer.GetWorldCorners(corners);

            minBounds = corners[0] + new Vector3(padding, padding, 0);
            maxBounds = corners[2] - new Vector3(padding, padding, 0);
        }
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            SetRandomTargetPosition();
            yield return new WaitForSeconds(Random.Range(2f, 5f)); // Change direction every 2 to 5 seconds
        }
    }

    void SetRandomTargetPosition()
    {
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        targetPosition = new Vector3(randomX, randomY, transform.position.z);
    }

    void MoveTowardsTarget()
    {
        if (targetPosition != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position.x < targetPosition.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(transform.position.x > targetPosition.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    void CheckForFood()
    {
        if (nearestFood == null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bitePosition.position, foodDetectionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Food"))
                {
                    nearestFood = hitCollider.gameObject;
                    MoveTowardsFood(nearestFood.transform.position);
                    break;
                }
            }
        }
        else
        {
            float distanceToFood = Vector3.Distance(bitePosition.position, nearestFood.transform.position);
            if (distanceToFood < eatingDistance)
            {
                EatFood();
            }
        }
    }

    void MoveTowardsFood(Vector3 foodPosition)
    {
        targetPosition = foodPosition;
    }

    void EatFood()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bitePosition.position, eatingDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Food"))
            {
                Destroy(hitCollider.gameObject);
            }
        }
        nearestFood = null;
        SetRandomTargetPosition(); // Go back to moving randomly
    }
}
