using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DatabaseHandler
{
    private static SQLiteConnection dbConnection;

    private readonly static string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SNEK GAME");
    private readonly static string dbName = "runs.sqlite";
    private readonly static string dbPath = Path.Combine(appDataPath, dbName);

    public static void Setup()
    {
        // if the folder doesn't exist, create it
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        // if the file doesn't exist, create it
        if (!File.Exists(dbPath))
        {
            File.Create(dbPath).Dispose();
        }

        // create the connection and create the table if it does not exist
        dbConnection = new SQLiteConnection(dbPath);
        dbConnection.CreateTable<Run>();
    }

    public static void AddRun(Run run)
    {
        dbConnection.Insert(run);
    }

    public static (List<Run>, int) GetPlayerRuns(string playerName, SortType sortType, bool ascending)
    {
        List<Run> runs = dbConnection.Query<Run>(SortBy("SELECT * FROM Runs WHERE player_name = ?", sortType, ascending), playerName);
        List<int> result = dbConnection.Query<int>("SELECT count(*) FROM Runs WHERE player_name = ?", playerName);

        Debug.Log(result[0]);

        if (result.Count != 1)
        {
            throw new Exception("COUNT request did not return 1 result");
        }

        return (runs, result[0]);
    }

    public static (List<Run>, int) GetSortedRuns(SortType sortType, bool ascending)
    {
        List<Run> runs = dbConnection.Query<Run>(SortBy("SELECT * FROM Runs", sortType, ascending));
        List<int> result = dbConnection.Query<int>("SELECT count(*) FROM Runs");

        if (result.Count != 1)
        {
            throw new Exception("COUNT request did not return 1 result");
        }

        return (runs, result[0]);
    }

    private static string SortBy(string query, SortType sortType, bool ascending)
    {
        query += " ORDER BY ";

        switch (sortType)
        {
            case SortType.PlayerName:
                query += "player_name";
                break;
            case SortType.Score:
                query += "score";
                break;
            case SortType.Time:
                query += "time";
                break;
            case SortType.Date:
                query += "date";
                break;
        }

        switch (ascending)
        {
            case true:
                query += " ASC";
                break;
            case false:
                query += " DESC";
                break;
        }

        return query;
    }
}
