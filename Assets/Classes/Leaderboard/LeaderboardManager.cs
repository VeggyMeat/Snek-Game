using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    /// <summary>
    /// Contains the rows of the leaderboard
    /// Each row has a number of gameObjects which contain a piece of text
    /// The row is made up of Name, Score, Time, and Date
    /// </summary>
    [SerializeField] private List<GameObject> rows;

    /// <summary>
    /// The Text field in which the player can enter the name to search for
    /// </summary>
    [SerializeField] private GameObject namePicker;

    [SerializeField] private DataCanvasController dataCanvas;

    /// <summary>
    /// How many rows to be shown on the leaderboard
    /// </summary>
    private int ROWS_PER_PAGE;

    /// <summary>
    /// How many collumns there are within each row
    /// </summary>
    private const int COLUMNS = 4;

    /// <summary>
    /// The rows of the leaderboard split into their individual text objects
    /// </summary>
    private List<List<TextMeshProUGUI>> splitRows;

    /// <summary>
    /// The current data to be shown on the leaderboard
    /// </summary>
    private List<Run> currentData;

    /// <summary>
    /// The current data to be shown on the leaderboard
    /// </summary>
    private List<Run> CurrentData
    {
        get
        {
            return currentData;
        }
        set
        {
            currentData = value;

            // when the data is changed, the result number is updated
            UpdateResultNumber();
        }
    }

    /// <summary>
    /// Contains the current way the data is being sorted
    /// </summary>
    private SortType sortType = SortType.Date;

    /// <summary>
    /// Whether the data is beign sorted in ascending or descending order
    /// </summary>
    private bool asc = false;

    /// <summary>
    /// What page number the leaderboard is currently displaying
    /// </summary>
    private int page = 0;

    /// <summary>
    /// The number of results recieved
    /// </summary>
    private int? results = null;

    /// <summary>
    /// 
    /// </summary>
    private bool pickName = false;

    private string chosenName = "";

    [SerializeField]
    private GameObject ResultNumberObject;
    private TextMeshProUGUI ResultNumber;

    private int MaxPage
    {
        get
        {
            return Mathf.CeilToInt(CurrentData.Count / (float)ROWS_PER_PAGE) - 1;
        }
    }

    private void UpdateResultNumber()
    {
        ResultNumber.text = $"{results} results found";
    }

    private void Awake()
    {   
        ROWS_PER_PAGE = rows.Count;

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

        ResultNumber = ResultNumberObject.GetComponent<TextMeshProUGUI>();

        (CurrentData, results) = DatabaseHandler.GetSortedRuns(sortType, asc);
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
            (CurrentData, results) = DatabaseHandler.GetSortedRuns(sortType, asc);
        }
        else
        {
            (CurrentData, results) = DatabaseHandler.GetPlayerRuns(chosenName, sortType, asc);
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
        endNum = Mathf.Min(endNum, CurrentData.Count);

        // goes through each number on the page
        for (int i = startNum; i < endNum; i++)
        {
            Run run = CurrentData[i];

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

    public void ShowData(int row)
    {
        // if the row is out of range, ignore it
        if (row > CurrentData.Count - 1)
        {
            return;
        }

        dataCanvas.ShowCanvas();

        dataCanvas.LoadData(CurrentData[page * ROWS_PER_PAGE + row].ID);

        gameObject.SetActive(false);
    }

    public void Resume()
    {
        gameObject.SetActive(true);
    }
}
