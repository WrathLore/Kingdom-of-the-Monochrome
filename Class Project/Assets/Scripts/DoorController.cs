using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    //THIS CLASS TO BE USED ONLY IN MAIN HUB
    public static DoorController singleton;
    [SerializeField] Player player;
    [SerializeField] List<Door> doors;

    void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this.gameObject);//only need one DoorController, but not sure how this will work across scenes, so may need to look this over later
        }
    }

    public void RegisterDoor(Door d)
    {//will only register activated doors
        doors.Add(d);
    }

    //swap around the doors as needed below
    public void RedDoor()//set red door to right colors
    {
        if(Player.isRed)//need to be main hub, also need to select only the correct doors
        {

        }
    }

    public void GreenDoor()//set green door to right colors
    {
        if(Player.isGreen)
        {

        }
    }

    public void BlueDoor()//set blue door to right colors
    {
        if(Player.isBlue)
        {

        }
    }

    public void FinalDoor()//set the final door to the right colors
    {
        if(Player.isRed && !Player.isBlue && !Player.isGreen)
        {
            //red only
        }
        else if(Player.isRed && Player.isGreen && !Player.isBlue)
        {
            //red and green
        }
        else if(Player.isRed && !Player.isGreen && Player.isBlue)
        {
            //red and blue
        }
        else if(!Player.isRed && Player.isGreen && !Player.isBlue)
        {
            //green only
        }
        else if(!Player.isRed && Player.isGreen && Player.isBlue)
        {
            //green and blue
        }
        else if(!Player.isRed && !Player.isGreen && Player.isBlue)
        {
            //blue only
        }
        else if(Player.isRed && Player.isGreen && Player.isBlue)
        {
            //red green and blue
        }
        else
        {
            //no colors
        }
    }

    
}