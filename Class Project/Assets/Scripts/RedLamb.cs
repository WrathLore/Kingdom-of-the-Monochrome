using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class RedLamb : MonoBehaviour
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
    [SerializeField] Button accept;//these to set the onClick listener for the buttons
    [SerializeField] Button turnIn;
    int track = 0;//0 is for quest, 1 is for change, 2 is for wrong turn in

    [Header("Writing")]
    [SerializeField] string text = "The cleric before you looks frazzled, quite a bit of concern present in their eyes as they rush this way and that, hardly even noticing you. In their hand is a large gleaming axe held firmly by clearly experienced hands. You are unsure what they may do if you alert them to your presence.";
    [SerializeField] string quest = "Give them some space and call out to them.";
    [SerializeField] string fight = "Take no chances, a surprise attack would be best.";

    [Header("Quest Objects")]
    [SerializeField] GameObject door;//door to open when accepting quest
    [SerializeField] GameObject flowerToFind;//flower in a flower field to find, spawn in when quest accepted
    public string flower = "Flower";//quest item for player to pick up
    [SerializeField] bool startedQuest = false;



     void OnTriggerEnter2D(Collider2D other)//this one will change the words in the textboxes
    {
        if(other.CompareTag("Player"))//initial quest part
        {
            if(!startedQuest)
            {
                if(turnIn != null && accept != null)
                {
                    turnIn.gameObject.SetActive(false);
                    accept.gameObject.SetActive(true);
                }
                if(charText != null && fightButton != null && questButton != null)
                {
                    charText.text = text;
                    fightButton.text = fight;
                    questButton.text = quest;
                }
                SetName("Lamb Cleric");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
                if(button != null)
                {
                    button.onClick.AddListener(Quest);
                }

            }
            if(startedQuest)
            {
                d.SetName(creature.creatureName);//need to reset the name and dialogue in case another one shows up
                if(track == 0)
                {
                    accept.gameObject.SetActive(true);
                    turnIn.gameObject.SetActive(false);
                    d.SetDialogue("Oh my! I did not see you there! You look like you may be able to help. You see, I have been stuck guarding this location without rest for a long time. However, I wish to find a certain flower that still holds the characteristics of the old land to remind me why I stay here. If you could, would you be willing to find that flower in the field behind me?"); 
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Have you found it yet? I have been stuck here for ages, waiting for a chance to look myself, yet it never comes. Remember, it is a flower and a rather unique one at that. Please return once you have found it.");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("I see you you still have not found it. Would you mind looking once more? I require only a single flower from the garden that I am forced to watch over. I see it every so often, but a glimpse of the true colors of the world is not enough anymore. Please, help me bring my colors back.");
                }  
                Player.inQuest = true;
            }

            if(accept != null && turnIn != null)
            {
                accept.onClick.AddListener(Change);
                turnIn.onClick.AddListener(TurnIn);
            }
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
            if(accept != null && turnIn != null)
            {
                accept.onClick.RemoveListener(Change);
                turnIn.onClick.RemoveListener(TurnIn);
            }
          
        }
    }

    public void Change()
    {
        //need to open the door so that player can continue on through
        door.SetActive(false); //take the door out of the scene
        //need to spawn in the flower for player to find
        flowerToFind.SetActive(true); //put flower into the scene
        //will want to change the dialogue box as well as deactivate the choice box
        d.DeactivateDialogueBox();
        //need to mess around with the dialogue now for next time player presses T
        track = 1;
        d.SetDialogue("Have you found it yet? I have been stuck here for ages, waiting for a chance to look myself, yet it never comes. Remember, it is a flower and a rather unique one at that. Please return once you have found it.");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);
    }

    public void TurnIn()
    {
        foreach(string item in player.questItems)
        {
            if(string.Equals(flower,item))
            {
                track = 3;//JUST IN CASE
                Rewards();
                questGiver.SetActive(false);
                d.DeactivateDialogueBox();
                accept.onClick.RemoveListener(Change);
                turnIn.onClick.RemoveListener(TurnIn); 
                break;//need to break from the loop here  
            }
            
        }
        if(track != 3)
        {
            track = 2;
            d.SetDialogue("I see you you still have not found it. Would you mind looking once more? I require only a single flower from the garden that I am forced to watch over. I see it every so often, but a glimpse of the true colors of the world is not enough anymore. Please, help me bring my colors back.");
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
        d.SetDialogue("Oh my! I did not see you there! You look like you may be able to help. You see, I have been stuck guarding this location without rest for a long time. However, I wish to find a certain flower that still holds the characteristics of the old land to remind me why I stay here. If you could, would you be willing to find that flower in the field behind me?");   
    }
    //fights rely on panel in canvas for stuff, and random ranges for the fighting
    //so can probably just do one fight script and just have them call it

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
            if(creature.inTutorialQuest)
            {
                 player.QuestVictory(creature.red, creature.green, creature.blue,"You found it! Thank you so much! This will be very helpful for my state of mind! And, for helping me in such a way, I wish to grant you something in return. I have my own little slice of the old world thanks to you, so I wish to give you your own bit of that world as well.", creature.GetPercent(), creature.GetProgress(), true);   
            }
            else
            {
                player.QuestVictory(creature.red, creature.green, creature.blue,"You found it! Thank you so much! This will be very helpful for my state of mind! And, for helping me in such a way, I wish to grant you something in return. I have my own little slice of the old world thanks to you, so I wish to give you your own bit of that world as well.", creature.GetPercent(), creature.GetProgress());
            }
        }
        
    }
}
