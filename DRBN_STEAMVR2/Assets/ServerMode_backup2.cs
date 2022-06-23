using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ServerMode_backup2 : MonoBehaviour
{

    public string savepath = ""; // make a variable that can be checked by other functions like SaveSnapShot
    //public string transformName = "Elastin_f60_pp"; // replace by command line argument later
    public string transformName = "Elastin_f4743_pp"; 
    public int repeat = 10; // number of molecules in the simulation, replace by command line argument later
                    //UnityEngine.Random rnd = new UnityEngine.Random();

    // Start is called before the first frame update
    void Start()
    {
        ////comment for now, wait until we have a functional instantiation in normal game mode
        if (Application.isBatchMode)
        {
            Debug.Log("In BatchMode!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");



            ///get arguments here 
            string[] args = System.Environment.GetCommandLineArgs();

            Debug.Log("D:/DRBN_VR_DepFork/DRBN_SteamVR/DRBN_STEAMVR2/Assets/Resources/" + "PDBs_To_Scale/Elastin" + args[1] + ".prefab");
            Debug.Log(System.IO.File.Exists("D:/DRBN_VR_DepFork/DRBN_SteamVR/DRBN_STEAMVR2/Assets/Resources/" + "PDBs_To_Scale/Elastin" + args[1] + ".prefab") + " GO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            //Debug.Log(args[1]!=null);
            ////Debug.Log(args.Length);

            //for(int i=0; i <= args.Length; i++)
            //{
            //    Debug.Log(args[i] +" " + i);
            //}

            //if (args[1] != null)
            //{
                transformName = args[1];
            //}

            //if (args[2] != null)
            //{
            //    repeat = Int32.Parse(args[2]);
            //}


            //Debug.Log(" args 0 " + args[0]);
            //Debug.Log(" args 1 " + args[1]);
            //Debug.Log("transformName " + transformName);
            //Debug.Log(" args 2 " + args[2]);

            Debug.Log(transformName + " transformName");

            GameObject Stand = GameObject.Find("stand");
            GameObject Tower = GameObject.Find("tower");
            GameObject Player = GameObject.Find("Player");
            GameObject GlassSphere = GameObject.Find("GlassSphere");
            GameObject SaveCube = GameObject.Find("Saveing_Cube");
            GameObject CubicContainer = GameObject.Find("CC");

            Stand.SetActive(false);
            Tower.SetActive(false);
            GlassSphere.SetActive(false);
            Player.SetActive(false);
            //SaveCube.SetActive(false);
            //ignominous cheating because Unity is too stupid to find inactive gameobjects, CC is outside the players view in VR, and teleported back to (0,0,0) for batch simulations
            CubicContainer.transform.position = new Vector3(0,0,0);

            int counter;
            counter = molcounter.molecules.Count;
            List<Transform> MolCount;
            MolCount = molcounter.molecules;

            Debug.Log("Prepare to run!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //try
            {
                //GameObject selfInstGO = Resources.Load("PDBs_To_Scale/Elastin/" + transformName) as GameObject;
                GameObject selfInstGO = Resources.Load<GameObject>("PDBs_To_Scale/Elastin/" + transformName);

                
                Debug.Log(System.IO.File.Exists("D:/DRBN_VR_DepFork/DRBN_SteamVR/DRBN_STEAMVR2/Assets/Resources/" + "PDBs_To_Scale/Elastin/" + transformName + ".prefab") + " GO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.Log(Application.dataPath + "/PDBs_To_Scale/Elastin/" + transformName);
                Debug.Log(selfInstGO.name + " GO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                //GameObject selfInstGO = Resources.Load("PDBs_To_Scale/Elastin/Elastin_f60_pp") as GameObject;
                Transform selfInstTrans = selfInstGO.transform;

                // recover Langevin GOS gameobject list and append the spawned gameobjects
                Langevin_v2 Lange = GameObject.FindGameObjectWithTag("Physics_Sim").GetComponent<Langevin_v2>();

                Debug.Log("Running!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                int step = 0;
                while (step < repeat)
                {

                    //Vector3 Loc = new Vector3(UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f));
                    Vector3 Loc = new Vector3(0f,0f,0f);
                    Quaternion Rot = UnityEngine.Random.rotation;
                    Transform spawn = Instantiate<Transform>(selfInstTrans, Loc, Rot);
                    Debug.Log("Instantiated!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + step +" step " + repeat + " repeat");
                    Rigidbody[] GOarray = spawn.gameObject.GetComponentsInChildren<Rigidbody>();
                    Lange.GOS.AddRange(GOarray);
                    MolCount.Add(spawn);

                    step++;
                }

                Debug.Log("GOS " + Lange.GOS.Count + " !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }

            //catch (NullReferenceException e)
            //{
            //    Debug.Log("transform not found, check name");
            //}

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
