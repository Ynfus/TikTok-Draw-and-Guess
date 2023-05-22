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
    public string id;
    public static TiktokController Instance { get; private set; }

    
    public TextMeshProUGUI textMeshProUGUI;
    
    [SerializeField] private InputField idInputField;
   
    [SerializeField] private Scrollbar scrollbar;
   
    [SerializeField] GameObject winFailInfo;

    [SerializeField] TextMeshProUGUI nicknameInfo;
    [SerializeField] TextMeshProUGUI wordToGuess;
    [SerializeField] TextMeshProUGUI stateInfo;
    [SerializeField] TextMeshProUGUI successFailText;

    [SerializeField] Image circleImage;
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
    
    //private List<string> chatMessages = new List<string>();




    public void SetSelectedWord(string word)
    {
        timeElapsed = 0;
        drawingStartTime = Time.timeSinceLevelLoad;
        PlayerPrefs.SetString("SelectedWord", word);
        _selectedWord = word;
        maxTime = PlayerPrefs.GetInt("RoundTime", 60);
        winFailInfo.SetActive(false);
    }
    private void Awake()
    {
        Instance = this;
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
    void Client_OnCommentRecieved(object sender, WebcastChatMessage e)
    {

        Debug.Log("OnCommentRecived");
        _comments.Enqueue(e.User.Nickname + ": " + e.Comment);

        if (GameManager.Instance.IsDrawing()&&isLooking)
        {
            LookForWord(e, GameManager.Instance);
        }
    }
    //public void AddMessage(string message)
    //{
    //    chatMessages.Add(message);

    //    if (chatMessages.Count > maxMessages)
    //    {
    //        chatMessages.RemoveAt(0);
    //    }

    //    UpdateChatText();
    //}

    //private void UpdateChatText()
    //{
    //    textMeshProUGUI.text = string.Join("\n", chatMessages);
    //}
    public TextMeshProUGUI textMeshPro;
    private StringBuilder chatText = new StringBuilder();

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
        textMeshPro.text = chatText.ToString();
    }
    void Update()
    {

        Debug.Log("Update");
        while (_comments.Count > 0)
        {
            var comment = _comments.Dequeue();
            AddMessage(comment);
            //textMeshProUGUI.text += "\n" + comment;
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
    void ScrollToBottom()
    {
        //Canvas.ForceUpdateCanvases();
        scrollbar.value = 0f;
    }


    private void LookForWord(WebcastChatMessage e, GameManager gameManager)
    {
        if (!string.IsNullOrEmpty(_selectedWord) && timeElapsed < maxTime)
        {
            string selectedWord = _selectedWord.ToLower();
            string comment = e.Comment.ToLower();
            if(comment.Contains("an")&& isLooking)
            {
                isLooking = false;
                float percentage = Mathf.Clamp01(timeElapsed / maxTime);
                int points = Mathf.RoundToInt((1 - percentage) * maxPoints);
                Debug.Log("Zdoby³eœ(aœ) " + points + " punktów za odgadniêcie s³owa.");


                DbConnect.Instance.UpsertRanking(e.User.Nickname, points);
                stateInfo.text = "Success!";
                wordToGuess.text = _selectedWord.ToUpper();
                nicknameInfo.text = e.User.Nickname;
                //_selectedWord = "";
                //successFailText.text = "Sukces";
                isLooking = false;
                //Debug.Log($"asdasd {GameManager.Instance.IsDrawing()}");
                //OnGuessed?.Invoke(this, e);



            }
        }

    }

    //public void OnGuessed1(object sender, WebcastChatMessage e)
    //{
    //    float percentage = Mathf.Clamp01(timeElapsed / maxTime);
    //    int points = Mathf.RoundToInt((1 - percentage) * maxPoints);
    //    Debug.Log("Zdoby³eœ(aœ) " + points + " punktów za odgadniêcie s³owa.");

    //    DbConnect.Instance.UpsertRanking(e.User.Nickname, points);
    //    _selectedWord = "";
    //    PlayerPrefs.SetString("SelectedWord", "");
    //    successFailText.text = "Sukces";
    //    GameManager.Instance.SetSuccessState();
    //}
    public void SetLooking()
    {
        isLooking=true;
    }
























    //public string id;
    //[SerializeField] TextMeshProUGUI textMeshProUGUI;
    //private TikTokLiveClient _client;
    //private Queue<string> _comments;

    //void Start()
    //{
    //    _client = new TikTokLiveClient(id);
    //    _comments = new Queue<string>();

    //    _client.OnCommentRecieved += Client_OnCommentRecieved;
    //    try
    //    {
    //        _client.Start();
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogException(e);
    //    }
    //    PlayerPrefs.SetString("SelectedWord", "Czeœæ");
    //}

    //void OnDestroy()
    //{
    //    _client.Stop();
    //    _client.OnCommentRecieved -= Client_OnCommentRecieved;
    //}

    //void Client_OnCommentRecieved(object sender, WebcastChatMessage e)
    //{
    //    Debug.Log(e.User.Nickname + ": " + e.Comment);
    //    lock (_comments)
    //    {
    //        _comments.Enqueue(e.User.Nickname + ": " + e.Comment);
    //    }
    //    if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SelectedWord")))
    //    {
    //        string selectedWord = PlayerPrefs.GetString("SelectedWord").ToLower();
    //        string comment = e.Comment.ToLower();
    //        if (selectedWord == comment)
    //        {
    //            string nickname = e.User.Nickname;
    //            Debug.Log(nickname + " znalaz³(a) s³owo: " + selectedWord);
    //            PlayerPrefs.SetString("SelectedWord", "");
    //        }
    //    }
    //}

    //void Update()
    //{
    //    Debug.Log($"Iloœæ komentarzy w kolejce: {_comments.Count}");
    //    lock (_comments)
    //    {
    //        while (_comments.Count > 0)
    //        {
    //            var comment = _comments.Dequeue();
    //            textMeshProUGUI.text += "\n" + comment;
    //        }
    //    }
    //}





    //public string id;

    //[SerializeField] TextMeshProUGUI textMeshProUGUI;

    //private TikTokLiveClient _client;

    //private Queue<string> _profilePictures;
    //void Start()
    //{
    //    _client = new TikTokLiveClient(id);
    //    _profilePictures = new Queue<string>();

    //    _client.OnCommentRecieved += Client_OnCommentRecieved;
    //    try
    //    {

    //        _client.Start();
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogException(e);
    //    }
    //}
    //void OnDestroy()
    //{
    //    _client.Stop();
    //    _client.OnCommentRecieved -= Client_OnCommentRecieved;
    //}

    //void Client_OnCommentRecieved(object sender, WebcastChatMessage e)
    //{
    //    Debug.Log(e.Comment);
    //    lock (_profilePictures)
    //    {
    //        var url = e.User.profilePicture.Urls.FirstOrDefault(x => x.Contains(".jpeg"));
    //        if (url != null)
    //        {
    //            _profilePictures.Enqueue(url);
    //            _profilePictures.Enqueue(e.Comment);
    //        }
    //    }
    //}

    //void Update()
    //{
    //    lock (_profilePictures)
    //    {
    //        if (_profilePictures.Count > 0)
    //        {
    //            var url = _profilePictures.Dequeue();
    //            var comment = _profilePictures.Dequeue();
    //            StartCoroutine(AddNewCube(url, comment));
    //        }
    //    }

    //}
    //IEnumerator AddNewCube(string url, string comment)
    //{
    //    var request = UnityWebRequestTexture.GetTexture(url);
    //    yield return request.SendWebRequest();
    //    var tex = DownloadHandlerTexture.GetContent(request);

    //    var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    obj.transform.position = Random.insideUnitSphere * 5;
    //    obj.GetComponent<MeshRenderer>().material.mainTexture = tex;

    //    //var textObj = new GameObject("Text");
    //    //textObj.transform.parent = obj.transform;
    //    //textObj.transform.localPosition = new Vector3(0, 0.5f, 0);
    //    //var textMesh = textObj.AddComponent<TextMesh>();
    //    textMeshProUGUI.text += "\n" + comment;
    //}
}