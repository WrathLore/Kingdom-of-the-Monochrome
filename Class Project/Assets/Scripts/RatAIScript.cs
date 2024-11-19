using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAIScript : MonoBehaviour
{
    //script to be used for the rat's movements for blue unique quest
    //spawn in a rat, maybe 2 if want to get more into it
    //have it move around semi erratically in the environment
    //keep it in a certain area, then once it is picked up
    //P is pressed while in its hitbox, then it turns off
    //otherwise it keeps on moving
    [SerializeField] string currentStateString; //use for more of a visual cue than anything else

    delegate void AIState();
    AIState currentState;
    [SerializeField] PlayerAnimationStateChanger a; //since this is where movement is changed, this is where animation is changed


    //trackers==================================================
    float stateTime = 0;
    bool justChangedState = false;
    int startingDirection = 0;
    bool onWall = false;

    void Start()
    {
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));
        startingDirection = Random.Range(0,4);
        if(startingDirection == 1)
        {
            ChangeState(RightState);
        }
        else if(startingDirection == 2)
        {
            ChangeState(UpState);
        }
        else if(startingDirection == 3)
        {
            ChangeState(LeftState);
        }
        else
        {
            ChangeState(DownState);
        }
    }

    void ChangeState(AIState newAIState)
    {
        currentState = newAIState;
        justChangedState = true;
    }
    //change state, ie direction, whenever you bump into a wall
    //so each one will just try the next direction and then the next direction 
    //if player is on it and presses P, then destroy it and give the player the rat or whatever
    //use PickUp script for that basically
    void LeftState()
    {
        a.ChangeAnimationState("Left");
        currentStateString = "Left";
        //if bump into a wall, try going down
        //also if certain amount of time passed without bumping into something go down
        if(stateTime > 5)
        {
            int rand = Random.Range(1,4);
            if(rand == 2)
            {
                ChangeState(UpState);
            }
            else if(rand == 3)
            {
                ChangeState(RightState);
            }
            else
            {
                ChangeState(DownState);
            }
            return;
        }
        if(onWall && stateTime > 2)
        {
            ChangeState(RightState);
            return;
        }
        transform.position += Vector3.left * 0.03f;
    }

    void RightState()
    {
        a.ChangeAnimationState("Right");
        currentStateString = "Right";
        //if bump into a wall, try going up
        //also if certain amount of time passed without bumping into something go up
        if(stateTime > 4)
        {
            int rand = Random.Range(1,4);
            if(rand == 2)
            {
                ChangeState(DownState);
            }
            else if(rand == 3)
            {
                ChangeState(LeftState);
            }
            else
            {
                ChangeState(UpState);
            }
            return;
        }
        if(onWall && stateTime > 2)
        {
            ChangeState(LeftState);
            return;
        }
        transform.position += Vector3.right * 0.03f;
        
    }

    void UpState()
    {
        a.ChangeAnimationState("Up");
        currentStateString = "Up";
        //if bump into a wall, try going left
        //also if certain amount of time passed without bumping into something go left
        if(stateTime > 4)
        {
            int rand = Random.Range(1,4);
            if(rand == 2)
            {
                ChangeState(RightState);
            }
            else if(rand == 3)
            {
                ChangeState(DownState);
            }
            else
            {
                ChangeState(LeftState);
            }
            return;
        }
        if(onWall && stateTime > 2)
        {
            ChangeState(DownState);
            return;
        }
        transform.position += Vector3.up * 0.03f;
    }

    void DownState()
    {
        a.ChangeAnimationState("Down");
        currentStateString = "Down";
        //if bump into a wall, try going right
        //also if certain amount of time passed without bumping into something go right
        if(stateTime > 5)
        {
            int rand = Random.Range(1,4);
            if(rand == 2)
            {
                ChangeState(LeftState);
            }
            else if(rand == 3)
            {
                ChangeState(UpState);
            }
            else
            {
                ChangeState(RightState);
            }
            return;
        }
        if(onWall && stateTime > 2)
        {
            ChangeState(UpState);
            return;
        }
        transform.position += Vector3.down * 0.03f;
    }

     void AITick()
     {
        if(justChangedState)
        {
            stateTime = 0;
            justChangedState = false;
        }
        currentState();
        stateTime += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            onWall = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            onWall = false;
        }
        
    }

    void Update()
    {
        AITick();
    }
}
