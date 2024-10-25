using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScript : MonoBehaviour
{
    [SerializeField] public GameObject panel;
    [SerializeField] public GameObject tutorial;
    [SerializeField] public GameObject start;
    [SerializeField] public GameObject stats;
    public static bool called = false;
    // Start is called before the first frame update
    void Start()
    {
        if(Player.FirstEnter())
        {
            Welcome();
        }
    }

    public void Welcome()
    {//have player press a button maybe to get rid of the popup here
        if(stats != null)
        {
            stats.SetActive(false);
        }
        if(Player.TutorialDone())
        {
            //show the welcome script for starting game from start button
            if(panel != null && tutorial != null)
            {
                panel.SetActive(true);
                tutorial.SetActive(true);
                start.SetActive(false);
            }
        }
        else{
            //show welcome script for starting game after tutorial
            if(panel != null && start != null)
            {
                panel.SetActive(true);
                start.SetActive(true);
                tutorial.SetActive(false);
            }
        }
    }
}
