/// Basic profile picture handling example.
/// - @sebheron 2022

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UIElements;
using Microsoft.Unity.VisualStudio.Editor;
using Image = UnityEngine.UIElements.Image;
using System.Text;

public class TiktokController : MonoBehaviour
{
    public static TiktokController Instance { get; private set; }

    [SerializeField] private InputField idInputField;

    [SerializeField] private Scrollbar scrollbar;

    [SerializeField] GameObject winFailInfo;

    [SerializeField] TextMeshProUGUI nicknameInfo;
    [SerializeField] TextMeshProUGUI wordToGuess;
    [SerializeField] TextMeshProUGUI stateInfo;
    [SerializeField] TextMeshProUGUI successFailText;
    [SerializeField] TextMeshProUGUI chatTextUI;

    [SerializeField] Image circleImage;

    private StringBuilder chatText = new StringBuilder();

    public event EventHandler<WebcastChatMessage> OnGuessed;

    private TikTokLiveClient _client;

    private Queue<string> _comments = new Queue<string>();

    private float drawingStartTime;
    private float maxTime;
    private float maxPoints = 100;
    private float timeElapsed;

    private string _selectedWord;

    private bool isLooking = true;

    private int maxMessages = 2000;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        while (_comments.Count > 0)
        {
            var comment = _comments.Dequeue();
            AddMessage(comment);
            ScrollToBottom();
        }
        if (GameManager.Instance.IsDrawing())
        {
            timeElapsed = Time.timeSinceLevelLoad - drawingStartTime;
            if (timeElapsed > maxTime)
            {
                successFailText.text = "Fail";
                GameManager.Instance.SetFailState();
                winFailInfo.SetActive(true);
                stateInfo.text = "Fail!";
                wordToGuess.text = _selectedWord.ToUpper();
            }
            if (!isLooking)
            {
                GameManager.Instance.SetSuccessState();
                winFailInfo.SetActive(true);
                isLooking = true;
            }
        }
    }
    void OnDestroy()
    {
        if (_client != null)
        {
            _client.Stop();
            _client.OnCommentRecieved -= Client_OnCommentRecieved;
        }
    }
    public void OnSubmitId(string id)
    {
        if (_client != null)
        {
            _client.Stop();
            _client.OnCommentRecieved -= Client_OnCommentRecieved;
        }
        try
        {
            _client = new TikTokLiveClient(id);
            _client.OnCommentRecieved += Client_OnCommentRecieved;
            _client.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    public void SetSelectedWord(string word)
    {
        timeElapsed = 0;
        drawingStartTime = Time.timeSinceLevelLoad;
        PlayerPrefs.SetString("SelectedWord", word);
        _selectedWord = word;
        maxTime = PlayerPrefs.GetInt("RoundTime", 60);
        winFailInfo.SetActive(false);
    }
    void Client_OnCommentRecieved(object sender, WebcastChatMessage e)
    {
        Debug.Log("OnCommentRecived");
        _comments.Enqueue(e.User.Nickname + ": " + e.Comment);
        if (GameManager.Instance.IsDrawing() && isLooking)
        {
            LookForWord(e, GameManager.Instance);
        }
    }
    private void LookForWord(WebcastChatMessage e, GameManager gameManager)
    {
        if (!string.IsNullOrEmpty(_selectedWord) && timeElapsed < maxTime)
        {
            string selectedWord = _selectedWord.ToLower();
            string comment = e.Comment.ToLower();
            if (comment.Contains("an") && isLooking)
            {
                isLooking = false;
                float percentage = Mathf.Clamp01(timeElapsed / maxTime);
                int points = Mathf.RoundToInt((1 - percentage) * maxPoints);
                DbConnect.Instance.UpsertRanking(e.User.Nickname, points);
                stateInfo.text = "Success!";
                wordToGuess.text = _selectedWord.ToUpper();
                nicknameInfo.text = e.User.Nickname;
                isLooking = false;
            }
        }
    }
    public void AddMessage(string message)
    {
        chatText.AppendLine(message);
        if (chatText.Length > maxMessages)
        {
            int excessLength = chatText.Length - maxMessages;
            chatText.Remove(0, excessLength);
        }
        UpdateChatText();
    }
    private void UpdateChatText()
    {
        chatTextUI.text = chatText.ToString();
    }
    void ScrollToBottom()
    {
        scrollbar.value = 0f;
    }

    public void SetLooking()
    {
        isLooking = true;
    }
}