using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePlayerMovement : MonoBehaviour
{
    public float moveSpeed; 

    private Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if(Mathf.Abs(moveInput) > 0)
        {
            float movement = moveInput >= 0 ? moveSpeed : -moveSpeed;

            rb2d.velocity = new Vector2(movement, 0f);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, 10f);
        }
    }
}
