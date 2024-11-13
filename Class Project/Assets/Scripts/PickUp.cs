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
    public bool onItem = false;
    [SerializeField] bool fairy = false;
    [SerializeField] bool devout = false;

    [SerializeField] GreenFairy greenFairy;
    [SerializeField] GreenDevout greenDevout;

    void Awake()
    {
        greenFairy = FindObjectOfType<GreenFairy>();
        greenDevout = FindObjectOfType<GreenDevout>();
    }

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
        if(greenFairy != null && fairy)
        {
            if(!greenFairy.oneItem)
            {
                //need to make sure can only pick up one item at a time to not cheat the system
                greenFairy.oneItem = true;
                player.RegisterItem(questObject);
                onItem = false;
                pickedUp = true;
                item.SetActive(false);
            }
        }
        else if(greenDevout != null && devout)
        {
            if(this.CompareTag("Bauble") && !greenDevout.bauble)
            {
                greenDevout.bauble = true;
                greenDevout.itemsPicked++;
                greenDevout.baubleItem = questObject;
                onItem = false;
                pickedUp = true;
                item.SetActive(false);

            }
            if(this.CompareTag("Statue") && !greenDevout.statue)
            {
                greenDevout.statue = true;
                greenDevout.itemsPicked++;
                greenDevout.statueItem = questObject;
                onItem = false;
                pickedUp = true;
                item.SetActive(false);

            }
            if(this.CompareTag("Tower") && !greenDevout.tower)
            {
                greenDevout.tower = true;
                greenDevout.itemsPicked++;
                greenDevout.towerItem = questObject;
                onItem = false;
                pickedUp = true;
                item.SetActive(false);

            }
        }
        else
        {
            //only called if onItem is true
            player.RegisterItem(questObject);
            onItem = false;
            pickedUp = true;
            item.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            onItem = false; 
        }
          
    }

    
}
