using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedMaiden : MonoBehaviour
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
    [SerializeField] int track = 4;
    [Header("Writing")]
    [SerializeField] string text = "The maiden before you stands resolute, both hands tight around the hilt of a sword. She stands firm and ready to fight though a tremble in her arm belies a hesitance.";
    [SerializeField] string quest = "Talk to her first";//she fears for the worst, sure that the only way to fix the colors is to fight for them(if most of your choices to this point are quests then she gives in, otherwise she fights anyways)
    [SerializeField] string fight = "Fight her";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    string questPrize = "Glimmering Sword";
    public string fightPrize = "Bloody Sword";
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
                SetName("Red Maiden");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("Ah, I see you have brought change to this world. Whether it be good or bad, who can truly tell? But, I will gladly attempt to give you my best measure of it.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Yes, I know what I must do, right now at least. Please, if you have anything elseto do, do it quickly before I decide my own final actions.");
                    //give one last chance for player to change outcome as pressing turn in will decide whether you fight the maiden or recieve just a prize
                }
                else if(track == 4)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(false);
                    d.SetDialogue("Who are you? I know not of your deeds or what you could possibly mean in this land, so how can I fairly judge you? Leave! A-and return when you have accomplished something of note!");
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
            d.SetDialogue("Who are you? I know not of your deeds or what you could possibly mean in this land, so how can I fairly judge you? Leave! A-and return when you have accomplished something of note!");
        }
        if(track == 0)
        {
            accept.gameObject.SetActive(true);
            turnIn.gameObject.SetActive(false);
            creature.choice.SetActive(false);
            startedQuest = true;
            Player.inQuest = true;
            d.SetName(creature.creatureName);
            d.SetDialogue("Ah, I see you have brought change to this world. Whether it be good or bad, who can truly tell? But, I will gladly attempt to give you my best measure of it.");
        }
        
    }

    public void Change()
    {
        track = 1;
        d.SetDialogue("Yes, I know what I must do, right now at least. Please, if you have anything elseto do, do it quickly before I decide my own final actions.");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"You are truly a fine person! I am thankful truly that this sword holds no real meaning now. Please, take it as a sign of my gratitude for your protection of our land. It may not be perfect, but I feel I can trust you enough now to make the right decisions.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
