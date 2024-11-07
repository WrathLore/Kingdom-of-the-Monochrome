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
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;//MAY PUT THIS ONE DYNAMICALLY OR WHATEVER SINCE SAME EVERYTIME
    [SerializeField] Button button;//will be the quest button no matter what
    [SerializeField] GameObject questGiver;
    [SerializeField] Player player;
    [SerializeField] Creature creature;//use to get the red green blue values
    [SerializeField] Dialogue d;//use for all the dialogue needs
    //[SerializeField] PickUp pick;
    [SerializeField] Button accept;//these to set the onClick listener for the buttons
    [SerializeField] Button turnIn;
    int track = 0;
    [Header("Writing")]
    [SerializeField] string text = "A giant snake lays curled up before you, blocking the only way out. The subtle movement of its body suggests it has already noticed you and is prepared for anything that comes next.";
    [SerializeField] string quest = "Pay attention to its body language.";//notice it tastes the air and keeps darting minutely towards any small rodents that scury past
    [SerializeField] string fight = "Continue forward, as cautiously as possible.";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!startedQuest)
            {
                if(turnIn != null)
                {
                    turnIn.gameObject.SetActive(false);
                }
                if(charText != null && fightButton != null && questButton != null)
                {
                    charText.text = text;
                    fightButton.text = fight;
                    questButton.text = quest;
                }
                SetName("Blue Snake");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
                if(button != null)
                {
                    button.onClick.AddListener(Quest);
                }
               
            }

            if(startedQuest)
            {
                Player.inQuest = true;
                d.SetName(creature.creatureName);
                if(track == 0)
                {
                    d.SetDialogue("TEST");
                }
                else if(track == 1)
                {
                    d.SetDialogue("TEST");
                }
                else if(track == 2)
                {
                    d.SetDialogue("TEST");
                }

            }
            if(accept != null)
            {
                accept.onClick.AddListener(Change);
            }
            turnIn.onClick.AddListener(TurnIn);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!startedQuest)
            {
                charText.text = "";
                fightButton.text = "";
                questButton.text = "";
                button.onClick.RemoveListener(Quest);
            }
            else
            {
                d.DeactivateDialogueBox();
            }
            accept.onClick.RemoveListener(Change);
            turnIn.onClick.RemoveListener(TurnIn);
          
        }
    }


    public void Quest()
    {
        track = 0;
        creature.choice.SetActive(false);
        startedQuest = true;//don't want to pull up the original text boxes, want the dialogue instead
        Player.inQuest = true;
        creature.interactedWith = true;
        d.SetName(creature.creatureName);
        d.SetDialogue("TEST");
    }

    public void Change()
    {
        track = 1;
        d.SetDialogue("TEST");

    }

    public void TurnIn()
    {
        track = 2;
        d.SetDialogue("TEST");
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public void Rewards()
    {
        if(creature != null)
        {
             player.QuestVictory(creature.red, creature.green, creature.blue,"TEST", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
