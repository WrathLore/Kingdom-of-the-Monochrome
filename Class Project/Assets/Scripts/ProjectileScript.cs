using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //USE FOR ANY PROJECTILE STUFF
    //BLOCKING PROJECTILES IN PLAYER SCRIPT TO BE USED WITH IT
    //CAN TAKE CERTAIN AMOUNT OF HITS BEFORE YOU LOSE THE BLOCKING MINIGAME/QUEST
    [SerializeField] Player player;
    [SerializeField] GreenMaiden greenMaiden;//easiest to do it this way
    [SerializeField] RedUnique redunique;

    void OnTriggerEnter2D(Collider2D other)//if hit the player
    {
        if(other.CompareTag("Player") && player != null)
        {
            if(!player.blocked)
            {
                player.IncreaseHits();
            }
            else
            {
                //if player blocked, then remove the projectile entirely
                Destroy(this.gameObject);
            }
        }

        if(other.CompareTag("Green Maiden") && greenMaiden != null)
        {
            greenMaiden.hitPoints--;
            Destroy(this.gameObject);
        }
    }
}
