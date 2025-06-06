using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GAMESTATE
{
    WAITING,
    PAUSED,
    PLAYING,
    FINISHED
}

public class GameManager : MonoBehaviour
{
    [Header("Player assignables")]
    [SerializeField] private PlayerMovement p_movement;

    [Header("UI Assignables")]
    [SerializeField] private GameObject HUD_ingame;
    [SerializeField] private GameObject HUD_pause;
    [SerializeField] private GameObject btn_startGame;
    [SerializeField] private GameObject btn_playAgain;
    [SerializeField] private Text lb_coins;
    [SerializeField] private Text lb_dist;
    [SerializeField] private Text lb_version;
    [SerializeField] private Text lb_finalStats;

    // Que sintaxe grande meu deus do ceu
    public static GAMESTATE currentState { get; private set; } = GAMESTATE.WAITING;

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

        lb_version.text = "Don's Journey Prototype v" + Application.version;
    }

    private void Start()
    {
        currentState = GAMESTATE.WAITING;
        HUD_ingame.SetActive(false);
        HUD_pause.SetActive(false);
        btn_playAgain.SetActive(false);
        btn_startGame.SetActive(true);

        p_movement.enabled = false;
    }

    public void StartGame()
    {
        currentState = GAMESTATE.PLAYING;
        btn_startGame.SetActive(false);
        HUD_ingame.SetActive(true);

        p_movement.enabled = true;
    }

    public void TogglePause(bool _state)
    {
        Time.timeScale = _state ? 0.0f : 1.0f;
        currentState = _state ? GAMESTATE.PAUSED : GAMESTATE.PLAYING;
        HUD_pause.SetActive(_state);
    }

    public void EndGame()
    {
        currentState = GAMESTATE.FINISHED;

        p_movement.enabled = false;
        p_movement.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        lb_finalStats.text = "Distance reached: " + ((int)LevelManager.distance).ToString() + "m\n" +
                             "Jellyfish eaten: " + PlayerStats.coinsCollected.ToString();
        btn_playAgain.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateCoins(int newValue)
    {
        lb_coins.text = newValue.ToString();
    }

    public void UpdateDistance(float newValue)
    {
        lb_dist.text = ((int)newValue).ToString() + "m";
    }
}
