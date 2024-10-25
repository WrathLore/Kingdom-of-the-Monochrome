using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GreenUnique : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "A large stag blocks the way forward. It looks at you with intelligent eyes, pawing at the ground with one massive hoof as you approach. It seems on edge as it shifts one way and then another.";
    [SerializeField] string quest = "Look around to see what is unsettling such a large creature";//similar to the green maiden, fend off wolves for certain amount of time
    [SerializeField] string fight = "Charge forward and try to slip by the stag";

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
