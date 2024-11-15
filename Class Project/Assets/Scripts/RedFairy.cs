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
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;//MAY PUT THIS ONE DYNAMICALLY OR WHATEVER SINCE SAME EVERYTIME
    [SerializeField] Button button;//will be the quest button no matter what
    [SerializeField] GameObject questGiver;
    [SerializeField] Player player;
    [SerializeField] Creature creature;//use to get the red green blue values
    [SerializeField] Dialogue d;//use for all the dialogue needs
    [SerializeField] Button accept;//these to set the onClick listener for the buttons
    [SerializeField] Button turnIn;
    int track = 0;
    [Header("Writing")]
    [SerializeField] string text = "A fairy flits from bush to bush, looking for the ripest amongst the fruits. Any path forward is impossible to see as the fairy grows and shrinks the plants around you. They do not seem intent on stopping until their basket is full.";
    [SerializeField] string quest = "Aid the fairy in their task";//collect a certain number of berries(maybe in a time limit)
    [SerializeField] string fight = "Cut a path forward to continue";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] GameObject puzzleBase;
    public string puzzle = "Saved Berry Field";
    [SerializeField] PuzzleScript puzzleScript;

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
                SetName("Red Fairy");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    accept.gameObject.SetActive(true);
                    turnIn.gameObject.SetActive(false);
                    d.SetDialogue("Ah! Help? Much appreciated! Though I don't really need help gathering the berries, there is quite a bit of work to be done with finding the right colors. As you can see much of the color has been leached, so would you mind bringing it back? You seem quite skilled with colors so Ihave no doubt you'll be able to help!");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Thank you kindly! Would you mind starting soon? There's only so long these colors will stay so vibrant after all, and I fear the next chance will be far too late!");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Focus on the task at hand! The color is draining too fast for small talk!");  
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
        d.SetDialogue("Ah! Help? Much appreciated! Though I don't really need help gathering the berries, there is quite a bit of work to be done with finding the right colors. As you can see much of the color has been leached, so would you mind bringing it back? You seem quite skilled with colors so Ihave no doubt you'll be able to help!");
        
    }

    public void Change()
    {
        track = 1;
        puzzleBase.SetActive(true);
        puzzleScript.StartGame();
        d.DeactivateDialogueBox();
        d.SetDialogue("Thank you kindly! Would you mind starting soon? There's only so long these colors will stay so vibrant after all, and I fear the next chance will be far too late!");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);

    }

    public void TurnIn()
    {
        foreach(string item in player.questItems)
        {
            if(string.Equals(puzzle,item))
            {
                track = 3;
                puzzleScript.FinishGame();
                puzzleBase.SetActive(false);
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
            d.SetDialogue("Focus on the task at hand! The color is draining too fast for small talk!");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"Thank you kindly. I never would have been able to complete such a feat. Maybe you could finally end this horrid curse we live under. Hmm? Curse? Ah, pay no mind to my ramblings, but here, have some of that color for yourself and ... maybe just keep an eye out for any other signs of color and the help you could give them.", creature.GetPercent(), creature.GetProgress());
        }
        
    }

}
