using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour
{
    public TrashSO trashSO;

    private float moveSpeed = 5f;
    private Vector3 moveDirection;

    private void Update()
    {
        // Move the trash object in the specified direction
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public TrashSO GetTrashSO()
    {
        return trashSO;
    }
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
