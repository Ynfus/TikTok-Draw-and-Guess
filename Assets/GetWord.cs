using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data;
using Mono.Data.Sqlite;
using UnityEngine.Scripting;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

public class GetWord : MonoBehaviour
{
    private string DataBaseName = "DrawAndGuess";
    private void Update()
    {
        GetRandomWord();
    }
    public void GetRandomWord()
    {
        string conn = "URI=file:Assets/Imports/StreamingAssets/DrawAndGuess.db";

        IDbCommand dbcmd;
        IDbConnection dbcon;
        IDataReader reader;

        dbcon = new SqliteConnection(conn);
        dbcon.Open();
        dbcmd = dbcon.CreateCommand();
        string SqlQuery = "SELECT * FROM Words ORDER BY RANDOM() LIMIT 1; ";
        dbcmd.CommandText = SqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log(reader.GetInt32(0));
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;



    }
}
