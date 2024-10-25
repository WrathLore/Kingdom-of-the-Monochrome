using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlueFairy : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "A fairy flitters this way and that, over a large bramble patch they seem to have created, muttering something about a locket. They appear to be looking for something, though they grow more agitated the longer you watch. If you aren't careful they may not react well to your presence.";
    [SerializeField] string quest = "Carefully move around the fairy and try to find a way around the brambles.";
    [SerializeField] string fight = "The way forward is blocked, the direct route is still the best.";

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
