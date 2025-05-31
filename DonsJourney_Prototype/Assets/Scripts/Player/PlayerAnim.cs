using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField] private Transform visual;

    [Header("Animation settings")]
    [SerializeField] private float swimStrength;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.currentState != GAMESTATE.PLAYING)
            return;

        visual.eulerAngles = new Vector3(0f, 0f, rb.velocity.y * swimStrength);
    }
}
