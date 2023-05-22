using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawMeshUI : MonoBehaviour
{
    private Button _lastColorButton;

    private void Awake()
    {
        GameObject.Find("ColorBlackButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorBlackButton").GetComponent<Button>()); });
        GameObject.Find("ColorWhiteButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorWhiteButton").GetComponent<Button>()); });
        GameObject.Find("ColorGreenButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorGreenButton").GetComponent<Button>()); });
        GameObject.Find("ColorBlueButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorBlueButton").GetComponent<Button>()); });
        GameObject.Find("ColorYellowButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorYellowButton").GetComponent<Button>()); });
        GameObject.Find("ColorRedButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorRedButton").GetComponent<Button>()); });
        GameObject.Find("ColorGrayButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorGrayButton").GetComponent<Button>()); });
        GameObject.Find("ColorPurpleButton").GetComponent<Button>().onClick.AddListener(() => { OnColorButtonClicked(GameObject.Find("ColorPurpleButton").GetComponent<Button>()); });

        GameObject.Find("Thickness1Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(5f); });
        GameObject.Find("Thickness2Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(10f); });
        GameObject.Find("Thickness3Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(20f); });
        GameObject.Find("Thickness4Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(30f); });

        GameObject.Find("BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => { DrawMesh.Instance.UndoLastMeshObject(); });
        GameObject.Find("DeleteCanva").GetComponent<Button>().onClick.AddListener(() => { DrawMesh.Instance.ClearCanva(); });
    }

    private void OnColorButtonClicked(Button clickedButton)
    {
        Color color = Color.black;
        switch (clickedButton.name)
        {
            case "ColorBlackButton":
                color = GameManager.GetColorFromString("000000");
                break;
            case "ColorWhiteButton":
                color = GameManager.GetColorFromString("FFFFFF");
                break;
            case "ColorGreenButton":
                color = GameManager.GetColorFromString("22FF00");
                break;
            case "ColorBlueButton":
                color = GameManager.GetColorFromString("0077FF");
                break;
            case "ColorYellowButton":
                color = GameManager.GetColorFromString("FFFF00");
                break;
            case "ColorRedButton":
                color = GameManager.GetColorFromString("FF0000");
                break;
            case "ColorGrayButton":
                color = GameManager.GetColorFromString("808080");
                break;
            case "ColorPurpleButton":
                color = GameManager.GetColorFromString("800080");
                break;
        }

        DrawMesh.Instance.SetColor(color);

        if (_lastColorButton != null)
        {
            _lastColorButton.transform.localScale = Vector3.one;
        }

        clickedButton.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        _lastColorButton = clickedButton;
    }

    private void SetThickness(float thickness)
    {
        DrawMesh.Instance.SetThickness(thickness);
    }



}
