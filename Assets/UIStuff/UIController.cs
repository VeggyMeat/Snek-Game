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

    void Start()
    {
        xpText = ui.GetComponentInChildren<TextMeshProUGUI>(ui);
    }

   void Update()
   {
        // dealing with the XP bar
        float percentageFull = headController.GetXPPercentage();
        // percentageFull %= 1;
        int full = (int)(percentageFull * xpBarLength);
        xpText.text = $"[{new string('#', full) + new string('-', xpBarLength - full)}]";
   }
}
