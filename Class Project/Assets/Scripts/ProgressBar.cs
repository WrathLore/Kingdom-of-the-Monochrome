using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            SetProgress(player.progressTracker, player.progress);
        }
   }
}
