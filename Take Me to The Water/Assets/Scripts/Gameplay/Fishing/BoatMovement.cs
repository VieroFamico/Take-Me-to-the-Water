using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] float forwardMaxSpeed = 20f; // Forward Maximum speed
    [SerializeField] float forwardAcceleration = 20f; // Forward Acceleration
    [SerializeField] float backwardMaxSpeed = 20f; // Backward Maximum speed
    [SerializeField] float backwardAcceleration = 20f; // Backward Acceleration 
    [SerializeField] float turnTorque = 50f; // Turn Speed
    [SerializeField] float drag = 1f;
    [SerializeField] float angularDrag = 2f;

    private float speed = 1;
    private Rigidbody rb;
    bool ableToMove;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.drag = drag;
        rb.angularDrag = angularDrag;

        ableToMove = true;
    }

    void FixedUpdate()
    {
        if (ableToMove)
        {
            Vector2 movement = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

            float moveInput = movement.x;
            float turnInput = movement.y;

            // Determine acceleration and maximum speed based on the direction
            float acceleration = moveInput >= 0 ? forwardAcceleration : backwardAcceleration;
            float maxSpeed = moveInput >= 0 ? forwardMaxSpeed : backwardMaxSpeed;

            float currSpeed = moveInput * acceleration * Time.fixedDeltaTime;
            rb.AddRelativeForce(Vector3.right * moveInput * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed) * speed; // Clamp the ship's velocity to the maximum speed

            rb.AddTorque(Vector3.up * turnInput * turnTorque * Time.fixedDeltaTime * currSpeed * 2); // Apply torque for turning
        }
    }

    public void StopBoat()
    {
        ableToMove = false;
    }

    public void StartBoat()
    {
        ableToMove = true;
    }
    public void GraduallyStopBoat(float decelerationRate)
    {
        if (ableToMove)
        {
            return; // Only apply deceleration when the boat is not allowed to move
        }

        if (rb.velocity.magnitude > 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * decelerationRate);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
