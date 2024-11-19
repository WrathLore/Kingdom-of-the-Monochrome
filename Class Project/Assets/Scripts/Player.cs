using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.PackageManager;
//FOR FIGHT VS QUEST; maybe something like undertale where it automatically goes into the page for the fight
//and in that case you can choose to either fight or to try to talk to them
//and if you choose to talk, then you automatically go down the quest path and it exits the screen
public class Player : MonoBehaviour
{
    [Header("Objects To Change")]
    Rigidbody2D pb;
    public string scene; //has to be public to access it
    public GameObject victoryPanel;//set to true on victory
    public GameObject fightPanel;//set to false on victory
    public GameObject victoryQuest;//set true on victory for quest
    [SerializeField] TextMeshProUGUI victoryText;
    [SerializeField] TextMeshProUGUI itemAcquired;//use for items gained through fighting
    [SerializeField] GameObject shieldBubble;
    
    [Header("Stats")]//alot of this has been changed to static to keep track between scenes
    public static float currentHealthPoints = 40;
    public static float maxHealthPoints = 40;
    public static float strength = 25;
    public static float dodgePercent = 0.1f;//start at 10% chance to dodge for player, can go up
    //dodge will be only thing affected by tutorial, get extra 3%
    //so max dodge by end game (including tutorial) should be 58%

    [Header("Movement")]
    [SerializeField] float speed = 10.0f;
    //NEED TO INCLUDE F CHARACTER AT END OF FLOAT NUMBER IF DECIMAL POINT

    //NAMING
    public static string creatureName = "Player";
    //string is lowercase in c# unlike with java
    //static because going to be same no matter what

    [Header("Information")]
    [SerializeField] bool isDead = false;
    public bool questFailed = false;
    public bool dodged = false;
    int dodgeCount = 0;//use to track number of times dodge increased
    public bool inTutorial = false;
    public static string actionTaken = "Waiting for input...";
    public int hit = 0;//reset at end of projectiles being launched
    public bool blocked = false;
    //c# does have bool values unlike with c
    [Header("Items")] //may or may not do static for these, may just have them show up starting as this in each area
    public static int healthPotions = 2;
    public static int maxHealthPotions = 2;
    public static int green = 0;
    public static int blue = 0;
    public static int red = 0;
    [SerializeField] public List<string> questItems; //can do a dropdown menu probably to decide which one to do
    //[SerializeField] List<WeaponScript> weaponsList;//the stats of the weapons
    //ABOVE MAY GO BACK TO, sticking with one weapon for easier time programming for now
    //could possibly do a list of fight items as well, but that would be more for flavor than anything else
    //at least right now, final progress depends on counter of quests and fights completed
    //would also have to change the list to public static instead of just public to keep progress on list between scenes
    [SerializeField] public WeaponScript weapon;

    //internal trackers below
    [Header("Internal Trackers")]
    public float progressTracker = 0;//1 is 100%
    public int progress = 0;//will be used for the text part of the progress bar
    //probably wont make this static since it is for each area
    static bool tutorialQuest = false;
    static bool tutorialFight = false;
    //use the above two for the tutorial specifically; ie don't let player progress tutorial until these are true
    public static int killed = 0;
    public static int quest = 0;//two ints to track number killed and number quested
    [SerializeField] int questInLevel = 0;
    [SerializeField] int fightInLevel = 0;//use to keep track of the number quested and killed in level for reset purposes
    public static int finalTotal = 15;//5 in each area(right now, so keep track here)
    public static bool isBlocking = false;//decide whether or not to block
    public bool isAttacking = false;
    public static bool inQuest = false;
    public static bool onCharacter = false;
    public static bool onCircle = false; //use for red unique, to know if on circle or not
    public static bool questDodge = false; //use for red unique, so can only go back and forth while true
    [Header("Main Hub Trackers")]
    public static bool isGreen = false;
    public static bool isRed = false;
    public static bool isBlue = false;
    [Header("Earnings")]
    [SerializeField] TextMeshProUGUI redEarned;
    [SerializeField] TextMeshProUGUI greenEarned;
    [SerializeField] TextMeshProUGUI blueEarned;
    //the three above will be true once the respective areas are finished
    //once all three areas are completed, the final door will be done
    //can also use these to decide which version of the final door to show
    //as well as which version of the base doors to show
    //MIGHT NEED TO MESS AROUND WITH LATER THOUGH
    //static variables for things that have to carry over between scenes
    //may need to mess around with the killed and quest static though, cause they should reset if you die
    //but for now this works well enough
    

