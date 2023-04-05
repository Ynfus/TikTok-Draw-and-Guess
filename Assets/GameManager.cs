using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera main)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 mousePosition, Camera main)
    {
        Vector3 worldPosition = main.ScreenToWorldPoint(mousePosition);
        return worldPosition;
    }
    public static Vector3 GetDirToMouse(Vector3 fromPosition)
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        return (mouseWorldPosition - fromPosition).normalized;
    
    }
    public static Color GetColorFromString(string color)
    {
        float red = Hex_to_Dec01(color.Substring(0, 2));
        float green = Hex_to_Dec01(color.Substring(2, 2));
        float blue = Hex_to_Dec01(color.Substring(4, 2));
        float alpha = 1f;
        if (color.Length >= 8)
        {
            // Color string contains alpha
            alpha = Hex_to_Dec01(color.Substring(6, 2));
        }
        return new Color(red, green, blue, alpha);
    }
    public static float Hex_to_Dec01(string hex)
    {
        return Hex_to_Dec(hex) / 255f;
    }
    public static int Hex_to_Dec(string hex)
    {
        return Convert.ToInt32(hex, 16);
    }
    public static bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            PointerEventData pe = new PointerEventData(EventSystem.current);
            pe.position = Input.mousePosition;
            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pe, hits);
            return hits.Count > 0;
        }
    }
}
