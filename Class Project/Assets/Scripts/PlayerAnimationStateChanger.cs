using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateChanger : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string currentState = "Idle";

    void Start()
    {
        ChangeAnimationState("Idle");
    }

    public void ChangeAnimationState(string newState, float speed = 1)
    {
        animator.speed = speed;
        if(string.Equals(currentState, newState))
        {
            return;
        }
        currentState = newState;
        animator.Play(currentState);

    }
}
