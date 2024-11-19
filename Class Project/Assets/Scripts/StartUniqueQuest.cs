using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUniqueQuest : MonoBehaviour
{
    //use for the circle for red unique for now
    //this way can set Player.onCircle to true
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player.onCircle = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player.onCircle = false;
        }
        
    }
}
