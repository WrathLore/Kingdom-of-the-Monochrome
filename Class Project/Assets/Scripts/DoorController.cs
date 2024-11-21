using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    //for main hub; on start, check to see if the door needs to be replaced
    //so on start call the red door green door blue door final door methods to see if they can change to the right form or not
    //THIS CLASS TO BE USED ONLY IN MAIN HUB
    [SerializeField] Player player;
    [SerializeField] List<Door> doors;

    void Start()
    {
        //swap the doors around as needed for the main hub
        //in the actual levels, might want to just keep it as it already is since it works
        if(doors.Count > 2)
        {
            RedDoor();
            GreenDoor();
            BlueDoor();
            FinalDoor();
        }
        
    }

    //swap around the doors as needed below
    public void RedDoor()//set red door to right colors
    {
        if(Player.isRed)//need to be main hub, also need to select only the correct doors
        {
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Red Base Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Finished Red Door"))
                {
                    door.gameObject.SetActive(true);
                }
            }

        }
    }

    public void GreenDoor()//set green door to right colors
    {
        if(Player.isGreen)
        {
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Green Base Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Finished Green Door"))
                {
                    door.gameObject.SetActive(true);
                }
            }

        }
    }

    public void BlueDoor()//set blue door to right colors
    {
        if(Player.isBlue)
        {
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Blue Base Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Finished Blue Door"))
                {
                    door.gameObject.SetActive(true);
                }
            }

        }
    }

    public void FinalDoor()//set the final door to the right colors
    {
        if(Player.isRed && !Player.isBlue && !Player.isGreen)
        {
            //red only
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Final Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Final Door R"))
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
        else if(Player.isRed && Player.isGreen && !Player.isBlue)
        {
            //red and green
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Final Door R") || string.Equals(door.name,"Final Door G") || string.Equals(door.name,"Final Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Final Door RG"))
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
        else if(Player.isRed && !Player.isGreen && Player.isBlue)
        {
            //red and blue
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Final Door R") || string.Equals(door.name,"Final Door B") || string.Equals(door.name,"Final Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Final Door RB"))
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
        else if(!Player.isRed && Player.isGreen && !Player.isBlue)
        {
            //green only
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Final Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Final Door G"))
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
        else if(!Player.isRed && Player.isGreen && Player.isBlue)
        {
            //green and blue
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Final Door B") || string.Equals(door.name,"Final Door G") || string.Equals(door.name,"Final Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Final Door GB"))
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
        else if(!Player.isRed && !Player.isGreen && Player.isBlue)
        {
            //blue only
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Final Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Final Door B"))
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
        else if(Player.isRed && Player.isGreen && Player.isBlue)
        {
            //red green and blue
            foreach(Door door in doors)
            {
                if(string.Equals(door.name,"Final Door RB") || string.Equals(door.name,"Final Door RG") || string.Equals(door.name,"Final Door GB") || string.Equals(door.name,"Final Door"))
                {
                    door.gameObject.SetActive(false);
                }
                if(string.Equals(door.name,"Finished Final Door"))
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
    }

    
}