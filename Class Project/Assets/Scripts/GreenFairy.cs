using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GreenFairy : MonoBehaviour
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
    [SerializeField] string text = "A whistle can be heard as a glowing fairy comes into view. They are lugging what appears to be a sword behind them, one that appears like it may fit into the puzzle ahead.";
    [SerializeField] string quest = "Ask for the sword from the fairy";//look for something to exchange with them
    [SerializeField] string fight = "Take the sword from the fairy";
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
                SetName("Green Fairy");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
        //activate the block door until quest is finished
        //spawn in the items to look for
        //maybe just offer them one at a time or work with a dropdown menu so items are not deleted
        //maybe try for a timer at some point
        //like 2 to 5 minutes to find the correct item
        //then once correct item is found, clear the item dropdown, and now have sword item
        //then walk up to door and use sword on it

        //could do a list with all the possible items in it and then random.range an int between 1 and however many items there are
        //to decide a different item each time that fairy wants

        //or to make timer better, only able to pick up one item at a time and if pressing P when item is already set, then can't pick it up
        //have to go back to fairy to see if it is the right item
        
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
}
