using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private bool immortal = false;

    [Header("Everything")]
    [SerializeField] private float invincibilityDuration;
    [SerializeField] private GameObject bubbleObj;
    private bool invincible = false;

    public int health { get; private set; } = 2;
    public bool bubbled { get; private set; }
    public static int coinsCollected { get; private set; }

    private PlayerAnim p_anim;

    public static PlayerStats instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    private void Start()
    {
        p_anim = GetComponent<PlayerAnim>();

        // Starting stats
        health = 2;
        GameManager.instance.UpdateCoins(coinsCollected);
        bubbled = false;
    }

    private void OnEnemyCollision(GameObject enemy)
    {
        if (invincible || immortal)
            return;

        if (!bubbled)
        {
            health--;
            p_anim.ApplyDamageColors();
        }
        else
            SetBubbled(false);

        Camera.main.GetComponent<CameraFollow>().ShakeScreen();

        if (health <= 0) GameManager.instance.EndGame();
        else StartCoroutine(BecomeInvincible(invincibilityDuration));
    }

    private IEnumerator BecomeInvincible(float duration)
    {
        invincible = true;
        p_anim.ToggleInvincibilityAnim(true);

        yield return new WaitForSeconds(duration);
        invincible = false;
        p_anim.ToggleInvincibilityAnim(false);
    }

    private void OnCoinCollision(GameObject coin)
    {
        SetCoins(coinsCollected + 1);
        p_anim.EmitParticles_Munching(coin.transform.position);

        GameManager.instance.UpdateCoins(coinsCollected);

        Destroy(coin);
    }

    public static bool SubtractCoins(int price)
    {
        if (coinsCollected < price)
            return false;

        coinsCollected -= price;
        return true;
    }

    public static void SetCoins(int value)
    {
        coinsCollected = value;
    }

    private void OnBubblePickupCollision(GameObject bubble)
    {
        Destroy(bubble);

        if (bubbled)
            return;

        SetBubbled(true);
    }

    public void SetBubbled(bool state)
    {
        bubbled = state;
        bubbleObj.SetActive(state);
        p_anim.EmitParticles_WaterSplash(transform.position);

        if (!state)
            LevelManager.instance.OnBubbleDestroyed();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                OnEnemyCollision(collision.gameObject);
                break;

            case "Coin":
                OnCoinCollision(collision.gameObject);
                break;

            case "BubblePickup":
                OnBubblePickupCollision(collision.gameObject);
                break;

            default: break;
        }
    }
}
