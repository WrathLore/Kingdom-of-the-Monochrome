using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GreenMaiden : MonoBehaviour
{
    [Header("Scripting")]
    [SerializeField] TextMeshProUGUI questButton;
    [SerializeField] TextMeshProUGUI fightButton;
    [SerializeField] TextMeshProUGUI charText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;//MAY PUT THIS ONE DYNAMICALLY OR WHATEVER SINCE SAME EVERYTIME
    [SerializeField] Button button;//will be the quest button no matter what
    [SerializeField] GameObject questGiver;
    [SerializeField] Player player;
    [SerializeField] Creature creature;//use to get the red green blue values
    [SerializeField] Dialogue d;//use for all the dialogue needs
    //[SerializeField] PickUp pick;
    [SerializeField] Button accept;//these to set the onClick listener for the buttons
    [SerializeField] Button turnIn;
    [Header("Writing")]
    [SerializeField] string text = "A maiden sits weeping by a well. She appears distraught, with tears staining her face as she wails into the air. Such a loud noise is sure to draw in something more dangerous soon.";
    [SerializeField] string quest = "Comfort the maiden";//fetch quest or maybe fend off things coming towards you for a number of minutes(start over if they get by you)
    [SerializeField] string fight = "Tell the maiden to be quiet";
    [Header("Quest Objects")]
    [SerializeField] bool startedQuest = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!startedQuest)
            {
                if(turnIn != null)
                {
                    turnIn.gameObject.SetActive(false);
                }
                if(charText != null && fightButton != null && questButton != null)
                {
                    charText.text = text;
                    fightButton.text = fight;
                    questButton.text = quest;
                }
                SetName("Green Maiden");//MAY DO COLOR OF TEXT AS WELL DEPENDS ON HOW IT LOOKS
                if(button != null)
                {
                    button.onClick.AddListener(Quest);
                }
                if(accept != null)
                {
                    accept.onClick.AddListener(Change);
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(startedQuest)
            {
                creature.choice.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!startedQuest)
            {
                charText.text = "";
                fightButton.text = "";
                questButton.text = "";
                button.onClick.RemoveListener(Quest);
            }
            else
            {
                d.DeactivateDialogueBox();
            }
          
        }
    }


    public void Quest()
    {
        
    }

    public void Change()
    {

    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }
}
