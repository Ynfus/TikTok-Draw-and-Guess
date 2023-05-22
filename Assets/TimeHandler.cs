using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    [SerializeField] GameObject button30;
    [SerializeField] GameObject button60;
    [SerializeField] GameObject button90;
    [SerializeField] GameObject button120;

    private void Start()
    {
        button30.GetComponent<Button>().onClick.AddListener(() => SetTime(30));
        button60.GetComponent<Button>().onClick.AddListener(() => SetTime(60));
        button90.GetComponent<Button>().onClick.AddListener(() => SetTime(90));
        button120.GetComponent<Button>().onClick.AddListener(() => SetTime(120));
    }

    private void SetTime(int time)
    {
        PlayerPrefs.SetInt("RoundTime", time);
        PlayerPrefs.Save();
    }
}
