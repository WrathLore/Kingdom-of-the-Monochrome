using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    //handler for timers
    //set a timer and update per second
    //if time runs out, start the level over
    //otherwise if win condition met, then end the timer early and nothing else happens
    //so call it in the right spot and then if condition is won, then end the timer

    //so maybe something like

    //while (win condition not met)
    //      while(timer != 0)
    //          decrease timer by one every second
    //          if( win condition met)
    //              break
    //      end while
    //      if( timer != 0)
    //          break
    //      restart the scene
    //end while
}
