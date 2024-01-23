using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> rows;

    [SerializeField]
    private GameObject namePicker;

    private const int ROWS_PER_PAGE = 10;
    private const int COLUMNS = 4;

    private List<List<TextMeshProUGUI>> splitRows;

    private List<Run> currentData;

    private SortType sortType = SortType.Date;
    private bool asc = false;

    private int page = 0;

    private bool pickName = false;

    private string chosenName = "";

    private int MaxPage
    {
        get
        {
            return Mathf.CeilToInt(currentData.Count / (float)ROWS_PER_PAGE) - 1;
        }
    }

    private void Awake()
    {
        DatabaseHandler.Setup();

        namePicker.SetActive(false);

        splitRows = new List<List<TextMeshProUGUI>>();
        for (int i = 0; i < ROWS_PER_PAGE; i += 1)
        {
            List<TextMeshProUGUI> newRow = new List<TextMeshProUGUI>();
            for (int j = 0; j < COLUMNS; j++)
            {
                newRow.Add(rows[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>());
            }

            splitRows.Add(newRow);
        }

        currentData = DatabaseHandler.GetSortedRuns(sortType, asc);
        Reorder();
    }

    public void SortBy(int sortNum)
    {
        SortType sortType = (SortType)sortNum;

        SortBy(sortType);
    }

    public void SortBy(SortType sortType)
    {
        if (this.sortType == sortType)
        {
            asc = !asc;
        }
        else
        {
            this.sortType = sortType;
            asc = false;
        }

        page = 0;

        if (chosenName == "")
        {
            currentData = DatabaseHandler.GetSortedRuns(sortType, asc);
        }
        else
        {
            currentData = DatabaseHandler.GetPlayerRuns(chosenName, sortType, asc);
        }
        
        Reorder();
    }

    public void Clear()
    {
        foreach (List<TextMeshProUGUI> row in splitRows)
        {
            foreach (TextMeshProUGUI text in row)
            {
                text.text = "";
            }
        }
    }

    private void Reorder()
    {
        Clear();

        // gets the start and end index of the current page
        int startNum = page * ROWS_PER_PAGE;
        int endNum = startNum + ROWS_PER_PAGE;
        endNum = Mathf.Min(endNum, currentData.Count);

        // goes through each number on the page
        for (int i = startNum; i < endNum; i++)
        {
            Run run = currentData[i];

            // sets the text of the row to the data of the run
            splitRows[i - startNum][0].text = run.PlayerName;
            splitRows[i - startNum][1].text = run.Score.ToString();
            splitRows[i - startNum][2].text = run.Time.ToString();
            splitRows[i - startNum][3].text = run.Date.ToString();
        }
    }

    public void NextPage()
    {
        page++;

        if (page > MaxPage)
        {
            page = MaxPage;
        }
        else
        {
            Reorder();
        }
    }

    public void PrevPage()
    {
        page--;

        if (page < 0)
        {
            page = 0;
        }
        else
        {
            Reorder();
        }
    }

    public void PickName()
    {
        pickName = true;
        
        namePicker.SetActive(pickName);
    }

    public void SubmitName()
    {
        chosenName = namePicker.GetComponent<TMP_InputField>().text;

        pickName = false;

        namePicker.SetActive(false);

        SortBy((int)sortType);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