    void Awake()
    {
        pb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector3 movement)
    {
       pb.velocity = movement * speed;
    }

    public void setScene(string scene)
    {
        this.scene = scene;
    }

    public void RegisterItem(string item)
    {
        questItems.Add(item);
    }

    public void IncreaseProgress(float p, int pr)
    {
        progressTracker += p;
        progress += pr;
    }

    public void IncreaseHits()
    {
        hit++;
        if(hit > 3)
        {
            questFailed = true;
            Defeat();
        }
    }

    public void changeScene()
    {
        //will rely on being on a door
        //will be when pressing space
        //only change scene if on door(ie OnCollisionEnter2D)
        //and if door object has not been entered already, ie entered = false
    
        //would do comparetag, but that relies on the thing being the first object in the tag, so probably won't work for the gamehub
        //may work for the levels though, since they'll only have one door each

        if(!string.IsNullOrEmpty(scene))
        {
            if(progress == 100 && string.Equals(scene,"GameHub"))
            {
                LevelFinished();
            }
            //then change scene
            SceneManager.LoadScene(scene);
        }
    }

    public static bool TutorialDone()
    {
        if(!tutorialFight && !tutorialQuest)
        {
            return false;
        }
        if(red != 255)
        {
            red = 0;//using red lamb for tutorial, so just reset red for this
        }
        return true;
    }

    public static bool FirstEnter()
    {
        if(!isBlue && !isRed && !isGreen)
        {
            return true;
        }
        return false;
    }

    public void SetDeathStatus()
    {
        if(currentHealthPoints <= 0)
        {
            isDead = true;
        }
    }

    public bool GetDeathStatus()
    {
        return isDead;
    }

    public void UsePotion()
    {
        if(currentHealthPoints < maxHealthPoints)
        {
            if(healthPotions != 0)
            {
                healthPotions--;
                if(currentHealthPoints+10 <= maxHealthPoints)
                {   
                    actionTaken = creatureName + " has healed for 10 health points.";
                    currentHealthPoints += 10;//health potions will restore health
                }
                else
                {
                    actionTaken = creatureName + " has healed to max health points.";
                    currentHealthPoints = maxHealthPoints;//restore up to maxHealthPoints, not beyond that
                }   
            }
        }
    }

    public void IncreaseMaxHealth()
    {
        maxHealthPoints += 20*fightInLevel; //give 20 more points of health per enemy defeated per level finished
    }

    public void IncreaseStrength()
    {
        strength += 2*fightInLevel; //increase strength by 2 per enemy defeated per level finished
    }

    public void IncreaseDodge()
    {
        dodgePercent += 0.03f;//dodge goes up by 3% for each victory
        dodgeCount++;
    }

    public void QuestFinished()//keep track of quests
    {
        quest++;
        questInLevel++;
    }

    public void FightFinished()//keep track of fights
    {
        killed++;
        fightInLevel++;
    }

    public int Total()//keep track of total quests and fights finished
    {
        return quest+killed;
    }

