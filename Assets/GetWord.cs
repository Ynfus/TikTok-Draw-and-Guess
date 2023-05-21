using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data;
using Mono.Data.Sqlite;
using UnityEngine.Scripting;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

public class GetWord : MonoBehaviour
{
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;
    [SerializeField] private GameObject button4;
    [SerializeField] private GameObject refreshButton;

    [SerializeField] private TextMeshProUGUI button1Text;
    [SerializeField] private TextMeshProUGUI button2Text;
    [SerializeField] private TextMeshProUGUI button3Text; 
    [SerializeField] private TextMeshProUGUI selectedWordText;

    private GameManager gameManager;

    [SerializeField] private TMP_InputField userInputField;





    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += GetWord_OnStateChanged;
        SetRandomWords();
    }

    private void GetWord_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsChoosingWord())
        {
            SetRandomWords();
        }
    }

    private void SetRandomWords()
    {

        button4.SetActive(true);


        List<string> words = DbConnect.Instance.GetRandomWords();
        userInputField.gameObject.SetActive(true);
        refreshButton.SetActive(true);
        button1.SetActive(true);
        button1Text.gameObject.SetActive(true);
        button2.SetActive(true);
        button2Text.gameObject.SetActive(true);
        button3.SetActive(true);
        button3Text.gameObject.SetActive(true);
        button1Text.text = words[0];
        button2Text.text = words[1];
        button3Text.text = words[2];
        button1.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(button1Text.text));
        button2.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(button2Text.text));
        button3.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(button3Text.text));

    }

    public void SetListener()
    {

        string userText = userInputField.text;
        OnButtonClick(userText);

    }
    private void OnButtonClick(string buttonText)
    {
        Debug.Log("Button clicked: " + buttonText);
        selectedWordText.text = buttonText;
        userInputField.text = "";
        refreshButton.SetActive(false);
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        userInputField.gameObject.SetActive(false);
        TiktokController.Instance.SetSelectedWord(buttonText);
        GameManager.Instance.SetDrawingState();
    }

}
