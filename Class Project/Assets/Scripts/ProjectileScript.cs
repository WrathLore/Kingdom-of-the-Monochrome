using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //USE FOR ANY PROJECTILE STUFF
    //BLOCKING PROJECTILES IN PLAYER SCRIPT TO BE USED WITH IT
    //CAN TAKE CERTAIN AMOUNT OF HITS BEFORE YOU LOSE THE BLOCKING MINIGAME/QUEST
    GameObject player;
    GameObject greenMaiden;//easiest to do it this way
    GameObject redUnique;
    GameObject launcher;
    [SerializeField] float speed = 7f;

    void Awake()
    {
        //want to set the projectile launcher here since projectiles are spawned in
        launcher = GameObject.FindGameObjectWithTag("Projectile Launcher");
        player = GameObject.FindGameObjectWithTag("Player");
        greenMaiden = GameObject.FindGameObjectWithTag("Green Maiden");
        redUnique = GameObject.FindGameObjectWithTag("Red Unique");

    }

    void Update()
    {
        if(greenMaiden != null)
        {
            MoveTowardsMaiden();
        }

        if(redUnique != null)
        {
            MoveDown();
        }
    }

    void OnTriggerEnter2D(Collider2D other)//if hit the player or green maiden
    {
        if(other.CompareTag("Player") && player != null)
        {
            if(!player.GetComponent<Player>().blocked)
            {
                player.GetComponent<Player>().IncreaseHits();
            }
            launcher.GetComponent<ProjectileLauncher>().destroyedProjectiles++;
            Destroy(this.gameObject);
        }

        if(other.CompareTag("Green Maiden") && greenMaiden != null)
        {//had to put rigid body on green maiden for this to work, also changed the box collider size back to 1 to 1 instead of 2.7 to 2.7 since that is where the arrow is destroyed at
            greenMaiden.GetComponent<GreenMaiden>().hitPoints--;
            launcher.GetComponent<ProjectileLauncher>().destroyedProjectiles++;
            Destroy(this.gameObject);
        }
        
        if(other.CompareTag("Ground"))
        {
            launcher.GetComponent<ProjectileLauncher>().destroyedProjectiles++;
            Destroy(this.gameObject);
        }

    }

    public void AimProjectile(Vector3 pos)
    {
        Quaternion goalRotation = Quaternion.LookRotation(Vector3.forward, pos - transform.position);
        Quaternion currentRotation = transform.rotation;

        transform.rotation = Quaternion.Lerp(currentRotation, goalRotation,Time.deltaTime * int.MaxValue);
        //int.MaxValue to get it to automatically rotate basically, otherwise this wouldn't work

    }

    public void MoveTowardsMaiden()
    {
        transform.position = Vector2.MoveTowards(transform.position, launcher.transform.position,Time.deltaTime*speed);
    }

    public void MoveDown()
    {
        transform.position += Vector3.down * Time.deltaTime * 7;
    }
}
