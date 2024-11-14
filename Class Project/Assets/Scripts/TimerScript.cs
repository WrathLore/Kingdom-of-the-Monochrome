using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] TextMeshProUGUI textTimer;
    [SerializeField] PuzzleScript puzzle;//to check if puzzle is completed or not
    [SerializeField] float currentTime = 300.0f;
    [SerializeField] float maxTime = 300.0f;
    [SerializeField] Player player;
    [SerializeField] GreenDevout greenDevout;
    [SerializeField] Dialogue d;

    void Awake()
    {
        greenDevout = FindObjectOfType<GreenDevout>();
    }

    void Start()
    {
        textTimer.text = currentTime.ToString() + " seconds remaining.";
    }

    //https://discussions.unity.com/t/c-countdown-timer/37915/2
    //comment 2 was where the main idea for this coroutine came from
    public IEnumerator CountDownRoutine(string currentTimer)
    {
        if(puzzle != null && string.Equals(currentTimer, "puzzle"))
        {
            while(currentTime > 0 && !puzzle.finishedPuzzle)
            {
                yield return new WaitForSeconds(1.0f);
                currentTime--;
                textTimer.text = currentTime.ToString() + " seconds remaining.";
            }

            //reached here means timer has ended or the puzzle has been completed
            if(currentTime == 0)
            {
                //reload the scene basically
                puzzle.FinishGame();//destroy the pieces just in case
                player.questFailed = true;
                player.Defeat();
            }
            else
            {
                //if reached here, then puzzle was completed, so reset the timer I guess
                currentTime = maxTime;
                textTimer.text = currentTime.ToString() + " seconds remaining.";
            }
        }
        else if(greenDevout != null && string.Equals(currentTimer,"greenDevout"))
        {
            while(currentTime > 0 && greenDevout.correctItems != 3)
            {
                yield return new WaitForSeconds(1.0f);
                currentTime--;
                textTimer.text = currentTime.ToString() + " seconds remaining.";
            }

            if(currentTime == 0)
            {//should just have to do this for the green devout since everything is being done through the green devout script
                player.questFailed = true;
                player.Defeat();
            }
            else
            {
                currentTime = maxTime;
                textTimer.text = currentTime.ToString() + " seconds remaining.";
            }
        }
    }

    public IEnumerator ClosePopUpRoutine(int time)
    {
        while(time > 0)
        {
            yield return new WaitForSeconds(1.0f);
            time--;
        }
        if(time == 0)
        {
            if(d != null)
            {
                d.DeactivateDialogueBox();
            }

        }
    }

}
