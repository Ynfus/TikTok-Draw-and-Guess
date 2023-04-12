using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject startView;
    [SerializeField] GameObject mainView;

    public void OnSubmit()
    {
        string id = inputField.text;
        if (string.IsNullOrEmpty(id))
        {
            Debug.Log("ID cannot be empty!");
            return;
        }

        TiktokController.Instance.OnSubmitId(id);
        startView.SetActive(false);
        mainView.SetActive(true);
    }
}









