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
    [SerializeField] private PlayerStats p_stats;

    [Header("HUDs")]
    [SerializeField] private GameObject HUD_ingame;
    [SerializeField] private GameObject HUD_pause;
    [SerializeField] private GameObject HUD_menu;

    [Header("UI Elements")]
    [SerializeField] private Button btn_playAgain;
    [SerializeField] private Text lb_coins;
    [SerializeField] private Text lb_dist;
    [SerializeField] private Text lb_version;
    [SerializeField] private Text lb_finalStats;

    [Header("Misc")]
    [SerializeField] private int menuBubblePrice;

    // Que sintaxe grande meu deus do ceu
    public static GAMESTATE currentState { get; private set; } = GAMESTATE.WAITING;

    public static GameManager instance { get; private set; }

    //
    // MOVER TUDO A VER COM UI PARA OUTRA CLASSE
    // por favor
    //
    // Update 1 mes depois:
    // To com preguica 
    //

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;

        lb_version.text = "Don's Journey Prototype v" + Application.version;
    }

    /*---------------------------------------------------------------*/
    /*---------------------------------------------------------------*/

    #region MANAGEMENT

    private void Start()
    {
        currentState = GAMESTATE.WAITING;
        HUD_ingame.SetActive(false);
        HUD_pause.SetActive(false);
        btn_playAgain.gameObject.SetActive(false);
        HUD_menu.SetActive(true);
        Time.timeScale = 1.0f;

        p_movement.enabled = false;
    }

    public void StartGame()
    {
        currentState = GAMESTATE.PLAYING;
        HUD_menu.SetActive(false);
        HUD_ingame.SetActive(true);

        p_movement.enabled = true;

        Camera.main.GetComponent<CameraFollow>().PrepareIngameCam();
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
        btn_playAgain.gameObject.SetActive(true);
        StartCoroutine(UnlockReplayBtnAfterTime(1.0f));
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    /*---------------------------------------------------------------*/
    /*---------------------------------------------------------------*/

    #region UI STUFF

    public void UpdateCoins(int newValue)
    {
        lb_coins.text = newValue.ToString();
    }

    public void UpdateDistance(float newValue)
    {
        lb_dist.text = ((int)newValue).ToString() + "m";
    }

    private IEnumerator UnlockReplayBtnAfterTime(float _t)
    {
        yield return new WaitForSeconds(_t);
        btn_playAgain.interactable = true;
    }

    #endregion

    /*---------------------------------------------------------------*/
    /*---------------------------------------------------------------*/

    #region MISC

    public void ResetSessionData()
    {
        PlayerStats.SetCoins(0);
        UpdateCoins(0);
    }

    public void BuyMenuBubble()
    {
        if (!PlayerStats.SubtractCoins(menuBubblePrice))
            return;

        UpdateCoins(PlayerStats.coinsCollected);
        p_stats.SetBubbled(true);
    }

    #endregion

    /*---------------------------------------------------------------*/
    /*---------------------------------------------------------------*/
}
