using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GreenUnique : MonoBehaviour
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
    [SerializeField] string text = "A large stag blocks the way forward. It looks at you with intelligent eyes, pawing at the ground with one massive hoof as you approach. It seems on edge as it shifts one way and then another.";
    [SerializeField] string quest = "Look around to see what is unsettling such a large creature";//actually, to make it easier, we'll just do a bunch of green fires you have to put out in a certain amount of time
    //do that with the block button to stick with the original idea for this quest; fending off wolves
    //press b as you approach to put it out, adds a tick to number of fires put out, then move onto next one
    //generate the fires in a certain space around the deer, and have timer for how long to do so or whatever
    [SerializeField] string fight = "Charge forward and try to slip by the stag";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] GameObject firePrefab;
    [SerializeField] int maxFires = 10;
    public int putOutFires = 0;

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
                SetName("Green Deer");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("The deer paws at the ground, snorting and looking around wildly at the fires popping up all around. It appears wary of moving as the flames flicker about. You may want to put on some protection before putting out those flames.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Now with you moving around, it seems more at ease with you now taking up the task of protecting the surrounding area from more damage. Be sure to protect yourself before trying to put those flames out otherwise you won't have a chance to snuff them out.");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("The deer snorts loudly as you approach. Seems there are still fires to put out. Press B followed by P to block if all else fails."); 
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
        d.SetDialogue("The deer paws at the ground, snorting and looking around wildly at the fires popping up all around. It appears wary of moving as the flames flicker about. You may want to put on some protection before putting out those flames.");
        
    }

    public void Change()
    {
        track = 1;
        d.DeactivateDialogueBox();
        d.SetDialogue("Now with you moving around, it seems more at ease with you now taking up the task of protecting the surrounding area from more damage. Be sure to protect yourself before trying to put those flames out otherwise you won't have a chance to snuff them out.");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);
        int i = 0;
        while(i < maxFires)
        {
            GameObject fire = Instantiate(firePrefab, new Vector3(Random.Range(transform.position.x-10, transform.position.x+10),Random.Range(transform.position.y-10, transform.position.y+5),transform.position.z), transform.rotation);
            i++;
        }

    }

    public void TurnIn()
    {
        if(putOutFires == maxFires)
        {
            track = 3;//JUST IN CASE
            Rewards();
            player.RegisterItem("Green Antler");
            questGiver.SetActive(false);
            d.DeactivateDialogueBox();
            accept.onClick.RemoveListener(Change);
            turnIn.onClick.RemoveListener(TurnIn); 
        }
        if(track != 3)
        {
            track = 2;
            d.SetDialogue("The deer snorts loudly as you approach. Seems there are still fires to put out. Press B followed by P to block if all else fails.");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"With a gentle shake of its head, the deer trots up to you and carefully bows its head before you. It seems to want to give you a loose antler from its head. With that given, the deer turns tail and bounds off into the surrounding greenery.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
