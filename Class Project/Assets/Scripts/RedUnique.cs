using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedUnique : MonoBehaviour
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
    [SerializeField] string text = "A winged entity blocks the way forward. A greatsword held with ease in one hand as they stand at the ready; wings splayed and muscles tensed. They stand in the middle of a small circle and it seems there is no way out without a fight.";
    [SerializeField] string quest = "Step into the circle";//dodge projectiles from the figure, maybe something with a time limit as well
    //might do what was originally planned for the blue maiden with the whirling whips. Blue maiden is now another puzzle instead to make it easier on the time limit.
    [SerializeField] string fight = "Attack from afar";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;
    [SerializeField] GameObject dodgeCircle; //if player on this and presses like X, then set Player.dodgeQuest = true and send off circle projectiles
    [SerializeField] GameObject groundObstacles;
    [SerializeField] ProjectileLauncher launcher;

    void Update()
    {
        if(Input.GetKey(KeyCode.X) && Player.onCircle)
        {
            //then lock in movement to only left and right and start launching projectiles
            dodgeCircle.SetActive(false);
            Player.onCircle = false;//make sure it doesn't happen again if accidentally pressed X again
            Player.questDodge = true;
             StartCoroutine(launcher.WaitRoutine());
        }

        if(launcher.destroyedProjectiles == launcher.maxProjectiles)
        {//once all projectiles have been destroyed, reset the movement
            Player.questDodge = false;
        }
    }

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
                SetName("Red Angel");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
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
                    d.SetDialogue("The angel stands there impassively, but it readies itself, its wings spreading wide as you approach. It seems to want you to move closer.");
                }
                else if(track == 1)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("The circle has fully formed behind you, and the angel seems to point you in that direction. It makes an X with its arms as it looks intently at the circle.");
                }
                else if(track == 2)
                {
                    accept.gameObject.SetActive(false);
                    turnIn.gameObject.SetActive(true);
                    d.SetDialogue("The angel looks at you in slight confusion and annoyance. Seems they want you to stand on the circle and ready yourself for a trial. (Press X once on the circle)");  
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
        d.SetDialogue("The angel stands there impassively, but it readies itself, its wings spreading wide as you approach. It seems to want you to move closer.");
        
    }

    public void Change()
    {
        track = 1;
        groundObstacles.SetActive(true);
        dodgeCircle.SetActive(true);
        d.DeactivateDialogueBox();
        d.SetDialogue("The circle has fully formed behind you, and the angel seems to point you in that direction. It makes an X with its arms as it looks intently at the circle.");
        accept.gameObject.SetActive(false);
        turnIn.gameObject.SetActive(true);
    }
    public void TurnIn()
    {
        if(launcher.destroyedProjectiles == launcher.maxProjectiles)
        {
            track = 3;
            Rewards();
            player.RegisterItem("Red Feather");
            questGiver.SetActive(false);
            groundObstacles.SetActive(false);
            d.DeactivateDialogueBox();
            accept.onClick.RemoveListener(Change);
            turnIn.onClick.RemoveListener(TurnIn); 
        }
        if(track != 3)
        {
            track = 2;
            d.SetDialogue("The angel looks at you in slight confusion and annoyance. Seems they want you to stand on the circle and ready yourself for a trial. (Press X once on the circle)");
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
             player.QuestVictory(creature.red, creature.green, creature.blue,"The angel hums in satisfation. It seems you have successfully passed its trial. It slowly approaches and presses its sword to your shoulder as it grants you a reward before taking off into the air, leaving behind only a single red feather.", creature.GetPercent(), creature.GetProgress());
        }
        
    }
}
