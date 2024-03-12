using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DatabaseHandler
{
    private static SQLiteConnection dbConnection;

    private readonly static string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SNEK GAME");
    private readonly static string dbName = "runs.sqlite";
    private readonly static string dbPath = Path.Combine(appDataPath, dbName);

    private static int id_num = 0;

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

        // create the connection and create the tables if they do not exist
        dbConnection = new SQLiteConnection(dbPath);
        dbConnection.CreateTable<Run>();
        dbConnection.CreateTable<ItemInfo>();
        dbConnection.CreateTable<BodyInfo>();

        // gets the current highest id number
        id_num = dbConnection.ExecuteScalar<int>("SELECT MAX(id) FROM Runs") + 1;
    }

    public static void AddRun(Run run, List<ItemInfo> items, List<BodyInfo> bodies)
    {
        run.ID = id_num;
        dbConnection.Insert(run);

        foreach (ItemInfo item in items)
        {
            item.RunID = id_num;
            dbConnection.Insert(item);
        }
        foreach (BodyInfo body in bodies)
        {
            body.RunID = id_num;
            dbConnection.Insert(body);
        }

        id_num++;
    }

    public static (List<Run>, int) GetPlayerRuns(string playerName, SortType sortType, bool ascending)
    {
        List<Run> runs = dbConnection.Query<Run>(SortBy("SELECT * FROM Runs WHERE player_name = ?", sortType, ascending), playerName);
        int result = dbConnection.ExecuteScalar<int>("SELECT count(*) FROM Runs WHERE player_name = ?", playerName);

        return (runs, result);
    }

    public static (List<Run>, int) GetSortedRuns(SortType sortType, bool ascending)
    {
        List<Run> runs = dbConnection.Query<Run>(SortBy("SELECT * FROM Runs", sortType, ascending));
        int result = dbConnection.ExecuteScalar<int>("SELECT count(*) FROM Runs");

        return (runs, result);
    }

    public static List<BodyInfo> GetBodyInfo(int runID)
    {
        List<BodyInfo> bodyInfos = dbConnection.Query<BodyInfo>(
        "SELECT b.id AS id, b.body_name AS body_name, b.run_id AS run_id, b.body_level AS body_level " +
        "FROM Runs r " +
        "INNER JOIN Bodies b ON b.run_id = r.id " +
        $"WHERE r.id = {runID}");

        return bodyInfos;
    }

    public static List<ItemInfo> GetItemInfo(int runID)
    {
        List<ItemInfo> itemInfos = dbConnection.Query<ItemInfo>(
        "SELECT i.id AS id, i.item_name AS item_name, i.run_id AS run_id, i.item_level AS item_level " +
        "FROM Runs r " +
        "INNER JOIN Items i ON i.run_id = r.id " +
        $"WHERE r.id = {runID}");

        return itemInfos;
    }

    public static Run GetRunInfo(int runID)
    {
        return dbConnection.Get<Run>(runID);
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
