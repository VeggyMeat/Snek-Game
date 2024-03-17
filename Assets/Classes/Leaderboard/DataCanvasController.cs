using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// COMPLETE

/// <summary>
/// The data canvas controller is placed on the data canvas game object
/// It is responsible for displaying the data of a particular run after a player clicks on a leaderboard entry
/// </summary>
public class DataCanvasController : MonoBehaviour
{
    /// <summary>
    /// The leaderboard manager
    /// </summary>
    [SerializeField] private LeaderboardManager leaderboardManager;

    /// <summary>
    /// The list of text objects that display the body names
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> bodyNames;

    /// <summary>
    /// The list of text objects that display the body levels
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> bodyLevels;

    /// <summary>
    /// The list of text objects that display the item names
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> itemNames;

    /// <summary>
    /// The list of text objects that display the item levels
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> itemLevels;

    /// <summary>
    /// The list of text objects that display the run info
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> runInfo;

    // Called as soon as the scene is loaded
    private void Awake()
    {
        // hides the canvas to the player
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the canvas
    /// </summary>
    public void ShowCanvas()
    {
        // show the canvas
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the canvas
    /// </summary>
    public void HideCanvas()
    {
        // hide the canvas
        gameObject.SetActive(false);

        leaderboardManager.Resume();
    }

    /// <summary>
    /// Loads the data of a particular run
    /// </summary>
    /// <param name="runID">The run id of the run</param>
    /// <exception cref="Exception">Exception thrown if the run data contains more items or bodies than can be displayed</exception>
    public void LoadData(int runID)
    {
        // gets the run info, item info and body info from the database
        Run run = DatabaseHandler.GetRunInfo(runID);
        List<ItemInfo> itemInfos = DatabaseHandler.GetItemInfo(runID);
        List<BodyInfo> bodyInfos = DatabaseHandler.GetBodyInfo(runID);

        if (bodyInfos.Count > 8)
        {
            throw new Exception("Too many bodies for the canvas to display");
        }
        if (itemInfos.Count > 8)
        {
            throw new Exception("Too many items for the canvas to display");
        }

        DisplayBodies(bodyInfos);
        DisplayItems(itemInfos);

        SetRunInfo(run.PlayerName, run.Score, run.Time, run.Date);
    }

    /// <summary>
    /// Displays the information of the bodies on the canvas' texts
    /// </summary>
    /// <param name="bodyInfos">The information for the bodies</param>
    private void DisplayBodies(List<BodyInfo> bodyInfos)
    {
        for (int i = 0; i < bodyNames.Count; i++)
        {
            // if there is a body to display
            if (i < bodyInfos.Count)
            {
                bodyNames[i].text = bodyInfos[i].Name;
                bodyLevels[i].text = bodyInfos[i].Level.ToString();
            }
            else
            {
                bodyNames[i].text = "";
                bodyLevels[i].text = "";
            }
        }
    }

    /// <summary>
    /// Displays the information of the items on the canvas' texts
    /// </summary>
    /// <param name="itemInfos">The information for the items</param>
    private void DisplayItems(List<ItemInfo> itemInfos)
    {
        for (int i = 0; i < itemNames.Count; i++)
        {
            // if there is an item to display
            if (i < itemInfos.Count)
            {
                itemNames[i].text = itemInfos[i].Name;
                itemLevels[i].text = itemInfos[i].Level.ToString();
            }
            else
            {
                itemNames[i].text = "";
                itemLevels[i].text = "";
            }
        }
    }

    /// <summary>
    /// Displays the run info on the canvas' texts
    /// </summary>
    /// <param name="playerName">The run's player name</param>
    /// <param name="score">The run's score</param>
    /// <param name="time">The run's time</param>
    /// <param name="date">The run's date</param>
    public void SetRunInfo(string playerName, int score, int time, DateTime date)
    {
        runInfo[0].text = playerName;
        runInfo[1].text = score.ToString();
        runInfo[2].text = time.ToString();
        runInfo[3].text = date.ToString();
    }
}
