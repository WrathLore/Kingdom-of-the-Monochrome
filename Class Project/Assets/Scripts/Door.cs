using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    //use this script to decide which version of the door to put on the main hub
    [SerializeField] string newLocation;//use to find which location it is going to
    [SerializeField] bool entered = false;//to determine if area is still accessible or not
    //CANNOT DO IT AS STATIC as this is alot of different doors
    //so need to figure out a way to prevent player from reentering a scene other than main hub
    //WORK IN PROGRESS, LIKELY GET TO THIS AT THE END
    string scene;//the new scene to move to
    GameObject player;
    //it will only be one player per area, so only one to find

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ChangeLocation();
    }
    public void ChangeLocation()
    {
        if(string.Equals(newLocation, "Red"))//head into red level
        {
            scene = "RedLevel";
        }
        else if(string.Equals(newLocation, "Green"))//head into green level
        {
            scene = "GreenLevel";
            //SceneManager.GetSceneByName("GreenLevel");
        }
        else if(string.Equals(newLocation, "Blue"))//head into blue level
        {
            scene = "BlueLevel";
        }
        else if(string.Equals(newLocation, "Final"))//head into the final level
        {
            scene = "FinalLevel";
        }
        else//head back to the hub
        {
            scene = "GameHub";
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !entered && Mathf.Approximately(player.GetComponent<Player>().progressTracker, 1f))
        {
            //if button is pressed(space bar), then go to new scene
            player.GetComponent<Player>().SetScene(scene);
        }
        else if(other.CompareTag("Player") && !entered && !string.Equals(scene,"GameHub"))
        {//to move around on main hub screen to other levels
            player.GetComponent<Player>().SetScene(scene);
        }
        else if(entered)
        {
            //print out a little blurb of some kind (flavor text) to let player know they can't return to this level
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !entered)
        {
          player.GetComponent<Player>().SetScene("");
        }
    }

    
}