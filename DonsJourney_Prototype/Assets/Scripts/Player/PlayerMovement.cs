using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool swimming = false;

    [SerializeField] private float maxSwimSpeed = 12.0f;
    [SerializeField] private float swimSpeed = 20.0f;
    [SerializeField] private float waterDeceleration;
    [SerializeField] private float waterHoverSpeed;

    [SerializeField] private bool hovering;

    [Header("Assignables")]
    [SerializeField] private FixedJoystick joystick;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
    }

    ////////////////////////////////////////////////////////
    /// Input
    /// 

    public void onSwimInputDown()
    {
        hovering = false;
        swimming = true;
    }

    public void onSwimInputUp()
    {
        swimming = false;
    }

    ////////////////////////////////////////////////////////
    /// Movement
    
    private void HandleMovement()
    {
        if (transform.position.y <= LevelManager.groundCeilingHeight)
        {
            if (swimming) Swim();
            else HandleDeceleration();
        }
        else
            rb.velocity += Physics2D.gravity * Time.deltaTime;
    }

    private void Swim()
    {
        float speedToAdd = (swimSpeed * joystick.Vertical) * Time.deltaTime;
        if (Mathf.Abs(rb.velocity.y + speedToAdd) <= maxSwimSpeed)
            rb.velocity = new Vector2(0.0f, rb.velocity.y + speedToAdd);
        else
            rb.velocity = new Vector2(0.0f, maxSwimSpeed * (rb.velocity.y < 0.0f ? -1.0f : 1.0f));
    }

    private void HandleDeceleration()
    {
        if (rb.velocity.y < -waterHoverSpeed)
            hovering = true;

        if (hovering)
        {
            rb.velocity = Vector2.Lerp(
                rb.velocity,
                new Vector2(0.0f, -waterHoverSpeed),
                waterDeceleration * Time.deltaTime);
        }
        else
            rb.velocity += Physics2D.gravity * Time.deltaTime;
    }
}
