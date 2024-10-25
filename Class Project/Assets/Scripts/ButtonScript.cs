using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject rulesPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject fightPanel;
    //will be same class for all buttons
    //each button has a different tag just in case, but not likely to need that

    //for the main menu, however, whatever ends up being put in for options and rules may still apply later
    public void OptionsButton()
    {
        //will call up the options menu
        if(optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }

    }

    public void CloseOptionsButton()
    {
        if(optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void RulesButton()
    {
        //will call up the rules menu
        if(rulesPanel != null)
        {
            rulesPanel.SetActive(true);
        }

    }

    public void CloseRulesButton()
    {
        if(rulesPanel != null)
        {
            rulesPanel.SetActive(false);
        }
    }

    public void TutorialButton()
    {
        //will go to the tutorial scene
        SceneManager.LoadScene("TutorialLevel");
    }

    public void QuitButton()
    {
        //will quit the application
        Application.Quit(); //works once game is built
    }

    public void StartButton()
    {
        //will go to the mainhub scene
        SceneManager.LoadScene("GameHub");
    }

    public void FightButton()
    {
        //if pressed open up the fight panel 
        //this will lead to an individual's fight
        //use tags for this bit as well
        if(fightPanel != null)
        {
            fightPanel.SetActive(true);
            PlayerInputController.inFight = true;//cannot move player while in fight
        }
        //this will pull up the fight panel, needs more work to get all the stuff fully working, but opens up to the right thing at least

        //once panel is active, need to assign all the variables and then work from there
    }

    //FOR THE THREE BELOW, SET PLAYER ACTION AND THEN CALL THE CREATURE METHOD ACTION TAKEN TO APPLY CHANGES
    //right now the setup below seems to be working, may mess around with it more when have more time
    //but moving on to the death stats next
    public void HealButton()
    {
        //use a potion
        if(player != null && Player.healthPotions != 0)//nothing done if no health potions when clicking heal button
        {
            player.UsePotion();
            //use FightScript.opponent to decide what to do next
            FightScript.opponent.ActionTaken();
        }
    }

    public void AttackButton()
    {
        //attack creature(use weapon)
        if(player != null)
        {
           //use player.Attack() and the FightScript.opponent to decide what to do next
           player.SetIsAttacking();
           FightScript.opponent.ActionTaken();
        }
    }

    public void DefendButton()
    {
        //defend(use shield)
        if(player != null)
        {
            Player.actionTaken = Player.creatureName + " has blocked for " + player.weapon.block + " points.";
            //use player.Block() and the FightScript.creature to decide what to do next
            player.SetIsBlocking();
            //if player is blocking then wait for creature move
            //call the DamageTaken method if creature attacks otherwise nothing happens to player
            FightScript.opponent.ActionTaken();
        }
    }
}
