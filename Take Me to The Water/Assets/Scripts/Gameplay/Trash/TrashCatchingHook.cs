using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCatchingHook : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;
    private float lifeTime = 10;
    private float currentTime = 0f;
    private bool isReturning = false;

    private Trash capturedTrashContainer = null;
    private PlayerInventory playerInventory;

    public void Initialize(Vector3 targetPosition, float speed)
    {
        this.targetPosition = targetPosition;
        this.speed = speed;
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (!isReturning)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isReturning = true;
                targetPosition = transform.parent.position;
            }
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                if (capturedTrashContainer != null)
                {
                    playerInventory.AddTrash(capturedTrashContainer.GetTrashSO());
                }
                Destroy(gameObject);
            }
        }

        if(currentTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Trash capturedTrash = other.gameObject.GetComponent<Trash>();
        if (capturedTrash)
        {
            PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

            if (capturedTrash != null && playerInventory != null)
            {
                capturedTrashContainer = capturedTrash;
                other.transform.parent = this.gameObject.transform;
                targetPosition = transform.parent.position;
            }
        }
    }
}
