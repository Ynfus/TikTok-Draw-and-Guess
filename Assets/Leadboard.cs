using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class Leadboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstPlace;
    [SerializeField] TextMeshProUGUI secondPlace;
    [SerializeField] TextMeshProUGUI thridPlace;
    [SerializeField] TextMeshProUGUI firstPlaceSession;
    [SerializeField] TextMeshProUGUI secondPlaceSession;
    [SerializeField] TextMeshProUGUI thridPlaceSession;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnStateChanged += Leadboard_OnStateChanged;
        UpdateRanking();
    }

    private void Leadboard_OnStateChanged(object sender, System.EventArgs e)
    {
        if (gameManager.IsSucced())
        {
            UpdateRanking();
        }

    }

    void UpdateRanking()
    {
        // Pobierz wyniki globalne
        DataTable globalRanking = DbConnect.Instance.GetGlobalRanking();

        // Przypisz wyniki do pól tekstowych
        if (globalRanking.Rows.Count > 0)
        {
            firstPlace.text = globalRanking.Rows[0]["name"] + " - " + globalRanking.Rows[0]["score"];
        }
        if (globalRanking.Rows.Count > 1)
        {
            secondPlace.text = globalRanking.Rows[1]["name"] + " - " + globalRanking.Rows[1]["score"];
        }
        if (globalRanking.Rows.Count > 2)
        {
            thridPlace.text = globalRanking.Rows[2]["name"] + " - " + globalRanking.Rows[2]["score"];
        }

        // Pobierz wyniki sesji
        DataTable sessionRanking = DbConnect.Instance.GetSessionRanking();

        // Przypisz wyniki do pól tekstowych
        if (sessionRanking.Rows.Count > 0)
        {
            firstPlaceSession.text = sessionRanking.Rows[0]["name"] + " - " + sessionRanking.Rows[0]["score"];
        }
        if (sessionRanking.Rows.Count > 1)
        {
            secondPlaceSession.text = sessionRanking.Rows[1]["name"] + " - " + sessionRanking.Rows[1]["score"];
        }
        if (sessionRanking.Rows.Count > 2)
        {
            thridPlaceSession.text = sessionRanking.Rows[2]["name"] + " - " + sessionRanking.Rows[2]["score"];
        }
    }

}
