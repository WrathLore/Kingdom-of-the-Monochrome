using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] WelcomeScript welcome;
    [SerializeField] RulesScript rules;
    [SerializeField] Dialogue dialogue;
    [SerializeField] GameObject itemBox;
    [SerializeField] TextMeshProUGUI itemText;
    public static bool inFight = false;
    [SerializeField] int blockTime = 5;
    [SerializeField] PlayerAnimationStateChanger a;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject finishGameCircle;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.E) && finishGameCircle != null)
        {
            if(finishGameCircle.GetComponent<FinishGame>().onCircle)
            {
                //reset all progress and head back to main menu
            }

        }
        PanelUpdate();
        ItemUpdate();
        if(!Player.questDodge)//don't want to block for red unique quest
        {
            Block();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.changeScene();
        }

        if(Input.GetKey(KeyCode.O))
        {
            optionsPanel.GetComponent<Canvas>().enabled = true;
        }

        if(!inFight)
        {
            MoveUpdate();
        }

        //pickup option for P in the PickUp script

        if(Input.GetKeyDown(KeyCode.T) && Player.onCharacter)
        {//needs keydown because get key will update too many times and flicker the dialogue box on and off
            if(Player.inQuest && dialogue != null)
            {
                DialogueActivator();
            }
            
        }

        
    }

    public void Block()
    {
        if(Input.GetKeyDown(KeyCode.B) && player != null)
        {
            player.blocked = true;
        }

        if(Input.GetKeyUp(KeyCode.B) && player != null)
        {
            player.blocked = false;
        }
        StartCoroutine(player.BlockRoutine(blockTime));
    }

    public void ItemUpdate()
    {
        if(Input.GetKey(KeyCode.I) && itemBox != null)
        {
            itemText.text = "";//just set it to nothing for the moment
            foreach(string item in player.questItems)
            {
                itemText.text += item + "\n";
            }
            itemBox.SetActive(true);
        }
    }

    public void PanelUpdate()
    {
         if(welcome != null && !WelcomeScript.called)
        {
            if(Input.GetKey(KeyCode.C))
            {
                welcome.panel.SetActive(false);
                welcome.tutorial.SetActive(false);
                welcome.start.SetActive(false);
                welcome.stats.SetActive(true);
                WelcomeScript.called = true;
            }
        }
        if(Input.GetKey(KeyCode.R))
        {
            rules.RulesOpen();
        }
        if(Input.GetKey(KeyCode.C))//covers the closing of all the panels
        {
            rules.RulesClose();
            player.victoryPanel.SetActive(false);
            player.victoryQuest.SetActive(false);
            itemBox.SetActive(false);
            optionsPanel.GetComponent<Canvas>().enabled = false;
        }
    }

    public void MoveUpdate()
    {
         Vector3 movement = Vector3.zero;
        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !Player.questDodge){ 
            movement += new Vector3(0,1,0);//up
            a.ChangeAnimationState("Back");
        }
        if((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !Player.questDodge){
            movement += new Vector3(0,-1,0);//down
            a.ChangeAnimationState("Forward");
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            movement += new Vector3(-1,0,0);//left
            a.ChangeAnimationState("Left");
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            movement += new Vector3(1,0,0);//right
            a.ChangeAnimationState("Right");
        }
        if(movement == Vector3.zero)//if not moving then just idle animation
        {
            a.ChangeAnimationState("Idle");
        }
        
        player.Move(movement);
    }

    public void DialogueActivator()//activate or deactivate the dialogue box
    {
        if(!Dialogue.activated)
        {
            dialogue.ActivateDialogueBox();
        }
        else
        {
            dialogue.DeactivateDialogueBox();
        }
    }
}
