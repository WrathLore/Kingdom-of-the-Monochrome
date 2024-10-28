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
    [SerializeField] string text = "A cleric paces back and forth, grumbling under their breath. A shattered tile sits behind them, the pieces messed up and out of order. They seem very irritated, likely to lash out if you aren't careful.";
    [SerializeField] string quest = "Offer to sort out the tile pieces";//mix and match puzzle, or if too hard to code, another fetch quest
    [SerializeField] string fight = "Step over the tile to continue forward";
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
                SetName("Green Lamb");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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

                }
                else if(track == 1)
                {

                }
                else if(track == 2)
                {
                    
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
        //have a puzzle set up, grey pops into existence
        //have to layer the colored pieces on top to finish the puzzle
        //once done, then can have the finished puzzle
        creature.choice.SetActive(false);
        startedQuest = true;//don't want to pull up the original text boxes, want the dialogue instead
        Player.inQuest = true;
        creature.interactedWith = true;
        d.SetName(creature.creatureName);
        
    }

    public void Change()
    {
        track = 1;

    }

    public void TurnIn()
    {
        track = 2;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }
}
