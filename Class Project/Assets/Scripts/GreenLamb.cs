using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GreenLamb : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "A cleric paces back and forth, grumbling under their breath. A shattered tile sits behind them, the pieces messed up and out of order. They seem very irritated, likely to lash out if you aren't careful.";
    [SerializeField] string quest = "Offer to sort out the tile pieces";//mix and match puzzle, or if too hard to code, another fetch quest
    [SerializeField] string fight = "Step over the tile to continue forward";

     void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            charText.text = text;
            fightButton.text = fight;
            questButton.text = quest;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
          charText.text = "";
          fightButton.text = "";
          questButton.text = "";
        }
    }

    public void Quest()
    {
        
    }
}
