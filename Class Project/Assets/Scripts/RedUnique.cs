using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedUnique : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "A winged entity blocks the way forward. A greatsword held with ease in one hand as they stand at the ready; wings splayed and muscles tensed. They stand in the middle of a small circle and it seems there is no way out without a fight.";
    [SerializeField] string quest = "Step into the circle";//dodge projectiles from the figure, maybe something with a time limit as well
    [SerializeField] string fight = "Attack from afar";

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
