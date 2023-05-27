using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class Leadboard : MonoBehaviour
{
    public static Leadboard Instance { get; private set; }

    [SerializeField] TextMeshProUGUI firstPlace;
    [SerializeField] TextMeshProUGUI secondPlace;
    [SerializeField] TextMeshProUGUI thridPlace;
    [SerializeField] TextMeshProUGUI firstPlaceSession;
    [SerializeField] TextMeshProUGUI secondPlaceSession;
    [SerializeField] TextMeshProUGUI thridPlaceSession;

    [SerializeField] TextMeshProUGUI scorefirstPlace;
    [SerializeField] TextMeshProUGUI scoresecondPlace;
    [SerializeField] TextMeshProUGUI scorethridPlace;
    [SerializeField] TextMeshProUGUI scorefirstPlaceSession;
    [SerializeField] TextMeshProUGUI scoresecondPlaceSession;
    [SerializeField] TextMeshProUGUI scorethridPlaceSession;
    private GameManager gameManager;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += Leadboard_OnStateChanged;
        UpdateRanking();
    }

    private void Leadboard_OnStateChanged(object sender, System.EventArgs e)
    {
        if (!gameManager.IsSucced())
        {
            UpdateRanking();
        }
    }

    public void UpdateRanking()
    {
        DataTable globalRanking = DbConnect.Instance.GetGlobalRanking();

        if (globalRanking != null && globalRanking.Rows.Count > 0)
        {
            firstPlace.text = globalRanking.Rows[0]["name"].ToString();
            scorefirstPlace.text = globalRanking.Rows[0]["score"].ToString();
        }
        else
        {
            firstPlace.text = "No data";
            scorefirstPlace.text = "-";
        }

        if (globalRanking != null && globalRanking.Rows.Count > 1)
        {
            secondPlace.text = globalRanking.Rows[1]["name"].ToString();
            scoresecondPlace.text = globalRanking.Rows[1]["score"].ToString();
        }
        else
        {
            secondPlace.text = "No data";
            scoresecondPlace.text = "-";
        }

        if (globalRanking != null && globalRanking.Rows.Count > 2)
        {
            thridPlace.text = globalRanking.Rows[2]["name"].ToString();
            scorethridPlace.text =  globalRanking.Rows[2]["score"].ToString();
        }
        else
        {
            thridPlace.text = "No data";
            scorethridPlace.text = "-";
        }

        DataTable sessionRanking = DbConnect.Instance.GetSessionRanking();

        if (sessionRanking != null && sessionRanking.Rows.Count > 0)
        {
            firstPlaceSession.text = sessionRanking.Rows[0]["name"].ToString();
            scorefirstPlaceSession.text = sessionRanking.Rows[0]["score"].ToString();
        }
        else
        {
            firstPlaceSession.text = "No data";
            scorefirstPlaceSession.text = "-";
        }

        if (sessionRanking != null && sessionRanking.Rows.Count > 1)
        {
            secondPlaceSession.text = sessionRanking.Rows[1]["name"].ToString();
            scoresecondPlaceSession.text =sessionRanking.Rows[1]["score"].ToString();
        }
        else
        {
            secondPlaceSession.text = "No data";
            scoresecondPlaceSession.text = "-";
        }

        if (sessionRanking != null && sessionRanking.Rows.Count > 2)
        {
            thridPlaceSession.text = sessionRanking.Rows[2]["name"].ToString();
            scorethridPlaceSession.text = sessionRanking.Rows[2]["score"].ToString();
        }
        else
        {
            thridPlaceSession.text = "No data";
            scorethridPlaceSession.text = "-";
        }
    }

}
