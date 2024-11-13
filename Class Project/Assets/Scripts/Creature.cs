using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Creature : MonoBehaviour
{//this will contain all the stats about the characters and will be used to tie into the fight script
//quest scripts will likely be individualized, so separate scripts for each one
    [Header("Background Information")]
    [SerializeField] Player player;
    [SerializeField] FightScript fightScript;
    [SerializeField] GameObject creature; //set equal to the specific creature object
    [SerializeField] bool inTutorialFight = false;
    public bool inTutorialQuest = false;
    public bool interactedWith = false;
    public string creatureName;
    public bool isDead = false;
    public bool dodged = false;
    public GameObject choice;
    [SerializeField] GameObject questButton;
    [SerializeField] GameObject fightButton;
    public static string actionTaken = "Waiting for the first move..."; //keep track of action taken in fight
    [Header("Items")]
    [SerializeField] public int healthPotions = 1;
    [SerializeField] public int green;
    [SerializeField] public int blue;
    [SerializeField] public int red;
    [SerializeField] public WeaponScript weapon;//use for details of the weapon
    public static bool isBlocking = false;
    [Header("Stats")]
    [SerializeField] public float healthPoints = 25;
    [SerializeField] public float strength = 10;
    float damageTaken = 0;
    [Header("Percentages")]//use in ActionTaken for how likely each action is to be taken
    [SerializeField] public float attackPercent = 0.5f;
    [SerializeField] public float defendPercent = 0.75f;
    [SerializeField] public float healPercent = 1f;
    [SerializeField] public float dodgePercent = 0.01f;
    public float randomNum;//random float between 0 and 1 
    public int randomInt; //random int between 1 and 5(once I figure out how to do random int)
    public float actionNum;//use for how much damage is blocked or given
    [Header("Earnings")]
    [SerializeField] TextMeshProUGUI redEarned;
    [SerializeField] TextMeshProUGUI greenEarned;
    [SerializeField] TextMeshProUGUI blueEarned;
    [SerializeField] TextMeshProUGUI potionsEarned;
    public float progressPercent;
    public int p;

    void Awake()
    {
        SetPercent();
    }

    public void SetPercent()
    {
        if(player.inTutorial)//only need for tutorial
        {
            progressPercent = 0.5f;
            p = 50;
        }
        else
        {
            progressPercent = 0.2f;
            p = 20;
        }
    }

    public float GetPercent()
    {
        return progressPercent;
    }
    public int GetProgress()
    {
        return p;
    }


    //UNIQUE QUEST SCRIPT SETS THE DIALOGUE AND SUCH
    //THIS ONE ACTIVATES IT
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.CompareTag("Player"))
        {
            Player.onCharacter = true;
            FightScript.RegisterOpponent(this);//set opponent to correct spot
            if(!interactedWith && fightScript != null)
            {
                if(inTutorialFight)
                {
                    choice.SetActive(true);
                    questButton.SetActive(false);
                    fightButton.SetActive(true);
                    fightScript.setValues();
                }
                else if(inTutorialQuest)
                {
                    choice.SetActive(true);
                    questButton.SetActive(true);
                    fightButton.SetActive(false);
                }
                else
                {
                    choice.SetActive(true);
                    fightScript.setValues();
                    fightButton.SetActive(true);
                    questButton.SetActive(true);
                }
            }
            else
            {
                choice.SetActive(false);//turn off the choice option if interacted with
            }
            
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.CompareTag("Player"))
        {
            Player.onCharacter = false;
            if(!interactedWith && fightScript != null)
            {
                if(choice != null)
                {
                    choice.SetActive(false);
                }
            }
            
        }
    }

    public void SetDeathStatus()
    {
        if(healthPoints <= 0)
        {
            healthPoints = 0;
            isDead = true;
            actionTaken = "Waiting for the first move...";
        }
    }

    public bool GetDeathStatus()
    {
        return isDead;
    }

    public int DropPotions()
    {
        return 1; //work on this, return between 1 to 5 health potions when called
    }

    public int GetRed()
    {
        return red;
    }

    public int GetGreen()
    {
        return green;
    }

    public int GetBlue()
    {
        return blue;
    }

    public void UsePotion()
    {
        if(healthPotions != 0)
        {
            healthPotions--;
            healthPoints += 10; //just add on 10 points to creature health, no upper limit for now  
        }
    }

    public int Block()
    {
        return weapon.block;//return amount of damage to be blocked
    }

    public void SetIsBlocking()
    {
        if(!isBlocking)
        {
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }
    }

    public float Attack()
    {
        if(strength >= weapon.damage)
        {
            return strength;
        }
        else
        {
            return weapon.damage + (strength/4);
        }
    }
    //randomish chance for attack block usepotion
    //maybe like 75% attack 20% block 5% usepotion(may increase if health dips below certain amount)

    public void DamageTaken(float damage)//damage will be from player
    {
        float x = Random.Range(0f,1f);
        if(x <= dodgePercent)
        {
            dodged = true;
            Player.actionTaken = Player.creatureName + " has attacked for " + damage + " but " + creatureName + " has dodged.";
            return;//take no damage if blocked
        }
        if(isBlocking)
        {
            damage -= Block();
            damageTaken = damage;
            if(damage > 0)
            {
                healthPoints -= damage;
                SetDeathStatus();
            }
            SetIsBlocking();//reset isBlocking back to false
        }
        else
        {
            damageTaken = damage;
            healthPoints -= damage;
            SetDeathStatus();
        }
    }

    public void Defeat()
    {
        if(GetDeathStatus())//only continue if creature is dead
        {
            //on defeat, show victory screen(press away with c)
            if(player != null && creature != null)
            {
                int p = DropPotions();//need it to be the same for two things and DropPotions should return a random number
                if(inTutorialFight)
                {
                    player.Victory(GetRed(),GetGreen(),GetBlue(),p, GetPercent(), GetProgress(), true);
                }
                else
                {
                    if(string.Equals(creatureName, "Green Fairy"))
                    {
                        player.Victory(GetRed(),GetGreen(),GetBlue(),p, GetPercent(), GetProgress(), false, creature.GetComponent<GreenFairy>().sword);
                    }
                    else if(string.Equals(creatureName, "Green Devout"))
                    {
                        player.Victory(GetRed(),GetGreen(),GetBlue(),p, GetPercent(), GetProgress(), false, creature.GetComponent<GreenDevout>().key);
                    }
                    else
                    {
                        player.Victory(GetRed(),GetGreen(),GetBlue(),p, GetPercent(), GetProgress());
                    }
                }
                //deactivate the creature as they are functionally dead now
                creature.SetActive(false);
                redEarned.text = GetRed().ToString();
                greenEarned.text = GetGreen().ToString();
                blueEarned.text = GetBlue().ToString();
                potionsEarned.text = p.ToString();
            }
        }
    }

    public void ActionTaken()
    {
            //use this in response to action by player
            //probably use IEnumerator or something, but keep it simple for now
            //so want to have it show the text on screen for what action it took
            //wait maybe like 5 seconds, and then have the default text show up again
            //have this happen in response to button being pressed by player on fight screen
        
            //so what would probably be easiest is to have random float between 0 and 1
            //if number is 0 to 0.75 inclusive, attack if number is 0.75 exclusive to 0.95 inclusive, defend, and if number is 0.95 exclusive to 1, heal
            randomNum = Random.Range(0f,1f);
            if(randomNum <= attackPercent)
            {
                actionNum = Attack();
                player.DamageTaken(actionNum);
                if(!player.dodged)
                {
                    actionTaken = creatureName + " has attacked for " + actionNum + " points";
                }
                else
                {
                    actionTaken = creatureName + " has attacked for " + actionNum + " points, but " + Player.creatureName + " has dodged the attack!";
                    player.dodged = false;
                }
                
                if(player.isAttacking)
                {
                    DamageTaken(player.Attack());
                    if(!dodged)
                    {
                        Player.actionTaken = Player.creatureName + " has attacked for " + damageTaken + " points"; 
                    }
                    else
                    {
                        dodged = false;
                    }
                    player.SetIsAttacking();
                    if(GetDeathStatus())
                    {
                        Defeat();
                    }
                }
                player.Defeat(); //call at end of attack to see if player is dead or not
            }
            else if(randomNum > attackPercent && randomNum <= defendPercent)
            {
                actionNum = Block();
                SetIsBlocking();
                actionTaken = creatureName + " has blocked " + actionNum + " points of your attack";
                if(player.isAttacking)
                {
                    DamageTaken(player.Attack());//only apply damage if attacking
                    if(!dodged)
                    {
                        Player.actionTaken = Player.creatureName + " has attacked for " + damageTaken + " points"; 
                    }
                    else
                    {
                        dodged = false;
                    }
                    player.SetIsAttacking();
                    if(GetDeathStatus())
                    {
                        Defeat();
                    }
                }
            }
            else
            {
                UsePotion();
                actionTaken = creatureName + " has been healed for 10 health points";
                if(player.isAttacking)
                {
                    DamageTaken(player.Attack());
                    if(!dodged)
                    {
                        Player.actionTaken = Player.creatureName + " has attacked for " + damageTaken + " points"; 
                    }
                    else
                    {
                        dodged = false;
                    }
                    player.SetIsAttacking();//reset isAttacking for the next round
                    if(GetDeathStatus())
                    {
                        Defeat();
                    }
                }
            }
    }


}
