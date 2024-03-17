using TMPro;
using UnityEngine;

// COMPLETE

/// <summary>
/// The controller for the UI canvas
/// </summary>
public class UIController : MonoBehaviour, IUIController
{
    /// <summary>
    /// The canvas for the UI
    /// </summary>
    [SerializeField] private Canvas uICanvas;

    /// <summary>
    /// The number of characters in the XP bar
    /// </summary>
    [SerializeField] private int xPBarLength;

    /// <summary>
    /// The text field for the XP bar
    /// </summary>
    private TextMeshProUGUI xpText;

    /// <summary>
    /// The text field for the kill count
    /// </summary>
    private TextMeshProUGUI killCount;

    /// <summary>
    /// The text field for the timer
    /// </summary>
    private TextMeshProUGUI timer;

    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    // Called by unity on the first frame
    private void Start()
    {
        xpText = uICanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        killCount = uICanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    
        timer = uICanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    // Called by unity every frame
   private void Update()
   {
        float percentageFull = gameSetup.HeadController.XPPercentage;

        // get the number of characters to fill in the xp bar
        int full = (int)(percentageFull * xPBarLength);

        // set the text of the xp bar
        xpText.text = $"[{new string('#', full) + new string('-', xPBarLength - full)}]";

        killCount.text = $"Kills: {gameSetup.EnemySummonerController.EnemiesDead}";

        timer.text = $"Time: {TimeManager.GetElapsedTime().ToString("mm':'ss")}";
   }
}
