using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousSounds : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    //use for various sounds that only want to play on objects otherwise not interacted with
    //ie objects that don't have unique scripts already

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
