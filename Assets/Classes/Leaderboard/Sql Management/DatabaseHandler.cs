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

    public static List<Run> GetRuns()
    {
        // return all runs
        return dbConnection.Query<Run>(SortBy("SELECT * FROM Runs", SortType.Score, true));
    }

    public static List<Run> GetPlayerRuns(string playerName, SortType sortType, bool ascending)
    {
        // return all runs with the given player name
        return dbConnection.Query<Run>(SortBy("SELECT * FROM Runs WHERE player_name = ?", sortType, ascending), playerName);
    }

    public static List<Run> GetSortedRuns(SortType sortType, bool ascending)
    {
        return dbConnection.Query<Run>(SortBy("SELECT * FROM Runs", sortType, ascending));
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

    public static void CreateSampleData()
    {
        // create sample data
        Run run1 = new Run
        {
            PlayerName = "Player 1",
            Score = 100,
            Time = 100,
            Date = DateTime.Now
        };

        Run run2 = new Run
        {
            PlayerName = "Player 1",
            Score = 200,
            Time = 200,
            Date = DateTime.Now
        };

        Run run3 = new Run
        {
            PlayerName = "Player 3",
            Score = 300,
            Time = 300,
            Date = DateTime.Now
        };

        Run run4 = new Run
        {
            PlayerName = "Player 1",
            Score = 400,
            Time = 400,
            Date = DateTime.Now
        };

        Run run5 = new Run
        {
            PlayerName = "Player 5",
            Score = 500,
            Time = 500,
            Date = DateTime.Now
        };

        Run run6 = new Run
        {
            PlayerName = "Player 5",
            Score = 600,
            Time = 600,
            Date = DateTime.Now
        };

        Run run7 = new Run
        {
            PlayerName = "Player 7",
            Score = 700,
            Time = 700,
            Date = DateTime.Now
        };

        Run run8 = new Run
        {
            PlayerName = "Player 5",
            Score = 800,
            Time = 800,
            Date = DateTime.Now
        };

        Run run9 = new Run
        {
            PlayerName = "Player 9",
            Score = 900,
            Time = 900,
            Date = DateTime.Now
        };

        Run run10 = new Run
        {
            PlayerName = "Player 9",
            Score = 1000,
            Time = 1000,
            Date = DateTime.Now
        };

        // add sample data to the database
        AddRun(run1);
        AddRun(run2);
        AddRun(run3);
        AddRun(run4);
        AddRun(run5);
        AddRun(run6);
        AddRun(run7);
        AddRun(run8);
        AddRun(run9);
        AddRun(run10);  
    }
}
