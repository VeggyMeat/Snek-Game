using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

// COMPLETE

/// <summary>
/// The database handler class is responsible for handling all database operations.
/// </summary>
public static class DatabaseHandler
{
    /// <summary>
    /// The connection to the database.
    /// </summary>
    private static SQLiteConnection dbConnection;

    /// <summary>
    /// The path to the database folder
    /// </summary>
    private readonly static string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SNEK GAME");
    
    /// <summary>
    /// The name of the database file
    /// </summary>
    private readonly static string dbName = "runs.sqlite";
    
    /// <summary>
    /// The path to the database file
    /// </summary>
    private readonly static string dbPath = Path.Combine(appDataPath, dbName);

    /// <summary>
    /// The current highest run id number
    /// </summary>
    private static int id_num = 0;

    /// <summary>
    /// Called to setup the database connection and create the tables if they do not exist
    /// </summary>
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

    /// <summary>
    /// Adds a run to the database
    /// </summary>
    /// <param name="run">The run to add</param>
    /// <param name="items">The items the player had</param>
    /// <param name="bodies">The bodies that were in the snake</param>
    public static void AddRun(Run run, List<ItemInfo> items, List<BodyInfo> bodies)
    {
        // set the id of the run and insert it into the database
        run.ID = id_num;
        dbConnection.Insert(run);

        // set the run id of the items and adds them to the database
        foreach (ItemInfo item in items)
        {
            item.RunID = id_num;
            dbConnection.Insert(item);
        }

        // set the run id of the bodies and adds them to the database
        foreach (BodyInfo body in bodies)
        {
            body.RunID = id_num;
            dbConnection.Insert(body);
        }

        id_num++;
    }

    /// <summary>
    /// Gets all the player runs in the database, matching a particular name, sorted by a particular type and in a particular order
    /// </summary>
    /// <param name="playerName">The name of the player to search for</param>
    /// <param name="sortType">The condition which the runs should be sorted by</param>
    /// <param name="ascending">Whether the runs should be ascending (true) or descending (false)</param>
    /// <returns></returns>
    public static (List<Run>, int) GetPlayerRuns(string playerName, SortType sortType, bool ascending)
    {
        List<Run> runs = dbConnection.Query<Run>(SortBy("SELECT * FROM Runs WHERE player_name = ?", sortType, ascending), playerName);
        int result = dbConnection.ExecuteScalar<int>("SELECT count(*) FROM Runs WHERE player_name = ?", playerName);

        return (runs, result);
    }

    /// <summary>
    /// Gets all the runs in the database, sorted by a particular type and in a particular order
    /// </summary>
    /// <param name="sortType">The condition which the runs should be sorted by</param>
    /// <param name="ascending">Whether the runs should be ascending (true) or descending (false)</param>
    /// <returns></returns>
    public static (List<Run>, int) GetSortedRuns(SortType sortType, bool ascending)
    {
        List<Run> runs = dbConnection.Query<Run>(SortBy("SELECT * FROM Runs", sortType, ascending));
        int result = dbConnection.ExecuteScalar<int>("SELECT count(*) FROM Runs");

        return (runs, result);
    }

    /// <summary>
    /// Gets all the bodies in the database, matching a particular run id
    /// </summary>
    /// <param name="runID">The run id to match from</param>
    /// <returns>The bodies returned</returns>
    public static List<BodyInfo> GetBodyInfo(int runID)
    {
        List<BodyInfo> bodyInfos = dbConnection.Query<BodyInfo>(
        "SELECT b.id AS id, b.body_name AS body_name, b.run_id AS run_id, b.body_level AS body_level " +
        "FROM Runs r " +
        "INNER JOIN Bodies b ON b.run_id = r.id " +
        $"WHERE r.id = {runID}");

        return bodyInfos;
    }

    /// <summary>
    /// Gets all the items in the database, matching a particular run id
    /// </summary>
    /// <param name="runID">The run id to match from</param>
    /// <returns>The items returned</returns>
    public static List<ItemInfo> GetItemInfo(int runID)
    {
        List<ItemInfo> itemInfos = dbConnection.Query<ItemInfo>(
        "SELECT i.id AS id, i.item_name AS item_name, i.run_id AS run_id, i.item_level AS item_level " +
        "FROM Runs r " +
        "INNER JOIN Items i ON i.run_id = r.id " +
        $"WHERE r.id = {runID}");

        return itemInfos;
    }

    /// <summary>
    /// Gets the run info for a particular run id
    /// </summary>
    /// <param name="runID">The run id to get the run info of</param>
    /// <returns>The run matching the run id</returns>
    public static Run GetRunInfo(int runID)
    {
        return dbConnection.Get<Run>(runID);
    }

    /// <summary>
    /// Adjusts a query to sort by a particular type and in a particular order
    /// </summary>
    /// <param name="query">The query text to edit</param>
    /// <param name="sortType">The condition which the runs should be sorted by</param>
    /// <param name="ascending">Whether the runs should be ascending (true) or descending (false)</param>
    /// <returns></returns>
    private static string SortBy(string query, SortType sortType, bool ascending)
    {
        query += " ORDER BY ";

        // adds the appropriate sort type to the query
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

        // adds the appropriate order to the query
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
