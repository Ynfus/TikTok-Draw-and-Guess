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
    [SerializeField] private GameObject word1;
    [SerializeField] private GameObject word2;
    [SerializeField] private GameObject word3;
    [SerializeField] private GameObject inputFieldWord;
    [SerializeField] private GameObject refreshButton;

    [SerializeField] private TextMeshProUGUI word1Text;
    [SerializeField] private TextMeshProUGUI word2Text;
    [SerializeField] private TextMeshProUGUI word3Text; 
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
        List<string> words = DbConnect.Instance.GetRandomWords();
        refreshButton.SetActive(true);
        word1.SetActive(true);
        word2.SetActive(true);
        word3.SetActive(true);
        inputFieldWord.SetActive(true);
        userInputField.gameObject.SetActive(true);
        word1Text.gameObject.SetActive(true);
        word2Text.gameObject.SetActive(true);
        word3Text.gameObject.SetActive(true);
        word1Text.text = words[0];
        word2Text.text = words[1];
        word3Text.text = words[2];
        word1.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(word1Text.text));
        word2.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(word2Text.text));
        word3.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(word3Text.text));

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
        word1.SetActive(false);
        word2.SetActive(false);
        word3.SetActive(false);
        inputFieldWord.SetActive(false);
        userInputField.gameObject.SetActive(false);
        TiktokController.Instance.SetSelectedWord(buttonText);
        GameManager.Instance.SetDrawingState();
    }

}
