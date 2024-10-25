using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] WelcomeScript welcome;
    [SerializeField] RulesScript rules;
    [SerializeField] Dialogue dialogue;
    public static bool inFight = false;

    // Update is called once per frame
    void Update()
    {
        PanelUpdate();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.changeScene();
        }

        if(!inFight)
        {
            MoveUpdate();
        }

        //pickup option for P in the PickUp script

        if(Input.GetKeyDown(KeyCode.T) && Player.onCharacter)
        {//needs keydown because get key will update too many times and flicker the dialogue box on and off
            if(Player.inQuest)
            {
                DialogueActivator();
            }
           /* else
            {
                //activate the choice button if possible
                //WORK IN PROGRESS
                //FIGURE OUT AFTER FINISHING DIALOGUE PARTS
            }*/
            
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
        }
    }

    public void MoveUpdate()
    {
         Vector3 movement = Vector3.zero;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){ 
            movement += new Vector3(0,1,0);//up
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            movement += new Vector3(0,-1,0);//down
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            movement += new Vector3(-1,0,0);//left
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            movement += new Vector3(1,0,0);//right
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
