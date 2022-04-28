using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMode : MonoBehaviour
{

    public string savepath = ""; // make a variable that can be checked by other functions like SaveSnapShot
    public string transformName = "Elastin_f60_pp"; // replace by command line argument later
    public int repeat = 5; // number of molecules in the simulation, replace by command line argument later
                    //UnityEngine.Random rnd = new UnityEngine.Random();

    // Start is called before the first frame update
    void Start()
    {
        ////comment for now, wait until we have a functional instantiation in normal game mode
        if (Application.isBatchMode)
        {
            Debug.Log("In BatchMode");

            GameObject Stand = GameObject.Find("stand");
            GameObject Tower = GameObject.Find("tower");
            GameObject Player = GameObject.Find("Player");
            GameObject GlassSphere = GameObject.Find("GlassSphere");
            GameObject SaveCube = GameObject.Find("Saveing_Cube");

            Stand.SetActive(false);
            Tower.SetActive(false);
            GlassSphere.SetActive(false);
            Player.SetActive(false);
            SaveCube.SetActive(false);

            int counter;
            counter = molcounter.molecules.Count;
            List<Transform> MolCount;
            MolCount = molcounter.molecules;

            try
            {

                GameObject selfInstGO = Resources.Load("PDBs_To_Scale/Elastin/" + transformName) as GameObject;
                //GameObject selfInstGO = Resources.Load("PDBs_To_Scale/Elastin/Elastin_f60_pp") as GameObject;
                Transform selfInstTrans = selfInstGO.transform;

                // recover Langevin GOS gameobject list and append the spawned gameobjects
                Langevin_v2 Lange = GameObject.FindGameObjectWithTag("Physics_Sim").GetComponent<Langevin_v2>();

                Debug.Log("Running");

                int step = 0;
                while (step < repeat)
                {

                    //Vector3 Loc = new Vector3(UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f));
                    Vector3 Loc = new Vector3(0f,0f,0f);
                    Quaternion Rot = UnityEngine.Random.rotation;
                    Transform spawn = Instantiate<Transform>(selfInstTrans, Loc, Rot);
                    Debug.Log("Instantiated");
                    Rigidbody[] GOarray = spawn.gameObject.GetComponentsInChildren<Rigidbody>();
                    Lange.GOS.AddRange(GOarray);
                    MolCount.Add(spawn);

                    step++;
                }
            }

            catch (NullReferenceException e)
            {
                Debug.Log("transform not found, check name");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
