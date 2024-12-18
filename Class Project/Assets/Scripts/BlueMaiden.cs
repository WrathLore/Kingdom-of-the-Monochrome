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
    [SerializeField] string text = "A maiden dances through the field before you, whipping this way and that with sharp tipped whips. Getting close may be a problem, but going around seems impossible with the way she moves around almost erratically.";
    [SerializeField] string quest = "Look at the way she is moving and follow along behind to avoid the danger.";
    [SerializeField] string fight = "Wait for an opening and then charge forward to take the whips.";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] GameObject puzzleBase;
    public string puzzle = "Sculpted Field";
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
                SetName("Blue Maiden");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("Oh dear, I didn't see you there! Please be careful now. Though, if you'd like, maybe you could help me finish up my artwork? I was inspired by the water to make this field as perfect as can be!");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Oh thank you ever so much! Though please be quick, I would hate to accidentally harm you in the process.");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Oh my, watch out! Please be careful while working, don't want to risk an injury after all!");   
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
        accept.gameObject.SetActive(true);
        turnIn.gameObject.SetActive(false);
        creature.choice.SetActive(false);
        startedQuest = true;//don't want to pull up the original text boxes, want the dialogue instead
        Player.inQuest = true;
        creature.interactedWith = true;
        d.SetName(creature.creatureName);
        d.SetDialogue("Oh dear, I didn't see you there! Please be careful now. Though, if you'd like, maybe you could help me finish up my artwork? I was inspired by the water to make this field as perfect as can be!");
    }

    public void Change()
    {
        track = 1;
        puzzleBase.SetActive(true);
        puzzleScript.StartGame();
        d.DeactivateDialogueBox();
        d.SetDialogue("Oh thank you ever so much! Though please be quick, I would hate to accidentally harm you in the process.");
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
            d.SetDialogue("Oh my, watch out! Please be careful while working, don't want to risk an injury after all!");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"It looks amazing! Please, take this as gratitude and make sure to have fun out there! There's always room for improvement, but also room for more fun!", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
