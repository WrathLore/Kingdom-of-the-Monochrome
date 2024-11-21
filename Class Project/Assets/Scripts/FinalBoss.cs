using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    //So, if player has fought more then they have quested
    //it goes straight into the fight script
    //otherwise dragon will stay on screen and only dialogue will show up
    //I guess
    //basically once you collide with dragon circle will unlock 
    //if you fight and lose, then booted out to main menu with all progress lost
    //same sort of thing with if you walk into the circle and press E
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI charText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;//MAY PUT THIS ONE DYNAMICALLY OR WHATEVER SINCE SAME EVERYTIME
    [SerializeField] GameObject questGiver;
    [SerializeField] Player player;
    [SerializeField] Creature creature;//use to get the red green blue values
    [SerializeField] Dialogue d;//use for all the dialogue needs
}
