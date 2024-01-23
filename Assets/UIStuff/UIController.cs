using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public HeadController headController;

    public Canvas ui;

    public int xpBarLength;

    private TextMeshProUGUI xpText;
    private TextMeshProUGUI killCount;
    private TextMeshProUGUI timer;

    void Start()
    {
        xpText = ui.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        killCount = ui.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    
        timer = ui.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

   void Update()
   {
        // dealing with the XP bar
        float percentageFull = headController.GetXPPercentage();

        int full = (int)(percentageFull * xpBarLength);
        xpText.text = $"[{new string('#', full) + new string('-', xpBarLength - full)}]";

        // dealing with the kill count
        killCount.text = $"Kills: {headController.enemySummonerController.enemiesDead}";

        // dealing with the timer
        timer.text = $"Time: {TimeManager.GetElapsedTime().ToString("mm':'ss")}";
   }
}
