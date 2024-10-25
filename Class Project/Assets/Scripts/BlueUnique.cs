using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlueUnique : MonoBehaviour
{  
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [Header("Writing")]
    [SerializeField] string text = "A giant snake lays curled up before you, blocking the only way out. The subtle movement of its body suggests it has already noticed you and is prepared for anything that comes next.";
    [SerializeField] string quest = "Pay attention to its body language.";//notice it tastes the air and keeps darting minutely towards any small rodents that scury past
    [SerializeField] string fight = "Continue forward, as cautiously as possible.";

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
