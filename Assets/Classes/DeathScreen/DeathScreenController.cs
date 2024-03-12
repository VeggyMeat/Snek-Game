using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// COMPLETE

/// <summary>
/// The controller for the death screen gameObject,
/// Which is shown to the player when the run ends
/// </summary>
public class DeathScreenController : MonoBehaviour, IDeathScreen
{
    /// <summary>
    /// The input field for the player to type their name
    /// </summary>
    [SerializeField]
    private GameObject nameBox;

    /// <summary>
    /// The button to return to the main menu
    /// </summary>
    [SerializeField]
    private GameObject mainMenuButton;

    /// <summary>
    /// The spot to display the run information
    /// </summary>
    [SerializeField]
    private GameObject runInformation;

    /// <summary>
    /// The text box saying 'Game Over'
    /// </summary>
    [SerializeField]
    private GameObject gameOverText;

    /// <summary>
    /// The background for the death screen
    /// </summary>
    [SerializeField]
    private GameObject deathScreen;

    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup"></param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    // Called by unity when the object is created (at the very start of the game)
    private void Awake()
    {
        // Hide all the UI elements
        nameBox.SetActive(false);
        mainMenuButton.SetActive(false);
        runInformation.SetActive(false);
        gameOverText.SetActive(false);
        deathScreen.SetActive(false);
    }

    /// <summary>
    /// Called when the player dies
    /// </summary>
    public void OnDeath()
    {
        // Show the UI elements
        nameBox.SetActive(true);
        gameOverText.SetActive(true);
        deathScreen.SetActive(true);
    }

    /// <summary>
    /// Called when the player has typed their name and presses enter (only called by unity button)
    /// </summary>
    public void NameTyped()
    {
        // grabs the name that the player typed
        string name = nameBox.GetComponent<TMP_InputField>().text;

        // if the player typed a name
        if (name.Length > 0)
        {
            // finish the run and get the run information
            List<string> runInformationData = gameSetup.HeadController.FinishRun(name);
            nameBox.SetActive(false);
            mainMenuButton.SetActive(true);
            runInformation.SetActive(true);

            SetRunInformation(runInformationData);
        }
    }

    /// <summary>
    /// Sets the run information to the UI
    /// </summary>
    /// <param name="runInformationData"></param>
    private void SetRunInformation(List<string> runInformationData)
    {
        // grabs the text box
        TextMeshProUGUI textBox = runInformation.GetComponent<TextMeshProUGUI>();

        // displays the run information in the text box
        textBox.text = $"Run Information\nName: {runInformationData[0]}\nScore: {runInformationData[1]}\nTime: {runInformationData[2]}\nDate: {runInformationData[3]}";
    }

    /// <summary>
    /// Called when the player presses the main menu button (only called by unity button)
    /// </summary>
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
