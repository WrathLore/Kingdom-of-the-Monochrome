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
    [SerializeField] GameObject puzzleBase;//use to activate the puzzle, ie bring in the required material for it
    public string puzzle = "Completed Tile";//to bring back to the green lamb
    [SerializeField] PuzzleScript puzzleScript;//to start the puzzle up

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
                    accept.gameObject.SetActive(true);
                    turnIn.gameObject.SetActive(false);
                    d.SetDialogue("Ah, maybe you can help. You see, I have a problem, one of the tiles depicting our history has shattered, leaving behind a grey husk of what it should be. The pieces are still here, but the color is fading fast and I fear it may be lost if they are not placed correctly in time. Would you be willing to help?");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Please hurry! There is not much time left to fix this!");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Please, stop wasting time! Hurry over and place the tile back together.");
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
        //have a puzzle set up, grey pops into existence
        //have to layer the colored pieces on top to finish the puzzle
        //once done, then can have the finished puzzle
        creature.choice.SetActive(false);
        startedQuest = true;//don't want to pull up the original text boxes, want the dialogue instead
        Player.inQuest = true;
        creature.interactedWith = true;
        d.SetName(creature.creatureName);
        d.SetDialogue("Ah, maybe you can help. You see, I have a problem, one of the tiles depicting our history has shattered, leaving behind a grey husk of what it should be. The pieces are still here, but the color is fading fast and I fear it may be lost if they are not placed correctly in time. Would you be willing to help?");
    }

    public void Change()
    {
        track = 1;
        puzzleBase.SetActive(true);
        puzzleScript.StartGame();
        d.DeactivateDialogueBox();
        d.SetDialogue("Please hurry! There is not much time left to fix this!");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);

    }

    public void TurnIn()
    {
        foreach(string item in player.questItems)
        {
            if(string.Equals(puzzle,item))
            {
                track = 3;//JUST IN CASE
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
            d.SetDialogue("Please, stop wasting time! Hurry over and place the tile back together.");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"Oh goodness, you actually managed to do it! I thank you from the bottom of my heart. I do not know what I would have done if nothing could be done to save our history. You have done me a great favor here today and all I can offer now is this paltry reward. Please, accept it even with what little I have.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
