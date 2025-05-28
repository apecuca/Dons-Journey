using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool swimming = false;
    [SerializeField] private float maxYSpeed = 12.0f;
    [SerializeField] private float swimYSpeed = 0.4f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (swimming &&
            transform.position.y <= LevelManager.groundCeilingHeight)
            Swim();
    }

    ////////////////////////////////////////////////////////
    /// Input

    public void onSwimInputDown()
    {
        swimming = true;
    }

    public void onSwimInputUp()
    {
        swimming = false;
    }

    ////////////////////////////////////////////////////////
    /// Movement
    
    private void Swim()
    {
        float speedToAdd = swimYSpeed + Time.deltaTime;
        if ((rb.velocity.y + speedToAdd) <= maxYSpeed)
            rb.velocity = new Vector2(0.0f, rb.velocity.y + speedToAdd);
        else
            rb.velocity = new Vector2(0.0f, maxYSpeed);
    }
}
