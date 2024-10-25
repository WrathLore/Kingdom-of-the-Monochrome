using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedFairy : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "A fairy flits from bush to bush, looking for the ripest amongst the fruits. Any path forward is impossible to see as the fairy grows and shrinks the plants around you. They do not seem intent on stopping until their basket is full.";
    [SerializeField] string quest = "Aid the fairy in their task";//collect a certain number of berries(maybe in a time limit)
    [SerializeField] string fight = "Cut a path forward to continue";

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
