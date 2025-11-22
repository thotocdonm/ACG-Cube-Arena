using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public GameState CurrentGameState { get; private set; }

    public static Action<GameState> onGameStateChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentGameState = GameState.Game;
    }

    public void ChangeGameState(GameState newGameState)
    {
        if (CurrentGameState == newGameState) return;
        CurrentGameState = newGameState;
        onGameStateChanged?.Invoke(CurrentGameState);
    }
}
