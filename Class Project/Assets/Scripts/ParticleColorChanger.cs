using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorChanger : MonoBehaviour
{
    //change the color of the particle system overtime
    //or more specifically, on start
    //more color as more levels are completed
    [SerializeField] ParticleSystem rainSystem;

    void Start()
    {
        var col = rainSystem.colorOverLifetime;
        col.enabled = true;

        Gradient grad = new Gradient();
        if(Player.isRed && !Player.isBlue && !Player.isGreen)
        {
            //red
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f),  new GradientColorKey(Color.red, 0.206f), new GradientColorKey(Color.black, 0.5f), new GradientColorKey(Color.black, 0.786f),new GradientColorKey(Color.black, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }
        else if(Player.isRed && Player.isGreen && !Player.isBlue)
        {
            //red and green
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f),  new GradientColorKey(Color.red, 0.206f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.black, 0.786f),new GradientColorKey(Color.black, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }
        else if(Player.isRed && !Player.isGreen && Player.isBlue)
        {
            //red and blue
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f),  new GradientColorKey(Color.red, 0.206f), new GradientColorKey(Color.black, 0.5f), new GradientColorKey(Color.blue, 0.786f),new GradientColorKey(Color.black, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }
        else if(!Player.isRed && Player.isGreen && !Player.isBlue)
        {
            //green
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f),  new GradientColorKey(Color.black, 0.206f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.black, 0.786f),new GradientColorKey(Color.black, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }
        else if(!Player.isRed && Player.isGreen && Player.isBlue)
        {
            //blue and green
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.black, 0.206f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.blue, 0.786f), new GradientColorKey(Color.black, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }
        else if(!Player.isRed && !Player.isGreen && Player.isBlue)
        {
            //blue
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.black, 0.206f), new GradientColorKey(Color.black, 0.5f), new GradientColorKey(Color.blue, 0.786f),new GradientColorKey(Color.black, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }
        else if(Player.isRed && Player.isGreen && Player.isBlue)
        {
            //red green and blue
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.red, 0.206f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.blue, 0.786f), new GradientColorKey(Color.black, 1.0f)}, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }
        else
        {
            //if nothing is true, all color is black
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.black, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) } );
        }

        col.color = grad;
    }

    
}
