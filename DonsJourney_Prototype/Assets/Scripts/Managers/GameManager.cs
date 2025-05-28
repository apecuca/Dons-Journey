using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAMESTATE
{
    IDLE,
    PLAYING,
    FINISHED
}

public class GameManager : MonoBehaviour
{
    // Que sintaxe grande meu deus do ceu
    public static GAMESTATE currentState { get; private set; } = GAMESTATE.IDLE;

    private void Start()
    {
        currentState = GAMESTATE.PLAYING;
    }
}
