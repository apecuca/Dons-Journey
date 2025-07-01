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
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask groundLayer;
    private bool onAir = false;

    private Rigidbody2D rb;
    private PlayerAnim p_anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        p_anim = GetComponent<PlayerAnim>();
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

            if (onAir)
            {
                p_anim.EmitParticles_WaterSplash(transform.position);
                onAir = false;
            }
        }
        else
        {
            if (!onAir)
            {
                p_anim.EmitParticles_WaterSplash(transform.position);
                onAir = true;
            }

            rb.velocity += Physics2D.gravity * Time.deltaTime;
        }
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

    public bool IsGrounded()
    {
        return Physics2D.Raycast(feet.position, Vector2.down, 0.1f, groundLayer);
    }
}
