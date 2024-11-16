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
    [SerializeField] bool maiden = false;
    [SerializeField] bool lamb = false;
    [SerializeField] bool unique = false;
    //this way can tell which one specifically they may be involved with
    //can go back in later and delete the ones that may not be used
    //or keep them in for further development down the line

    [Header("Characters")]
    [SerializeField] GreenFairy greenFairy;
    [SerializeField] GreenDevout greenDevout;
    [SerializeField] GreenMaiden greenMaiden;
    [SerializeField] GreenLamb greenLamb;
    [SerializeField] GreenUnique greenUnique;
    [SerializeField] RedFairy redFairy;
    [SerializeField] RedDevout redDevout;
    [SerializeField] RedMaiden redMaiden;
    [SerializeField] RedLamb redLamb;
    [SerializeField] RedUnique redUnique;
    [SerializeField] BlueFairy blueFairy;
    [SerializeField] BlueDevout blueDevout;
    [SerializeField] BlueMaiden blueMaiden;
    [SerializeField] BlueLamb blueLamb;
    [SerializeField] BlueUnique blueUnique;


    void Awake()
    {
        greenFairy = FindObjectOfType<GreenFairy>();
        greenDevout = FindObjectOfType<GreenDevout>();
        greenMaiden = FindObjectOfType<GreenMaiden>();
        greenLamb = FindObjectOfType<GreenLamb>();
        greenUnique = FindObjectOfType<GreenUnique>();
        redFairy = FindObjectOfType<RedFairy>();
        redDevout = FindObjectOfType<RedDevout>();
        redMaiden = FindObjectOfType<RedMaiden>();
        redLamb = FindObjectOfType<RedLamb>();
        redUnique = FindObjectOfType<RedUnique>();
        blueFairy = FindObjectOfType<BlueFairy>();
        blueDevout = FindObjectOfType<BlueDevout>();
        blueMaiden = FindObjectOfType<BlueMaiden>();
        blueLamb = FindObjectOfType<BlueLamb>();
        blueUnique = FindObjectOfType<BlueUnique>();
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
        if(fairy)
        {
            if(greenFairy != null)
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
            if(redFairy != null)
            {

            }
            if(blueFairy != null)
            {
                player.RegisterItem(questObject);
                onItem = false;
                pickedUp = true;
                item.SetActive(false);
            }
        }
        else if(devout)
        {
            if(greenDevout != null)
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

            if(redDevout != null)
            {
                if(string.Equals(questObject,"Wrong"))
                {
                    //pop up the dialogue box to say wrong item, don't even think about picking that up
                    redDevout.wrongItem = true;
                }
                else
                {
                    player.RegisterItem(questObject);
                    onItem = false;
                    pickedUp = true;
                    item.SetActive(false);
                    //right object, so put it into items
                }

            }
            if(blueDevout != null)
            {

            }
        }
        else if(lamb)
        {
            if(greenLamb != null)
            {

            }
            if(redLamb != null)
            {
                //only called if onItem is true
                player.RegisterItem(questObject);
                onItem = false;
                pickedUp = true;
                item.SetActive(false);
            }
            if(blueLamb != null)
            {

            }

        }
        else if(maiden)
        {
            if(greenMaiden != null)
            {
                //only called if onItem is true
                player.RegisterItem(questObject);
                onItem = false;
                pickedUp = true;
                item.SetActive(false);
            }
            if(redMaiden != null)
            {

            }
            if(blueMaiden != null)
            {

            }
            
        }
        else if(unique)
        {
            if(greenUnique != null)
            {

            }
            if(redUnique != null)
            {

            }
            if(blueUnique != null)
            {
                player.RegisterItem(questObject);
                onItem = false;
                pickedUp = true;
                Destroy(item); //this will be instatiated in, so want to destroy it once it is picked up
            }
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
