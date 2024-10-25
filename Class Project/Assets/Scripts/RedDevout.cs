using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedDevout : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "The tapping of a cane can be heard as a crone hobbles down the path. She seems to be close to tipping over, but holds her cane with a strength unbefitting to her exterior. As you go to pass her, she suddenly tips over as her cane goes flying.";
    [SerializeField] string quest = "Help the woman to her feet";//look for the cane, easy quest
    [SerializeField] string fight = "Continue forward";//sort of based on beauty and the beast here, help the old woman out or risk being annihilated

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
