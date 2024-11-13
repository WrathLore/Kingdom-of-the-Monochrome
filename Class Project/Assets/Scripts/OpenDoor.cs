using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    //use in conjuction with any doors that need to be unlocked
    [SerializeField] GameObject door;//door to be unlocked
    [SerializeField] string doorKey;//object to unlock the door
    [SerializeField] Player player;//player with the object to unlock the door
    bool onDoor = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            if(onDoor)
            {
                foreach(string item in player.questItems)
                {
                    if(string.Equals(doorKey,item))
                    {
                        door.SetActive(false);
                        onDoor = false;
                    }
                }
            }
        }
    }

        
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            onDoor = true;
        }
        
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            onDoor = false;
        }
        
    }
}
