using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.PackageManager;

public class FightScript : MonoBehaviour
{
    //this should call up the fight screen(will need to work on that bit)
    //and get the stats for the specific character being fought as well as the player
    [Header("Fight Panel Items")]//set these ones yourself
    [SerializeField]  TextMeshProUGUI nameP;
    [SerializeField]  TextMeshProUGUI healthP;
    [SerializeField]  TextMeshProUGUI strengthP;
    [SerializeField]  TextMeshProUGUI dodgeP;
    [SerializeField]  TextMeshProUGUI potionsP;
    [SerializeField]  TextMeshProUGUI redP;
    [SerializeField]  TextMeshProUGUI greenP;
    [SerializeField]  TextMeshProUGUI blueP;
    [SerializeField]  TextMeshProUGUI weaponP;
    [SerializeField]  TextMeshProUGUI weaponDamageP;
    [SerializeField]  TextMeshProUGUI shieldP;
    [SerializeField]  TextMeshProUGUI shieldBlockP;
    [SerializeField]  TextMeshProUGUI actionP;
    [SerializeField]  TextMeshProUGUI nameO;
    [SerializeField]  TextMeshProUGUI healthO;
    [SerializeField]  TextMeshProUGUI strengthO;
    [SerializeField]  TextMeshProUGUI dodgeO;
    [SerializeField]  TextMeshProUGUI potionsO;
    [SerializeField]  TextMeshProUGUI redO;
    [SerializeField]  TextMeshProUGUI greenO;
    [SerializeField]  TextMeshProUGUI blueO;
    [SerializeField]  TextMeshProUGUI weaponO;
    [SerializeField]  TextMeshProUGUI weaponDamageO;
    [SerializeField]  TextMeshProUGUI shieldO;
    [SerializeField]  TextMeshProUGUI shieldBlockO;
    [SerializeField]  TextMeshProUGUI actionO;//keep track of what action the opponent is taking and display it on the screen
    
    //player will be easiest bit, but npc/character may need some work
    [Header("Opponents")]
    [SerializeField] Player player;
    [SerializeField] public static Creature opponent;//keep all possible opponents on here
    //WORK IN PROGRESS

    void FixedUpdate()
    {//MAY DO FIXED UPDATE BECAUSE TURN BASED COMBAT(SORT OF)
        //UPDATE THE TEXT WHENEVER PLAYER TURN IS TAKEN(ie whenever a button is pressed on fightpanel)
        setValues();
    }

    public static void RegisterOpponent(Creature c)
    {
        opponent = c;
    }

    public void setValues()
    {
        if(player != null && opponent != null)
        {
            //set the values of the text in the fightscreen panel
            //only a single fightscript, so it will have everything needed
            //just need to find the correct oppoenent from the Creature list to effect it
            //STATS
            healthP.text = Player.currentHealthPoints.ToString();
            strengthP.text = Player.strength.ToString();
            dodgeP.text = (Player.dodgePercent * 100).ToString() + "%";
            healthO.text = opponent.healthPoints.ToString();
            strengthO.text = opponent.strength.ToString();
            dodgeO.text = (opponent.dodgePercent * 100).ToString() + "%";
            //NAMES
            nameP.text = Player.creatureName;
            nameO.text = opponent.creatureName;

            //ITEMS
            potionsP.text = Player.healthPotions.ToString();
            redP.text = Player.red.ToString();
            greenP.text = Player.green.ToString();
            blueP.text = Player.blue.ToString();
            weaponP.text = player.weapon.weaponName;
            weaponDamageP.text = player.weapon.damage.ToString();
            shieldP.text = player.weapon.shieldName;
            shieldBlockP.text = player.weapon.block.ToString();
            actionP.text = Player.actionTaken;
            potionsO.text = opponent.healthPotions.ToString();
            redO.text = opponent.red.ToString();
            greenO.text = opponent.green.ToString();
            blueO.text = opponent.blue.ToString();
            weaponO.text = opponent.weapon.weaponName;
            weaponDamageO.text = opponent.weapon.damage.ToString();
            shieldO.text = opponent.weapon.shieldName;
            shieldBlockO.text = opponent.weapon.block.ToString();
            actionO.text = Creature.actionTaken;

        }

        //above will keep the visual stats updated, update the actual stats through the ButtonScript

    }

    
}
