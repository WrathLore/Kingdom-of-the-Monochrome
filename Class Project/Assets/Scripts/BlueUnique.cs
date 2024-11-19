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
    [SerializeField] string snakeFood = "Rat";
    [SerializeField] GameObject ratPrefab;

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
                    accept.gameObject.SetActive(true);
                    turnIn.gameObject.SetActive(false);
                    d.SetDialogue("The large snake glances at you for a moment before its tongue flickers out once more and its large head swings in the direction of a rat scurrying by.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Numerous rats are scurrying by, better catch as many as you can see. The snake seems uninterested in you at the moment.");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("The snake hisses in disapproval. Seems you have no yet collected enough rats.");
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
        d.SetDialogue("The large snake glances at you for a moment before its tongue flickers out once more and its large head swings in the direction of a rat scurrying by.");
    }

    public void Change()
    {
        track = 1;
        d.DeactivateDialogueBox();
        int i = 0;
        while(i < 3)
        {
            GameObject rat = Instantiate(ratPrefab, new Vector3(Random.Range(transform.position.x-5, transform.position.x+5),Random.Range(transform.position.y-5, transform.position.y+5),transform.position.z), transform.rotation);
            i++;
        }

        d.SetDialogue("Numerous rats are scurrying by, better catch as many as you can see. The snake seems uninterested in you at the moment.");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);

    }

    public void TurnIn()
    {
        int i = 0;
        foreach(string item in player.questItems)
        {
            if(string.Equals(snakeFood,item))
            {
                i++;
            }
            
        }
        if(i == 3)
        {
            track = 3;//JUST IN CASE
            Rewards();
            player.RegisterItem("Blue Scale");
            questGiver.SetActive(false);
            d.DeactivateDialogueBox();
            accept.onClick.RemoveListener(Change);
            turnIn.onClick.RemoveListener(TurnIn); 
        }
        if(track != 3) //if i != 3
        {
            track = 2;
            i = 0;
            d.SetDialogue("The snake hisses in disapproval. Seems you have no yet collected enough rats.");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"The snake blinks as it finishes eating the rats. It nudges against you as it slithers by, leaving behind a few glittering scales in its wake. It finally seems satisfied.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
