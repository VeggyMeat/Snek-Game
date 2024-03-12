using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataCanvasController : MonoBehaviour
{
    [SerializeField] private LeaderboardManager leaderboardManager;

    [SerializeField] private List<TextMeshProUGUI> bodyNames;
    [SerializeField] private List<TextMeshProUGUI> bodyLevels;
    [SerializeField] private List<TextMeshProUGUI> itemNames;
    [SerializeField] private List<TextMeshProUGUI> itemLevels;

    [SerializeField] private List<TextMeshProUGUI> runInfo;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowCanvas()
    {
        // show the canvas
        gameObject.SetActive(true);
    }

    public void HideCanvas()
    {
        // hide the canvas
        gameObject.SetActive(false);

        leaderboardManager.Resume();
    }

    public void LoadData(int runID)
    {
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

    private void DisplayBodies(List<BodyInfo> bodyInfos)
    {
        for (int i = 0; i < bodyNames.Count; i++)
        {
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

    private void DisplayItems(List<ItemInfo> itemInfos)
    {
        for (int i = 0; i < itemNames.Count; i++)
        {
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

    public void SetRunInfo(string playerName, int score, int time, DateTime date)
    {
        runInfo[0].text = playerName;
        runInfo[1].text = score.ToString();
        runInfo[2].text = time.ToString();
        runInfo[3].text = date.ToString();
    }
}
