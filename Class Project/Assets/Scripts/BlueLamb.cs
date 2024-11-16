using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlueLamb : MonoBehaviour
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
    [SerializeField] string text = "A cleric looks off to the side, concern raidiating from their form. They stand before a burned bridge, the only way across the chasm, still smoldering softly. Their hands glow with a similar light to the embers. They do not appear to be willing to move or fix the bridge.";
    [SerializeField] string quest = "Enquire about their concern";
    [SerializeField] string fight = "Force them to fix the bridge";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] GameObject puzzleBase;
    public string puzzle = "Fixed Bridge";
    public string junk = "Unfixed Bridge";
    [SerializeField] PuzzleScript puzzleScript;
    [SerializeField] public GameObject water;
    [SerializeField] GameObject bridge;
    [SerializeField] public GameObject destroyedBridge;


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
                SetName("Blue Lamb");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("Oh hello. Sorry for this awful scene in front of you and I'm sorry to ask this, but would you be willing to help? I'm afraid that much has been going wrong around here. Though, I'm sure you could tell from the bramble patch my friend has been dealing with. I wonder if they ever found that locket I gave them.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Thank you, your help is much appreciated. Would you mind helping me put together a new bridge? I'm afraid that there is not much I can do by myself with these problems. It needs a keener eye and a more delicate touch than one I can lend.");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("Perfect, now please, finish as quickly as you can please.");  
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
        d.SetDialogue("Oh hello. Sorry for this awful scene in front of you. I'm afraid that much has been going wrong around here. Though, I'm sure you could tell from the bramble patch my friend has been dealing with. I wonder if they ever found that locket I gave them.");
        
    }

    public void Change()
    {
        //for this one, it'll be a puzzle, put together the bridge once more
        //will block off the land into 2 until you either do the puzzle or fight them
        //if fight them, then debris will fill the hole and can continue that way
        //otherwise will be an obstacle
        track = 1;
        puzzleBase.SetActive(true);
        puzzleScript.StartGame();
        d.DeactivateDialogueBox();
        d.SetDialogue("Thank you, your help is much appreciated. Would you mind helping me put together a new bridge? I'm afraid that there is not much I can do by myself with these problems. It needs a keener eye and a more delicate touch than one I can lend.");
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
                bridge.SetActive(true);
                Destroy(water);
                d.DeactivateDialogueBox();
                accept.onClick.RemoveListener(Change);
                turnIn.onClick.RemoveListener(TurnIn); 
                break;//need to break from the loop here  
            }
            
        }
        if(track != 3)
        {
            track = 2;
            d.SetDialogue("Perfect, now please, finish as quickly as you can please.");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"Please, allow me to place the bridge. I would hate for you to leave empty handed, so please, take this as a small token of my gratitude. Safe travels!", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
