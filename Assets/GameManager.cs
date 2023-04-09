using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        ChoosingWord,
        Drawing,
        //Guessing,
        Guessed,
        Failed,
        Results
    }
    public static GameManager Instance { get; private set; }

    private GameState currentGameState;
    public event EventHandler OnStateChanged;
    private void Awake()
    {
        Instance = this;
        currentGameState= GameState.ChoosingWord;
    }

    void Update()
    {

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

        LayerMask ignoreLayer = LayerMask.NameToLayer("Canva");
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = Input.mousePosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);

        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject.layer == ignoreLayer)
            {
                return false;
            }
        }

        return true;





        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return true;
        //}
        //else
        //{
        //    PointerEventData pe = new PointerEventData(EventSystem.current);
        //    pe.position = Input.mousePosition;
        //    List<RaycastResult> hits = new List<RaycastResult>();
        //    EventSystem.current.RaycastAll(pe, hits);
        //    return hits.Count > 0;
        //}
    }
    public bool IsChoosingWord()
    {
        return currentGameState == GameState.ChoosingWord;

    }
    public bool IsDrawing()
    {
        return currentGameState == GameState.Drawing;

    }
}
