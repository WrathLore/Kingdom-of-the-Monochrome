using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ProgressBar : MonoBehaviour
{
   //need to go from 0 to 100% as you fight/quest through the levels
   //if tutorial, only 2 characters so 50% each
   //outside of tutorial, 5 characters, so 20% each

   //will tie into the progressTracker for the player
   //this changes every level/scene
   [SerializeField] Player player;
   [SerializeField] Transform progressBar;
   [SerializeField] TextMeshProUGUI progressText;
   [SerializeField] string scene;

   void Awake()
   {
          scene = SceneManager.GetActiveScene().name;
   }

   //start the scale off at 0 and grow it by the percent from the player

   public void SetProgress(float progress, int prog)
   {
          progressBar.localScale = new Vector3(progress, progressBar.localScale.y, 1);
          progressText.text = prog.ToString() + "%";
   }

   void FixedUpdate()
   {
        if(progressBar != null && progressText != null)
        {
               if(string.Equals(scene,"GameHub"))
               {
                    if(Player.isRed && Player.isGreen && Player.isBlue)
                    {
                         SetProgress(1f, 100);
                    }
                    else if((Player.isRed && Player.isGreen) || (Player.isRed && Player.isBlue) || (Player.isGreen && Player.isBlue))
                    {
                         SetProgress(.7f, 70);
                    }
                    else if(Player.isRed || Player.isGreen || Player.isBlue)
                    {
                         SetProgress(.3f,30);
                    }
                    else
                    {
                         SetProgress(0f, 0);
                    }

               }
               else
               {
                    SetProgress(player.progressTracker, player.progress);
               }
        }
   }
}
