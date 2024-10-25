using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    //use for the dialogue in the quests
    [Header("Text Info")]
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Button accept;//accept the quest
    [SerializeField] Button turnIn;//turn in the quest
    [SerializeField] TextMeshProUGUI dialogue;
    [SerializeField] Image charImage;//will look into later, ignore for now
    [SerializeField] TextMeshProUGUI charName;
    public static bool activated = false;

    //need to make dialogue box active when quest button is clicked
    //and need to put in the right images and dialogue and the like

    public void SetName(string nameChar)
    {
        if(charName != null)
        {
            charName.text = nameChar;
        }
    }

    public void SetDialogue(string d)
    {
        if(dialogue != null)
        {
            dialogue.text = d;
        }
    }

    public void ActivateDialogueBox()
    {
        if(dialogueBox != null)
        {
            dialogueBox.SetActive(true);
            activated = true;
        }
    }

    public void DeactivateDialogueBox()
    {
        if(dialogueBox != null)
        {
            dialogueBox.SetActive(false);
            activated = false;
        }
    }
}
