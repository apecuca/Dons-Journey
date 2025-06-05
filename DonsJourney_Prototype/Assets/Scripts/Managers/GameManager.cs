using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GAMESTATE
{
    IDLE,
    PLAYING,
    FINISHED
}

public class GameManager : MonoBehaviour
{
    [Header("Player assignables")]
    [SerializeField] private PlayerMovement p_movement;

    [Header("UI Assignables")]
    [SerializeField] private GameObject HUD_ingame;
    [SerializeField] private GameObject btn_startGame;
    [SerializeField] private GameObject btn_playAgain;
    [SerializeField] private Text lb_coins;

    // Que sintaxe grande meu deus do ceu
    public static GAMESTATE currentState { get; private set; } = GAMESTATE.IDLE;

    public static GameManager instance { get; private set; }

    //
    // MOVER TUDO A VER COM UI PARA OUTRA CLASSE
    // por favor
    //

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        currentState = GAMESTATE.IDLE;
        HUD_ingame.SetActive(false);
        btn_startGame.SetActive(true);
        btn_playAgain.SetActive(false);

        p_movement.enabled = false;
    }

    public void StartGame()
    {
        currentState = GAMESTATE.PLAYING;
        btn_startGame.SetActive(false);
        HUD_ingame.SetActive(true);

        p_movement.enabled = true;
    }

    public void EndGame()
    {
        currentState = GAMESTATE.FINISHED;
        btn_playAgain.SetActive(true);

        p_movement.enabled = false;
        p_movement.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateCoins(int newValue)
    {
        lb_coins.text = newValue.ToString();
    }
}
