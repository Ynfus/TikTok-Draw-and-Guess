using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Definicja mo�liwych stan�w gry
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

    // Event wywo�ywany przy zmianie stanu gry
    public delegate void OnGameStateChange(GameState newState);
    public static event OnGameStateChange onGameStateChange;

    void Start()
    {
        // Ustawienie pocz�tkowego stanu gry
        currentGameState = GameState.ChoosingWord;
        if (onGameStateChange != null)
        {
            onGameStateChange(currentGameState);
        }
    }

    void Update()
    {
        // Kod obs�uguj�cy zmian� stanu gry (np. w zale�no�ci od inputu gracza)
        // ...

        // Wywo�anie eventu przy zmianie stanu gry
        //if (onGameStateChange != null && currentGameState != newGameState)
        //{
        //    currentGameState = newGameState;
        //    onGameStateChange(currentGameState);
        //}
    }
}
