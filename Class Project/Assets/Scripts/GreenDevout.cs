using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GreenDevout : MonoBehaviour
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
    [SerializeField] string text = "An old woman sits before a shrine, head bent down and muttering frantic devotions. Around her neck is a key one that looks like it fits the lock to the only other door in the room.";
    [SerializeField] string quest = "Kneel beside the old woman";//sends you on a fetch quest to get 3 items from a hidden room
    [SerializeField] string fight = "Steal the key";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] GameObject shrineDoor;
    [SerializeField] GameObject secretRoom;
    public string key = "Shrine Key";
    public int itemsPicked = 0;//needs to be 3 to check with finishing the quest
    public int correctItems = 0;
    public string towerItem = "Wrong";
    public string baubleItem = "Wrong";
    public string statueItem = "Wrong";
    bool foundTower = false;
    bool foundBauble = false;
    bool foundStatue = false;
    public bool tower = false;
    public bool bauble = false;
    public bool statue = false;
    [SerializeField] TimerScript timer;
    [SerializeField] GameObject timerText;


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
                SetName("Green Devout");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("You are not from these lands are you? And yet you indulge an old woman while she prays? Many thanks for the prayers. I'm sure you would like to continue on further into these lands, but would you mind helping an old woman one more time?");
                }
                else if(track == 1)
                {
                    d.SetDialogue("Thank you kindly. I've revealed a secret room behind me. I ask that you find the true forms of the objects within and bring them back to me. Try as many times as you want, but be sure to keep an eye on the time please!");
                }
                else if(track == 2)
                {
                    d.SetDialogue("Not enough items I see! Come back with one of each item so I can check them over.");
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
        d.SetDialogue("You are not from these lands are you? And yet you indulge an old woman while she prays? Many thanks for the prayers. I'm sure you would like to continue on further into these lands, but would you mind helping an old woman one more time?");
        
    }

    public void Change()
    {
        timerText.SetActive(true);
        StartCoroutine(timer.CountDownRoutine("greenDevout"));
        d.DeactivateDialogueBox();
        secretRoom.SetActive(true);
        track = 1;
        d.SetDialogue("Thank you kindly. I've revealed a secret room behind me. I ask that you find the true forms of the objects within and bring them back to me. Try as many times as you want, but be sure to keep an eye on the time please!");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);
    }

    public void TurnIn()
    {
        if(itemsPicked == 3)
        {
            if(string.Equals(towerItem,"Tower") && !foundTower)
            {
                foundTower = true;
                correctItems++;
            }
            if(string.Equals(statueItem,"Statue") && !foundStatue)
            {
                foundStatue = true;
                correctItems++;
            }
            if(string.Equals(baubleItem,"Bauble") && !foundBauble)
            {
                foundBauble = true;
                correctItems++;
            }
            if(correctItems == 3)
            {
                track = 3;//JUST IN CASE
                player.RegisterItem(key);
                timerText.SetActive(false);
                Rewards();
                questGiver.SetActive(false);
                d.DeactivateDialogueBox();
                accept.onClick.RemoveListener(Change);
                turnIn.onClick.RemoveListener(TurnIn); 

            }
            else
            {
                string towerCorrect = "Tower has been found";
                string baubleCorrect = "Bauble has been found";
                string statueCorrect = "Statue has been found";
                if(string.Equals(towerItem,"Wrong"))
                {
                    tower = false;
                    itemsPicked--;
                    towerCorrect = "Tower has not been found";
                }
                if(string.Equals(statueItem,"Wrong"))
                {
                    statue = false;
                    itemsPicked--;
                    statueCorrect = "Statue has not been found";
                }
                if(string.Equals(baubleItem,"Wrong"))
                {
                    bauble = false;
                    itemsPicked--;
                    baubleCorrect = "Bauble has not been found";
                }
                track = 2;
                d.SetDialogue("Well, you got a few of them right. Just keep the right ones and get back for the rest of them!\n" + towerCorrect + "\n" + statueCorrect + "\n" + baubleCorrect);
            }

        }
        else
        {
            track = 2;
            d.SetDialogue("Not enough items I see! Come back with one of each item so I can check them over.");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"Thank you kindly! It would have taken me ages to find the right items, so it was mighty helpful what you did for me just now! I'm sure you want to explore further into our lands don't you? Take this key and best of luck to you on your journey.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
