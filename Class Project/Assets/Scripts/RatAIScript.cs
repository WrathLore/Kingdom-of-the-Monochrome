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

    public string currentStateString;
    Rigidbody2D pb;
    delegate void AIState();
    AIState currentState;
    [SerializeField] PlayerAnimationStateChanger a; //since this is where movement is changed, this is where animation is changed


    //trackers==================================================
    float stateTime = 0;
    bool justChangedState = false;
    int startingDirection = 0;
    //bool onWall = false;
    float speed = 6f;

    void Awake()
    {
        pb = GetComponent<Rigidbody2D>();
    }

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
        currentStateString = "Left";
        a.ChangeAnimationState("Left");
        //if bump into a wall, try going down
        //also if certain amount of time passed without bumping into something go down
        if(stateTime > 3)
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
        /*if(onWall && stateTime > 0)
        {
            ChangeState(RightState);
            return;
        }*/
        pb.velocity = Vector3.left * speed;
    }

    void RightState()
    {
        currentStateString = "Right";
        a.ChangeAnimationState("Right");
        //if bump into a wall, try going up
        //also if certain amount of time passed without bumping into something go up
        if(stateTime > 3)
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
        /*if(onWall && stateTime > 0)
        {
            ChangeState(LeftState);
            return;
        }*/
        pb.velocity = Vector3.right * speed;
        
    }

    void UpState()
    {
        currentStateString = "Up";
        a.ChangeAnimationState("Up");
        //if bump into a wall, try going left
        //also if certain amount of time passed without bumping into something go left
        if(stateTime > 3)
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
        /*if(onWall && stateTime > 0)
        {
            ChangeState(DownState);
            return;
        }*/
        pb.velocity = Vector3.up * speed;
    }

    void DownState()
    {
        currentStateString = "Down";
        a.ChangeAnimationState("Down");
        //if bump into a wall, try going right
        //also if certain amount of time passed without bumping into something go right
        if(stateTime > 3)
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
       /* if(onWall && stateTime > 0)
        {
            ChangeState(UpState);
            return;
        }*/
        pb.velocity = Vector3.down * speed;
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
            if(string.Equals(currentStateString,"Right"))
            {
                ChangeState(LeftState);
            }
            else if(string.Equals(currentStateString,"Left"))
            {
                ChangeState(RightState);
            }
            else if(string.Equals(currentStateString,"Up"))
            {
                ChangeState(DownState);
            }
            else
            {
                ChangeState(UpState);
            }
        }
        
    }

    void Update()
    {
        AITick();
    }
}
