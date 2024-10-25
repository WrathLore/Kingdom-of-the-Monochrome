using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateStates : MonoBehaviour
{
   [Header("Stats")]
   [SerializeField] TextMeshProUGUI redStats;
   [SerializeField] TextMeshProUGUI greenStats;
   [SerializeField] TextMeshProUGUI blueStats;

   void FixedUpdate()
   {
        Increase();
   }

   public void Increase()
   {
        if(redStats != null && greenStats != null && blueStats != null)
        {
            redStats.text = Player.red.ToString();
            greenStats.text = Player.green.ToString();
            blueStats.text = Player.blue.ToString();
        }

   }
}
