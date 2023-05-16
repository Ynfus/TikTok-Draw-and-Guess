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

public class TiktokController : MonoBehaviour
{
    public string id;
    public static TiktokController Instance { get; private set; }

    public TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private InputField idInputField;
    [SerializeField] TextMeshProUGUI successFailText;
    [SerializeField] private Scrollbar scrollbar;
    public event EventHandler<WebcastChatMessage> OnGuessed;
    private TikTokLiveClient _client;
    private Queue<string> _comments = new Queue<string>();
    private float drawingStartTime;
    private float maxTime = 200f;
    private float maxPoints = 100;
    private string _selectedWord;
    public float timeElapsed;
    private bool isLooking = true;

    public void SetSelectedWord(string word)
    {
        timeElapsed = 0;
        drawingStartTime = Time.timeSinceLevelLoad;
        PlayerPrefs.SetString("SelectedWord", word);
        _selectedWord = word;
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

    void Update()
    {

        Debug.Log("Update");
        while (_comments.Count > 0)
        {
            var comment = _comments.Dequeue();
            textMeshProUGUI.text += "\n" + comment;
            ScrollToBottom();
        }

        if (GameManager.Instance.IsDrawing())
        {
            timeElapsed = Time.timeSinceLevelLoad - drawingStartTime;
            if (timeElapsed > maxTime)
            {
                successFailText.text = "Fail";
                DbConnect.Instance.UpsertRanking("1651", 11);
                GameManager.Instance.SetFailState();
            }
            if (!isLooking)
s            {
                GameManager.Instance.SetSuccessState();
                isLooking= true;
            }
        }


    }
    void ScrollToBottom()
    {
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
                Debug.Log("Zdoby�e�(a�) " + points + " punkt�w za odgadni�cie s�owa.");


                DbConnect.Instance.UpsertRanking(e.User.Nickname, points);
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
    //    Debug.Log("Zdoby�e�(a�) " + points + " punkt�w za odgadni�cie s�owa.");

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
    //    PlayerPrefs.SetString("SelectedWord", "Cze��");
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
    //            Debug.Log(nickname + " znalaz�(a) s�owo: " + selectedWord);
    //            PlayerPrefs.SetString("SelectedWord", "");
    //        }
    //    }
    //}

    //void Update()
    //{
    //    Debug.Log($"Ilo�� komentarzy w kolejce: {_comments.Count}");
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