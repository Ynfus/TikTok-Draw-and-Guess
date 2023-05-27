using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        EnterID,
        ChoosingWord,
        Drawing,
        Failed,
        Success,
    }
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject changeStateChoosingWord;
    [SerializeField] GameObject cancelRoundBtn;


    [SerializeField] TextMeshProUGUI selectedWordText;

    private GameState currentGameState;

    public event EventHandler OnStateChanged;


    private void Awake()
    {
        Instance = this;
        currentGameState = GameState.EnterID;
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
    }
    public bool IsChoosingWord()
    {
        return currentGameState == GameState.ChoosingWord;

    }
    public bool IsSucced()
    {
        return currentGameState == GameState.Success;

    }
    public bool IsDrawing()
    {
        return currentGameState == GameState.Drawing;

    }
    public void SetDrawingState()
    {
        currentGameState = GameState.Drawing;
        TiktokController.Instance.SetLooking();
        cancelRoundBtn.SetActive(true);
    }
    public void SetFailState()
    {
        DrawMesh.Instance.ClearCanva();
        currentGameState = GameState.Failed;
        changeStateChoosingWord.SetActive(true);
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        selectedWordText.text = "";
        cancelRoundBtn.SetActive(false);
    }
    public void SetSuccessState()
    {

        Debug.Log("asdasd1" + currentGameState);
        changeStateChoosingWord.SetActive(true);
        currentGameState = GameState.Success;
        Debug.Log($"asdasd2 {currentGameState}");
        PlayerPrefs.SetString("SelectedWord", "");
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        DrawMesh.Instance.ClearCanva();
        selectedWordText.text = "";
        cancelRoundBtn.SetActive(false);
    }
    public void SetChooseWordState()
    {
        currentGameState = GameState.ChoosingWord;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        changeStateChoosingWord.SetActive(false);
    }
    public void SetChooseWordStateByCancelBtn()
    {
        currentGameState = GameState.ChoosingWord;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        changeStateChoosingWord.SetActive(false);
        cancelRoundBtn.SetActive(false);
        DrawMesh.Instance.ClearCanva();
    }
}
