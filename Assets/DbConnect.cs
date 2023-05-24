using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DbConnect : MonoBehaviour
{
    private IDbConnection _dbConnection;
    private IDbCommand _dbCommand;

    [SerializeField] GameObject dropDatabaseOptions;

    const string conn = "URI=file:Assets/Imports/StreamingAssets/DrawAndGuess.db";
    public static DbConnect Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SQLiteManager(conn);
        CreateTable();
    }
    private void Start()
    {

    }
    private void SQLiteManager(string connectionString)
    {
        _dbConnection = new SqliteConnection(connectionString);
        _dbConnection.Open();
    }

    private void CreateTable()
    {
        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS score (id INTEGER PRIMARY KEY, name TEXT, score INTEGER)";
        _dbCommand.ExecuteNonQuery();
        _dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS session (id INTEGER PRIMARY KEY, name TEXT, score INTEGER)";
        _dbCommand.ExecuteNonQuery();
    }

    public void UpsertRanking(string name, int score)
    {
        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.Parameters.Add(new SqliteParameter("@name", name));
        _dbCommand.CommandText = "SELECT count(*) FROM session WHERE name = @name";
        int count = Convert.ToInt32(_dbCommand.ExecuteScalar());

        if (count > 0)
        {
            _dbCommand.CommandText = "UPDATE session SET score = score + @score WHERE name = @name";
            _dbCommand.Parameters.Add(new SqliteParameter("@score", score));
            _dbCommand.ExecuteNonQuery();
        }
        else
        {
            _dbCommand.CommandText = "INSERT INTO session (name, score) VALUES (@name, @score)";
            _dbCommand.Parameters.Add(new SqliteParameter("@score", score));
            _dbCommand.ExecuteNonQuery();
        }

        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.Parameters.Add(new SqliteParameter("@name", name));
        _dbCommand.CommandText = "SELECT count(*) FROM score WHERE name = @name";
        count = Convert.ToInt32(_dbCommand.ExecuteScalar());

        if (count > 0)
        {
            _dbCommand.CommandText = "UPDATE score SET score = score + @score WHERE name = @name";
            _dbCommand.Parameters.Add(new SqliteParameter("@score", score));
            _dbCommand.ExecuteNonQuery();
        }
        else
        {
            _dbCommand.CommandText = "INSERT INTO score (name, score) VALUES (@name, @score)";
            _dbCommand.Parameters.Add(new SqliteParameter("@score", score));
            _dbCommand.ExecuteNonQuery();
        }
    }


    public DataTable GetGlobalRanking()
    {
        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = "SELECT name, score FROM score ORDER BY score DESC limit 3";
        var dataAdapter = new SqliteDataAdapter((SqliteCommand)_dbCommand);
        var dataTable = new DataTable();
        dataAdapter.Fill(dataTable);
        return dataTable;
    }
    public DataTable GetSessionRanking()
    {
        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = "SELECT name, score FROM session ORDER BY score DESC limit 3";
        var dataAdapter = new SqliteDataAdapter((SqliteCommand)_dbCommand);
        var dataTable = new DataTable();
        dataAdapter.Fill(dataTable);
        return dataTable;
    }
    public List<string> GetRandomWords()
    {
        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = "SELECT * FROM Words ORDER BY RANDOM() LIMIT 3; ";
        IDataReader reader = _dbCommand.ExecuteReader();

        List<string> words = new List<string>();
        while (reader.Read())
        {
            words.Add(reader.GetString(1));
        }
        return words;
    }
    public void CloseConnection()
    {
        _dbConnection.Close();
    }
    private void OnApplicationQuit()
    {
        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = "DROP TABLE IF EXISTS session";
        _dbCommand.ExecuteNonQuery();
        CloseConnection();
    }
    public void ClearTable(string tableName)
    {
        _dbCommand = _dbConnection.CreateCommand();
        _dbCommand.CommandText = $"DELETE FROM {tableName}";
        _dbCommand.ExecuteNonQuery();
    }
    public void sessionButton_Click()
    {
        ClearTable("session");
        goBack();
    }
    public void openOptionsButton_Click()
    {
        dropDatabaseOptions.SetActive(true);
    }
    public void mainButton_Click()
    {
        ClearTable("score");
        goBack();
    }

    public void bothButton_Click()
    {
        ClearTable("session");
        ClearTable("score");
        goBack();
    }
    public void goBack()
    {
        dropDatabaseOptions.SetActive(false);
        Leadboard.Instance.UpdateRanking();

    }
}
