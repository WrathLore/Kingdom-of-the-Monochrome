using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //for location of the objects to pickup, could potentially randomize it
    //make it within a certain area, but random every time on Start
    [SerializeField] Player player;
    [SerializeField] GameObject item;
    //use this to pick up quest items and the like
    [Header("Information")]
    [SerializeField] string questObject;//public so that can use it in player input controller
    public bool pickedUp = false; //use to tell if item has been picked up now
    public static bool onItem = false;
    //will be best to have this bool be static so that can do it multiple times

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && onItem)
        {//don't have to technically do keydown, but best to do it just in case
            PickUpItem();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            onItem = true;//just need to do this
        }
        
    }

    public void PickUpItem()
    {
        //only called if onItem is true
        player.RegisterItem(questObject);
        onItem = false;
        pickedUp = true;
        item.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            onItem = false; 
        }
          
    }

    
}
