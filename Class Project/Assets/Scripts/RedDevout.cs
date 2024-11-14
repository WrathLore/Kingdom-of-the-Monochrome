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
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;//MAY PUT THIS ONE DYNAMICALLY OR WHATEVER SINCE SAME EVERYTIME
    [SerializeField] Button button;//will be the quest button no matter what
    [SerializeField] GameObject questGiver;
    [SerializeField] Player player;
    [SerializeField] Creature creature;//use to get the red green blue values
    [SerializeField] Dialogue d;//use for all the dialogue needs
    [SerializeField] Button accept;//these to set the onClick listener for the buttons
    [SerializeField] Button turnIn;
    [SerializeField] TimerScript timer;
    int track = 0;
    [Header("Writing")]
    [SerializeField] string text = "The tapping of a cane can be heard as a crone hobbles down the path. She seems to be close to tipping over, but holds her cane with a strength unbefitting to her exterior. As you go to pass her, she suddenly tips over as her cane goes flying.";
    [SerializeField] string quest = "Help the woman to her feet";//look for the cane, easy quest
    [SerializeField] string fight = "Continue forward";//sort of based on beauty and the beast here, help the old woman out or risk being annihilated
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] string cane = "Red Cane";
    //but if trying to pick up the wrong one, it prompts the dialogue box to pop up and the character to remark on it being the wrong one
    //MAY COME BACK TO DO THIS: could have list of strings to say and pick a random one each time the wrong cane is chosen
    [SerializeField] GameObject caneObjects;
    public bool wrongItem = false;

    void Update()
    {
        if(wrongItem)
        {
            d.SetName(creature.creatureName);
            d.SetDialogue("That's the wrong item! Put that back and find my cane!");
            d.ActivateDialogueBox();
            wrongItem = false;
            if(timer != null)
            {
                StartCoroutine(timer.ClosePopUpRoutine(5));
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
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
                SetName("Red Devout");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
                if(button != null)
                {
                    button.onClick.AddListener(Quest);
                }
        
            }

            if(startedQuest)
            {
                Player.inQuest = true;//if start 2 quests at once, then inQuest is set to false when completing one and unable to talk to other again
                d.SetName(creature.creatureName);
                if(track == 0)
                {
                    accept.gameObject.SetActive(true);
                    turnIn.gameObject.SetActive(false);
                    d.SetDialogue("Well, at least there's some good still left in the world. Augh, but all this color that remains certainly doesn't help. Look at that sucking my own cane dry of its color. Would you help me find my cane? I'm unable to walk well without it, but I can tell as soon as you pick it up which one is mine.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Thank you kindly. Don't you worry, I'll be sure to shout out to you if you can't find it on the first try or the second or the third...");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Why are you coming back now? I told you I'd give you a shout if you chose wrong, so why wander back anyways? Get back to it will you?"); 
                }
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


    public void Quest()
    {
        track = 0;
        creature.choice.SetActive(false);
        startedQuest = true;//don't want to pull up the original text boxes, want the dialogue instead
        Player.inQuest = true;
        creature.interactedWith = true;
        d.SetName(creature.creatureName);
        d.SetDialogue("Well, at least there's some good still left in the world. Augh, but all this color that remains certainly doesn't help. Look at that sucking my own cane dry of its color. Would you help me find my cane? I'm unable to walk well without it, but I can tell as soon as you pick it up which one is mine.");
    }

    public void Change()
    {
        track = 1;
        caneObjects.SetActive(true);
        d.DeactivateDialogueBox();
        d.SetDialogue("Thank you kindly. Don't you worry, I'll be sure to shout out to you if you can't find it on the first try or the second or the third...");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);
        
    }

    public void TurnIn()
    {
        foreach(string item in player.questItems)
        {
            if(string.Equals(cane,item))
            {
                track = 3;//JUST IN CASE
                Rewards();
                questGiver.SetActive(false);
                caneObjects.SetActive(false);
                d.DeactivateDialogueBox();
                accept.onClick.RemoveListener(Change);
                turnIn.onClick.RemoveListener(TurnIn); 
                break;//need to break from the loop here  
            }
            
        }
        if(track != 3)
        {
            track = 2;
            d.SetDialogue("Why are you coming back now? I told you I'd give you a shout if you chose wrong, so why wander back anyways? Get back to it will you?");
        }

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
             player.QuestVictory(creature.red, creature.green, creature.blue,"Ah, finally. Thank you kindly. As a reward, how's about you take all that color that the ground managed to leach from my staff? Don't worry about me, I get the sense you'll put it to much better use elsewhere than if it stayed with a grouchy old woman like me.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
