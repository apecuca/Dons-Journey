using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private bool immortal = false;

    [SerializeField] private float invincibilityDuration;
    private bool invincible = false;

    public int health { get; private set; } = 2;
    public static int coinsCollected { get; private set; }

    private PlayerAnim p_anim;

    private void Start()
    {
        p_anim = GetComponent<PlayerAnim>();

        // Starting stats
        health = 2;
        coinsCollected = 0;
    }

    private void OnEnemyCollision(GameObject enemy)
    {
        if (invincible)
            return;

        if (!immortal)
        {
            health--;
            p_anim.ApplyDamageColors();
        }

        Camera.main.GetComponent<CameraFollow>().ShakeScreen();

        if (health <= 0) GameManager.instance.EndGame();
        else StartCoroutine(BecomeInvincible(invincibilityDuration));

        //Destroy(enemy);
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
        coinsCollected++;

        GameManager.instance.UpdateCoins(coinsCollected);

        Destroy(coin);
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

            default: break;
        }
    }
}
