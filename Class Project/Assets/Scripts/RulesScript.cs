using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesScript : MonoBehaviour
{
    [SerializeField] public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RulesOpen()
    {
        if(panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void RulesClose()
    {
        if(panel != null)
        {
            panel.SetActive(false);
        }
    }
}
