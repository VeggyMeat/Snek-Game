using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour, IDeathScreen
{
    [SerializeField]
    private GameObject nameBox;

    [SerializeField]
    private GameObject mainMenuButton;

    [SerializeField]
    private GameObject runInformation;

    [SerializeField]
    private GameObject gameOverText;

    [SerializeField]
    private GameObject deathScreen;

    private IGameSetup gameSetup;

    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    private void Awake()
    {
        nameBox.SetActive(false);
        mainMenuButton.SetActive(false);
        runInformation.SetActive(false);
        gameOverText.SetActive(false);
        deathScreen.SetActive(false);
    }

    public void OnDeath()
    {
        nameBox.SetActive(true);
        gameOverText.SetActive(true);
        deathScreen.SetActive(true);
    }

    /// <summary>
    /// Called when the player has typed their name and presses enter (only called by unity button)
    /// </summary>
    public void NameTyped()
    {
        string name = nameBox.GetComponent<TMP_InputField>().text;

        if (name.Length > 0)
        {
            List<string> runInformationData = gameSetup.HeadController.FinishRun(name);
            nameBox.SetActive(false);
            mainMenuButton.SetActive(true);
            runInformation.SetActive(true);

            SetRunInformation(runInformationData);
        }
    }

    private void SetRunInformation(List<string> runInformationData)
    {
        TextMeshProUGUI textBox = runInformation.GetComponent<TextMeshProUGUI>();

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