    public int Block()
    {
        //will take into regards block of shield
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

    public void SetIsAttacking()
    {
        if(!isAttacking)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    public float Attack()
    {
        //attack will take into regards strength and damage of weapon
        if(strength > weapon.damage)
        {
            return strength;
        }
        else if(strength < weapon.damage)
        {
            return weapon.damage + (strength/4);
        }
        else
        {
            //strength == weapon.damage
            return weapon.damage+strength;//will be hidden mechanic basically
            //if damage equals strength then they are both used for attack
        }
        //return amount of damage to be dealt
    }
    //BLOCK ATTACK and USEPOTION will be tied to the buttons on the fightscript
    //and in response the Creature script will either attack block or usepotion in return

    public void DamageTaken(float damage)//damage will be from creature
    {
        float x = UnityEngine.Random.Range(0f,1f);
        if(x <= dodgePercent)
        {
            dodged = true; 
            return;//no damage taken
        }
        if(isBlocking)
        {
            damage -= Block();
            if(damage > 0)
            {
                currentHealthPoints -= damage;
                SetDeathStatus();
            }
            SetIsBlocking();//reset isBlocking back to false
        }
        else
        {
            currentHealthPoints -= damage;
            SetDeathStatus();
        }
        
    }

    public void PickUpPotions(int extra)
    {
        healthPotions += extra;
        maxHealthPotions += extra;
    }

    public void IncreaseColor(int r, int g, int b)
    {
        red += r;
        green += g;
        blue += b;
    }

    public void SetColor()
    {
        if(red == 255 && !isRed)
        {
            isRed = true;
        }
        if(green == 255 && !isGreen)
        {
            isGreen = true;
        }
        if(blue == 255 && !isBlue)
        {
            isBlue = true;
        }
    }

    public void Victory(int red, int green, int blue, int potions, float prog, int p, bool tutorial = false, string itemGained = "")
    {
        actionTaken = "Waiting for input...";
        onCharacter = false;
        IncreaseProgress(prog, p);
        tutorialFight = tutorial;
        IncreaseDodge();
        hit = 0;
        if(!tutorial)
        {
            IncreaseColor(red,green,blue);//gain the colors
            PickUpPotions(potions);//gain the potions
            FightFinished();//increase number fought by 1
        }
        currentHealthPoints = maxHealthPoints;//heal player once battle is over
        PlayerInputController.inFight = false;//can move player once out of fight
        if(fightPanel != null && victoryPanel != null)
        {
            fightPanel.SetActive(false);
            victoryPanel.SetActive(true);
            if(itemAcquired != null)
            {
                itemAcquired.text = "";
            }
            if(!string.IsNullOrEmpty(itemGained))
            {
                RegisterItem(itemGained);
                if(itemAcquired != null)
                {
                    itemAcquired.text = "Additionally, " + creatureName + " has acquired the item " + itemGained;
                }
            }
        }

    }

    public void QuestVictory(int red, int green, int blue, string v, float prog, int p, bool tutorial = false)
    {
        onCharacter = false;
        inQuest = false;//once quest is over, not in quest anymore
        IncreaseProgress(prog, p);
        tutorialQuest = tutorial;
        hit = 0;
        if(!tutorial)//only want to update this stuff if not tutorial cause otherwise pain for just 2 cases
        {
            IncreaseColor(red,green,blue);
            QuestFinished();
        }
        if(victoryPanel != null && victoryText != null && redEarned != null && greenEarned != null && blueEarned != null)
        {
            victoryText.text = v;
            redEarned.text = red.ToString();
            greenEarned.text = green.ToString();
            blueEarned.text = blue.ToString();
            victoryQuest.SetActive(true);
        }
    }

    public void Defeat()
    {
        if(isDead || questFailed)
        {
            currentHealthPoints = maxHealthPoints;

            questDodge = false;
            onCircle = false;
            questFailed = false;
            isDead = false;//just in case I'll set these to false as well
            hit = 0;
            PlayerInputController.inFight = false;//if died in fight, make this false so that you can move again once level restarts
            actionTaken = "Waiting for input...";
            Creature.actionTaken = "Waiting for first move...";
            if(dodgeCount > 0)
            {
                float lostDodge = dodgeCount * 0.03f;
                dodgePercent -= lostDodge;
                dodgeCount = 0;//just in case, reset dodgeCount
            }
            onCharacter = false;//just in case
            //reset the color progress for the level
            if(red != 0 && red != 255)
            {
                red = 0;
            }
            else if(blue != 0 && blue != 255)
            {
                blue = 0;
            }
            else if(green != 0 && green != 255)
            {//if on final level, none of the colors should reset, either that or if lose on final level, restart from scratch maybe
                green = 0;
            }
            quest -= questInLevel;
            killed -= fightInLevel;
            questItems.Clear();
            //would need to save the progress at the beginning of the scene and load that probably
            //show a game over screen(probs have scene start over)
            //either this or start over from scratch so jump to main menu and reset all the variables to their original states
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //if start from scratch
            //need to reset everything to what it was originally, might be easiest way to go about it though
        }
    }

    public void LevelFinished()
    {
        IncreaseMaxHealth();
        IncreaseStrength();
        SetColor();
        healthPotions = maxHealthPotions;
    }

    public void BlockProjectiles()
    {
        //use for the blocking sections
    }


    public IEnumerator BlockRoutine(int currentTime)
    {
        if(shieldBubble != null)
        {
            shieldBubble.SetActive(true);
            while(currentTime > 0 && blocked)
            {
                yield return new WaitForSeconds(1.0f);
                currentTime--;
            }

            //once it breaks out of the while loop either because time ran out or because the button stopped being pressed then turn off the shield
            blocked = false;
            shieldBubble.SetActive(false);
        }
    }
}
