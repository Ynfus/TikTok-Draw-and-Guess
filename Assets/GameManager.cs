using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Definicja mo¿liwych stanów gry
    public enum GameState
    {
        ChoosingWord,
        Drawing,
        //Guessing,
        Guessed,
        Failed,
        Results
    }

    // Obecny stan gry
    private GameState currentGameState;

    // Event wywo³ywany przy zmianie stanu gry
    public delegate void OnGameStateChange(GameState newState);
    public static event OnGameStateChange onGameStateChange;

    void Start()
    {
        // Ustawienie pocz¹tkowego stanu gry
        currentGameState = GameState.ChoosingWord;
        if (onGameStateChange != null)
        {
            onGameStateChange(currentGameState);
        }
    }

    void Update()
    {
        // Kod obs³uguj¹cy zmianê stanu gry (np. w zale¿noœci od inputu gracza)
        // ...

        // Wywo³anie eventu przy zmianie stanu gry
        //if (onGameStateChange != null && currentGameState != newGameState)
        //{
        //    currentGameState = newGameState;
        //    onGameStateChange(currentGameState);
        //}
    }
}
