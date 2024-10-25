using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlueMaiden : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "A maiden dances through the field before you, whipping this way and that with sharp tipped whips. Getting close may be a problem, but going around seems impossible with the way she moves around almost erratically.";
    [SerializeField] string quest = "Look at the way she is moving and follow along behind to avoid the danger.";
    [SerializeField] string fight = "Wait for an opening and then charge forward to take the whips.";

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
