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

    [SerializeField] private TextMeshProUGUI button1Text;
    [SerializeField] private TextMeshProUGUI button2Text;
    [SerializeField] private TextMeshProUGUI button3Text;

    private GameManager gameManager;


    
    private string DataBaseName = "DrawAndGuess";


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
        else
        {

        }
    }

    private void SetRandomWords()
    {
        List<string> words = GetRandomWords();
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
    public List<string> GetRandomWords()
    {
        string conn = "URI=file:Assets/Imports/StreamingAssets/DrawAndGuess.db";

        IDbCommand dbcmd;
        IDbConnection dbcon;
        IDataReader reader;

        dbcon = new SqliteConnection(conn);
        dbcon.Open();
        dbcmd = dbcon.CreateCommand();
        string SqlQuery = "SELECT * FROM Words ORDER BY RANDOM() LIMIT 3; ";
        dbcmd.CommandText = SqlQuery;
        reader = dbcmd.ExecuteReader();
        List<string> words = new List<string>();
        while (reader.Read())
        {
            Debug.Log(reader.GetInt32(0));
            words.Add(reader.GetString(1));
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;

        return words;



    }
    private void OnButtonClick(string buttonText)
    {
        Debug.Log("Button clicked: " + buttonText);
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        TiktokController.Instance.SetSelectedWord(buttonText);
        GameManager.Instance.SetDrawingState();
    }

}
