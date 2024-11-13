using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject circlePrefab;
    //these will be from the prefabs folder as they are being launched 
    [SerializeField] string scene;
    public int maxProjectiles = 10;
    [SerializeField] int currentProjectiles = 0;
    [SerializeField] Transform finalDest;
    public int destroyedProjectiles = 0;//increase by 1 every time a projectile is destroyed

    void Start()
    {
        scene = SceneManager.GetActiveScene().name;
        //if scene is GreenLevel, then projectile to shoot out is arrow
        //if scene is RedLevel, then projectile to shoot out is circle
    }


    //if green level, projectiles go towards the launch handler

    //if red level, projectiles move away from the launch handler

    public void Launch()
    {
        if(string.Equals(scene, "GreenLevel"))
        {
            //spread out in about a 20 by 20 circle and move towards the center of the circle
            //so pick the location on the edge of the circle
            //so basically want them to show up in the circle of trees so do like Random.Range(transform.position.x+25, transform.position.x+20)
            //rotate the object so that it is facing the right way
            //FOR ROTATE; projectile script has method aim projectile on it
            //so take the position of the projectile launcher as the vector 3
            //then move it towards the launch handler
            float x1 = Random.Range(finalDest.position.x+7, finalDest.position.x+12);
            float x2 = Random.Range(finalDest.position.x-12, finalDest.position.x-7);
            float y1 = Random.Range(finalDest.position.y+7, finalDest.position.y+12);
            float y2 = Random.Range(finalDest.position.y-12, finalDest.position.y-7);
            int xLocation = Random.Range(1,3);
            int yLocation = Random.Range(1,3);
            float x, y;
            if(xLocation == 2)
            {
                x = x2;
            }
            else
            {
                x = x1;
            }
            if(yLocation == 2)//do the opposite of the x 
            {
                y = y1;
            }
            else
            {
                y = y2;
            }
            GameObject arrow = Instantiate(arrowPrefab, new Vector3(x,y,transform.position.z), transform.rotation);
            arrow.GetComponent<ProjectileScript>().AimProjectile(finalDest.position);
            //arrows are now spawning in correctly, now just need them to start moving towards the finalDest position
            currentProjectiles++;
        }
        else if(string.Equals(scene, "RedLevel"))
        {

        }
    }

    public IEnumerator WaitRoutine()
    {
        while(currentProjectiles < maxProjectiles)
        {
            Launch();
            yield return new WaitForSeconds(5f);
        }
    }

}
