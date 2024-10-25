using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{//could do thing with player where weapon "levels up" when an area is complete
//ie a different name for the weapon and an increase in damage
    [SerializeField] public string weaponName;
    [SerializeField] public int damage;//right now just need the weapon name and the damage it can deal
    //might make more complicated if time but this is good for now
    [SerializeField] public string shieldName;
    [SerializeField] public int block;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
