using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Color invColor;
    [SerializeField] private Color onDamageColor;

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

        visual.transform.eulerAngles = new Vector3(0f, 0f, rb.velocity.y * swimStrength);
    }

    public void ApplyDamageColors()
    {
        visual.color = new Color(Color.red.r, Color.red.g, Color.red.b, visual.color.a);
    }

    public void ToggleInvincibilityAnim(bool state)
    {
        if (state) visual.color = new Color(visual.color.r, visual.color.g, visual.color.b, invColor.a);
        else visual.color = new Color(visual.color.r, visual.color.g, visual.color.b, Color.white.a);
    }
}
