using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    [SerializeField] Image clockImage;

    public static ClockUI Instance { get; private set; }

    private void Awake()
    {
        Instance= this;
    }
    public void FillClock(float amount)
    { 
        clockImage.fillAmount= amount;
    }
}
