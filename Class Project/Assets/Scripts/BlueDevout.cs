using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlueDevout : MonoBehaviour
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
    public int track = 4;
    [Header("Writing")]
    [SerializeField] string text = "An old devout sits before you. She looks at you suspicously, hands tightening on her cane as you step closer. What do you do?";
    [SerializeField] string quest = "Speak from a distance";//this one will be same set up as red maiden only in this case, can only get there after doing at least 2 quests
    [SerializeField] string fight = "Continue forward";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    string questPrize = "Worn Cane";
    public string fightPrize = "Broken Cane";
    [SerializeField] GameObject fightPanel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if((Player.killed != 0 || Player.quest != 0) && track == 4)
            {
                track = 0;
            }
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
                SetName("Blue Devout");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("I see you have accomplished something then. Come closer dearie and let me get a good look at you.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Ah. I see. Well, there is only one thing left to do now, isn't there?");
                }
                else if(track == 4)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(false);
                    d.SetDialogue("Hmph are you really trying to talk right now? Don't you have more important things to do than talk to an old crone such as I?");  
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
        if(track == 4)
        {
            accept.gameObject.SetActive(false);
            turnIn.gameObject.SetActive(false);
            creature.choice.SetActive(false);
            startedQuest = true;//don't want to pull up the original text boxes, want the dialogue instead
            Player.inQuest = true;
            creature.interactedWith = true;
            d.SetName(creature.creatureName);
            d.SetDialogue("Hmph are you really trying to talk right now? Don't you have more important things to do than talk to an old crone such as I?");
        }
        if(track == 0)
        {
            accept.gameObject.SetActive(true);
            turnIn.gameObject.SetActive(false);
            creature.choice.SetActive(false);
            startedQuest = true;
            Player.inQuest = true;
            d.SetName(creature.creatureName);
            d.SetDialogue("I see you have accomplished something then. Come closer dearie and let me get a good look at you.");
        }
        
    }

    public void Change()
    {
        track = 1;
        d.SetDialogue("Ah. I see. Well, there is only one thing left to do now, isn't there?");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);
    }

    public void TurnIn()
    {
         if(Player.killed > Player.quest)
        {
            //activate the fight script
            if(fightPanel != null)
            {
                d.DeactivateDialogueBox();
                accept.onClick.RemoveListener(Change);
                turnIn.onClick.RemoveListener(TurnIn);
                fightPanel.SetActive(true);
                PlayerInputController.inFight = true;//cannot move player while in fight
            }
        }
        else
        {
            //activate the rewards method
            Rewards();
            questGiver.SetActive(false);
            d.DeactivateDialogueBox();
            player.RegisterItem(questPrize);
            accept.onClick.RemoveListener(Change);
            turnIn.onClick.RemoveListener(TurnIn);
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"Hm, yes. I am sure you shall accomplish much more in the future. I wish you luck and give you this cane as my blessing. I hope you are able to accomplish much more in the future.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
