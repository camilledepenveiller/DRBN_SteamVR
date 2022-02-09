using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class molcounter : MonoBehaviour
{
    public static int limit=40;
    public static List<Transform> molecules = new List<Transform>();
    Transform sacrifice;

    //void UnityEngine.SceneManagement.SceneManager.sceneLoaded //<---- find more info about this function 

    //destroy molecule if list goes above N
    private void Update()
    {
        Langevin_v2 Lange = GameObject.Find("Simulation").GetComponent<Langevin_v2>();
        if (limit!=0)
        {
            if (molecules.Count > limit)
            {
                sacrifice = molecules[0];
                
                //add lines to cleanup langevin rigid body list before flushing out gameobject
                List<Rigidbody> GO = new List<Rigidbody>();

                //GO.AddRange(sacrifice.gameObject.GetComponentsInChildren<Rigidbody>()); // too finicky, might be a problem with joints... 
                GO.AddRange(sacrifice.gameObject.GetComponents<Rigidbody>());
                Debug.Log("sacrifiiiiice " + sacrifice.gameObject.GetComponents<Rigidbody>());

                //Debug.Log("sacrifiiiiice " + sacrifice.gameObject.GetComponentsInChildren<Rigidbody>());
                //Debug.Log("GO.Count " + GO.Count);
                foreach (Rigidbody GOel in GO) {
                    Debug.Log(GOel);
                    Lange.GOS.Remove(GOel);
                    //Debug.Log("Removed " + GOel.name);
                }
                // problematic for now, might work on it more later... 
                //molecules.Remove(sacrifice);
                //Destroy(sacrifice.gameObject, 0);

                
            }
        }
    }
}
