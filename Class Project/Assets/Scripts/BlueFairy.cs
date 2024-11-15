using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlueFairy : MonoBehaviour
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
    [SerializeField] string text = "A fairy flitters this way and that, over a large bramble patch they seem to have created, muttering something about a locket. They appear to be looking for something, though they grow more agitated the longer you watch. If you aren't careful they may not react well to your presence.";
    [SerializeField] string quest = "Carefully move around the fairy and try to find a way around the brambles.";
    [SerializeField] string fight = "The way forward is blocked, the direct route is still the best.";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] GameObject mazeBlock; //turn off the maze block, turn on the maze 
    [SerializeField] GameObject maze; //to turn on the maze to travel through
    //[SerializeField] MazeScript mazeScript; //to connect with the maze handler
    //ABOVE WILL BE USED IF DECIDE TO GO WITH A PROCEDURALLY GENERATED MAZE
    [SerializeField] GameObject newLocation; //new spot to put fairy at
    [SerializeField] GameObject mazeExit;//turn off when finished with quest
    [SerializeField] string objectToCollect = "Cherished Locket";
    public bool finishedQuest = false;
    [SerializeField] TimerScript timer;
    [SerializeField] GameObject timerText;

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
                SetName("Blue Fairy");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("Oh hello. I was looking for my locket, but I'm having trouble holding back the brambles and searching. Would you mind terribly if you would search for my locket while I keep the brambles at bay? I'll be over by the exit to this awful bramble patch while you search.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Ah, I see you have found the exit! Any luck on the locket by the way? I can only hold back these brambles for so long I'm afraid.");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Sorry, but my locket is still not with you. Would you mind giving a second look to find it please?");  
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
        d.SetDialogue("Oh hello. I was looking for my locket, but I'm having trouble holding back the brambles and searching. Would you mind terribly if you would search for my locket while I keep the brambles at bay? I'll be over by the exit to this awful bramble patch while you search.");
        //once accepted, then the fairy will change position 
        //the mazeobjects will turn on
        //the maze block will be deactivated
        //the timer will start
        
    }

    public void Change()
    {
        timerText.SetActive(true);
        StartCoroutine(timer.CountDownRoutine("blueFairy"));
        mazeBlock.SetActive(false);
        maze.SetActive(true);
        transform.position = newLocation.transform.position;
        track = 1;
        d.DeactivateDialogueBox();
        d.SetDialogue("Ah, I see you have found the exit! Any luck on the locket by the way? I can only hold back these brambles for so long I'm afraid.");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);

    }

    public void TurnIn()
    {
        foreach(string item in player.questItems)
        {
            if(string.Equals(objectToCollect,item))
            {
                track = 3;//JUST IN CASE
                Rewards();
                questGiver.SetActive(false);
                mazeExit.SetActive(false);
                finishedQuest = true;
                timerText.SetActive(false);
                d.DeactivateDialogueBox();
                accept.onClick.RemoveListener(Change);
                turnIn.onClick.RemoveListener(TurnIn); 
                break;//need to break from the loop here  
            }
            
        }
        if(track != 3)
        {
            track = 2;
            d.SetDialogue("Sorry, but my locket is still not with you. Would you mind giving a second look to find it please?");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"TEST", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
