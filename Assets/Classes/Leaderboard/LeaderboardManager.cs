using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// COMPLETE

/// <summary>
/// The leaderboard manager is responsible for displaying the leaderboard to the player
/// It is a script placed on the leaderboard canvas' game object
/// </summary>
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
    /// Whether the player currently has the text field to pick a new name to search by open
    /// </summary>
    private bool pickName = false;

    /// <summary>
    /// The current name the player has chosen to search for
    /// </summary>
    private string chosenName = "";

    /// <summary>
    /// The game object object that displays the number of results found
    /// </summary>
    [SerializeField]
    private GameObject ResultNumberObject;

    /// <summary>
    /// The text object to display the number of results found
    /// </summary>
    private TextMeshProUGUI ResultNumber;

    /// <summary>
    /// The max number of pages that can be displayed
    /// </summary>
    private int MaxPage
    {
        get
        {
            return Mathf.CeilToInt(CurrentData.Count / (float)ROWS_PER_PAGE) - 1;
        }
    }

    /// <summary>
    /// Updates the result number text object
    /// </summary>
    private void UpdateResultNumber()
    {
        ResultNumber.text = $"{results} results found";
    }

    // Called by unity as soon as the scene is loaded
    private void Awake()
    {   
        ROWS_PER_PAGE = rows.Count;

        DatabaseHandler.Setup();

        // hides the name picker
        namePicker.SetActive(false);

        GetTextRows();

        ResultNumber = ResultNumberObject.GetComponent<TextMeshProUGUI>();
        
        // grabs the current data from the database
        (CurrentData, results) = DatabaseHandler.GetSortedRuns(sortType, asc);

        Reorder();
    }

    /// <summary>
    /// Grabs the text objects from the rows
    /// </summary>
    private void GetTextRows()
    {
        // goes through each row and grabs the text objects from them
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
    }

    /// <summary>
    /// Sorts by a number representing the sort type (called by unity buttons)
    /// </summary>
    /// <param name="sortNum"></param>
    public void SortBy(int sortNum)
    {
        SortType sortType = (SortType)sortNum;

        SortBy(sortType);
    }

    /// <summary>
    /// Changes the way the data is sorted
    /// </summary>
    /// <param name="sortType">The new way to sort by</param>
    public void SortBy(SortType sortType)
    {
        // if it is already being sorted by this type, reverse the order
        if (this.sortType == sortType)
        {
            asc = !asc;
        }
        else
        {
            this.sortType = sortType;
            asc = false;
        }

        // resets the page number
        page = 0;

        // gets the current data from the database
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

    /// <summary>
    /// Clears the text of the leaderboard
    /// </summary>
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

    /// <summary>
    /// Reorders the leaderboard
    /// </summary>
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

    /// <summary>
    /// Shows the next page of data (if there is one) (called by the unity button)
    /// </summary>
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

    /// <summary>
    /// Shows the previous page of data (if there is one) (called by the unity button)
    /// </summary>
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

    /// <summary>
    /// Called by the unity button when the player wants to pick a name to search by
    /// </summary>
    public void PickName()
    {
        pickName = true;
        
        // shows the name picker
        namePicker.SetActive(pickName);
    }

    /// <summary>
    /// Called by the unity text field after the player has submitted their picked name
    /// </summary>
    public void SubmitName()
    {
        chosenName = namePicker.GetComponent<TMP_InputField>().text;

        pickName = false;

        namePicker.SetActive(false);

        SortBy((int)sortType);
    }

    /// <summary>
    /// Called by the unity button to set the scene back to the main menu scene
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Shows the data of a particular run (Called by the unity button when the player presses a field)
    /// </summary>
    /// <param name="row">The row that was pressed</param>
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

    /// <summary>
    /// Resumes the leaderboard being shown
    /// </summary>
    public void Resume()
    {
        gameObject.SetActive(true);
    }
}
