using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GreenMaiden : MonoBehaviour
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
    [SerializeField] public int hitPoints = 3;//use in tandem with projectile script
    int track = 0;
    [SerializeField] ProjectileLauncher launcher;
    [Header("Writing")]
    [SerializeField] string text = "A maiden sits weeping by a well. She appears distraught, with tears staining her face as she wails into the air. Such a loud noise is sure to draw in something more dangerous soon.";
    [SerializeField] string quest = "Comfort the maiden";//fetch quest or maybe fend off things coming towards you for a number of minutes(start over if they get by you)
    [SerializeField] string fight = "Tell the maiden to be quiet";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] string protection = "Broken Arrow";
    [SerializeField] GameObject brokenArrow;


    void Update()
    {
        if(hitPoints <= 0)
        {
            player.questFailed = true;
            player.Defeat();
        }
    }

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
                SetName("Green Maiden");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("Oh, hello. My apologies I was distraught over the hardships plauging my people. I find myself in quite the predicament as others aim to hunt me down for the sins and wrongdoings of my father. I fear they are close, lurking in the shadows with one last desperate attempt to try to slay me. WOuld you help protect me please?");
                }
                else if(track == 1)
                {
                    d.SetDialogue("Oh thank you! I fear they are closing in though, you may want to keep an eye out now!");
                }
                else if(track == 2)
                {
                    d.SetDialogue("Please, for both our sakes, focus on the task at hand!");
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
                accept.onClick.AddListener(Change);
                turnIn.onClick.AddListener(TurnIn);
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
        d.SetDialogue("Oh, hello. My apologies I was distraught over the hardships plauging my people. I find myself in quite the predicament as others aim to hunt me down for the sins and wrongdoings of my father. I fear they are close, lurking in the shadows with one last desperate attempt to try to slay me. Would you help protect me please?");
    }

    public void Change()
    {
        track = 1;
        d.SetDialogue("Oh thank you! I fear they are closing in though, you may want to keep an eye out now!");
        d.DeactivateDialogueBox();
        StartCoroutine(launcher.WaitRoutine());
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);
    }

    public void TurnIn()
    {
        if(launcher.destroyedProjectiles == launcher.maxProjectiles)
        {
            brokenArrow.SetActive(true);
        }
        foreach(string item in player.questItems)
        {
            if(string.Equals(protection,item))
            {
                track = 3;//JUST IN CASE
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
            d.SetDialogue("Please, for both our sakes, focus on the task at hand! Bring me something that will let me know this battle is over!");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"Thank you! I feared that would be the end of me and my kingdom would truly have been left with no hope for a better day. Thank you for all your help, I will never forget you.", creature.GetPercent(), creature.GetProgress());
        }
        
    }

}
