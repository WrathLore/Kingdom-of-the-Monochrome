using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialScript : MonoBehaviour
{
    //will be used only in the tutorial
    [SerializeField] GameObject tutorial;//panel for the tutorial
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] GameObject player;//don't need the player script, just the player object for the location
    [SerializeField] string location;//use like an enum, B is beginning, E is end, F is fight, and Q is quest

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            tutorial.SetActive(true);
            if(string.Equals(location,"B"))
            {
                Begin();
            }
            if(string.Equals(location,"F"))
            {
                Fight();
            }
            if(string.Equals(location,"Q"))
            {
                Quest();
            }
            if(string.Equals(location,"E"))
            {
                End();
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && tutorial != null)
        {
            tutorial.SetActive(false);
        }
    }
    public void Begin()
    {
        //text at the beginning of the level
        title.text = "Welcome to the Tutorial";
        info.text = "The tutorial is a straight room, introducing you to the actions you can take when interacting with different characters.\r\nTo begin with, movement is controlled through the arrow keys or, if you prefer, through WASD.\r\nThe goal of the tutorial is to guide you through the level and get you ready to experience the real thing. More information shall follow, but for a sneak peek on the rules of the game, press R. To close the popups, make sure to press C.";
    }

    public void End()
    {
        //text at end of level
        title.text = "The End of the Level";
        info.text = "Now that the fight and the quest have been finished, you will notice the progress bar now says 100%. This shows that everything has been accomplished in the level and the door to the main hub is now accessible.\r\nTo move to the main hub, simply walk up to the door ahead and press Space. This is how doors can be travelled through, but do note that if you skipped the fight and/or quest, the door will not open. You must complete a level, ie get 100% progress to exit and continue to the next stage.";
    }

    public void Fight()
    {
        //text for the fight
        title.text = "Fighting";
        info.text = "Ahead of you, there is a character. In the actual game, there will be two options for every character you encounter. One will be a fight and the other will be a quest. We shall start though, by introducing the fight.\r\nSimply walk up to the character and press the button presented to you, then you shall be in a fight!\r\nOnce in the fight, there shall be three options presented to you; fight, block, and heal. Fight will attack the character, Block will attempt to block the character's attack, and Heal will use a potion to restore a set amount of your health.\r\nIf you die in battle, you will have to restart the level from the beginning. However, if you win, your health will be restored, you will gain potions and colors to restore the world around you.\r\nIf you are lucky, your opponent may even drop a weapon or shield of their own for you to use.";   
    }

    public void Quest()
    {
        //text for the quest
        title.text = "Questing";
        info.text = "Now, there are more peaceful options to peruse, you could aid the one before you instead of meeting their hostility.\r\nA quest will grant you the same amount of colors as a fight, only instead of defeating them through violence, you shall instead aid them. This could vary from character to character and land to land.\r\nThey may need you to retrieve an item(pick up with P), or maybe defend against an enemy(block with B), or even piece together something that has broken(use mouse to drag and drop).\r\nThe only way to find what they shall do is to walk up and press the button presented to you. Once that has been pressed, press T to talk to the character and pull up their dialogue for the quest. Make sure to press the additional accept button to find what they need.";
    }
}
