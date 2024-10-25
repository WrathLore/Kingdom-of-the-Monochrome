using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedMaiden : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "The maiden before you stands resolute, both hands tight around the hilt of a sword. She stands firm and ready to fight though a tremble in her arm belies a hesitance.";
    [SerializeField] string quest = "Talk to her first";//she fears for the worst, sure that the only way to fix the colors is to fight for them(if most of your choices to this point are quests then she gives in, otherwise she fights anyways)
    [SerializeField] string fight = "Fight her";

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
