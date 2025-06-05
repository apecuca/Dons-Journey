using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health { get; private set; } = 2;
    public int coinsCollected { get; private set; }

    private void Start()
    {
        health = 2;
    }

    private void OnEnemyCollision(GameObject enemy)
    {
        health--;
        if (health <= 0)
            GameManager.instance.EndGame();

        Destroy(enemy);
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
