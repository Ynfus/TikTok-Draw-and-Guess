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

public class TiktokController : MonoBehaviour
{
    public string id;
    public TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Scrollbar scrollbar;
    private TikTokLiveClient _client;
    private Queue<string> _comments = new Queue<string>();

    private string _selectedWord = "";

    void Start()
    {
        _client = new TikTokLiveClient(id);

        _client.OnCommentRecieved += Client_OnCommentRecieved;
        try
        {
            _client.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        PlayerPrefs.SetString("SelectedWord", "hej");
        _selectedWord = PlayerPrefs.GetString("SelectedWord", "");
    }

    void OnDestroy()
    {
        _client.Stop();
        _client.OnCommentRecieved -= Client_OnCommentRecieved;
    }

    void Client_OnCommentRecieved(object sender, WebcastChatMessage e)
    {
        lock (_comments)
        {
            _comments.Enqueue(e.User.Nickname + ": " + e.Comment);
        }
        if (GameManager.Instance.IsDrawing())
        {
            LookForWord(e);
        }
    }

    void Update()
    {
        lock (_comments)
        {
            while (_comments.Count > 0)
            {
                var comment = _comments.Dequeue();
                textMeshProUGUI.text += "\n" + comment;
                ScrollToBottom();
            }
        }
    }
    void ScrollToBottom()
    {
        scrollbar.value = 0f;
    }


    private void LookForWord(WebcastChatMessage e)
    {
        if (!string.IsNullOrEmpty(_selectedWord))
        {
            string selectedWord = _selectedWord.ToLower();
            string comment = e.Comment.ToLower();
            if (selectedWord == comment)
            {
                string nickname = e.User.Nickname;
                Debug.Log(nickname + " znalaz³(a) s³owo: " + selectedWord);

                PlayerPrefs.SetString("SelectedWord", "");

                _selectedWord = "";
            }
        }

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