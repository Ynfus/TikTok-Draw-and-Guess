using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawMeshUI : MonoBehaviour
{
    private void Awake()
    {
        //Button color4Btn = GameObject.Find("Color4Btn").GetComponent<Button>();
        //color4Btn.onClick.AddListener(() => { SetColor(GameManager.GetColorFromString("0077FF")); });


        GameObject.Find("Color1Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GameManager.GetColorFromString("000000")); });
        GameObject.Find("Color2Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GameManager.GetColorFromString("FFFFFF")); });
        GameObject.Find("Color3Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GameManager.GetColorFromString("22FF00")); });
        GameObject.Find("Color4Btn").GetComponent<Button>().onClick.AddListener(() => { SetColor(GameManager.GetColorFromString("0077FF")); });

        GameObject.Find("Thickness1Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(2f); });
        GameObject.Find("Thickness2Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(6f); });
        GameObject.Find("Thickness3Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(10f); });
        GameObject.Find("Thickness4Btn").GetComponent<Button>().onClick.AddListener(() => { SetThickness(20f); });
    }

    private void SetThickness(float thickness)
    {
        DrawMesh.Instance.SetThickness(thickness);
    }

    private void SetColor(Color color)
    {
        DrawMesh.Instance.SetColor(color);
    }
}
