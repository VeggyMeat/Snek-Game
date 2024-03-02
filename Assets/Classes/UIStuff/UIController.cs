using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IUIController
{
    [SerializeField] private Canvas uICanvas;

    [SerializeField] private int xPBarLength;

    private TextMeshProUGUI xpText;
    private TextMeshProUGUI killCount;
    private TextMeshProUGUI timer;

    private IGameSetup gameSetup;

    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    private void Start()
    {
        xpText = uICanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        killCount = uICanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    
        timer = uICanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

   private void Update()
   {
        // dealing with the XP bar
        float percentageFull = gameSetup.HeadController.XPPercentage;

        int full = (int)(percentageFull * xPBarLength);
        xpText.text = $"[{new string('#', full) + new string('-', xPBarLength - full)}]";

        // dealing with the kill count
        killCount.text = $"Kills: {gameSetup.EnemySummonerController.EnemiesDead}";

        // dealing with the timer
        timer.text = $"Time: {TimeManager.GetElapsedTime().ToString("mm':'ss")}";
   }
}
