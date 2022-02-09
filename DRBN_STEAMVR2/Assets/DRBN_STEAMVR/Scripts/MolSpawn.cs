using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolSpawn : MonoBehaviour {
	//scaling part of the code 
	Vector3 minScale;
	public Vector3 maxScale;
	public float speed = 2f;
	public float duration = 5f;

	public Transform prefab;
	private Transform spawn;
    public GameObject spawnpoint;


	//Rigidbody[] children;

	// code used for UI purpose
	public void SpawnMolecule()
	{
        int counter;
        counter = molcounter.molecules.Count;
        List<Transform> MolCount;
        MolCount = molcounter.molecules;

        if (counter < molcounter.limit)
        {
            //Vector3 loc = gameObject.transform.position;
            Vector3 loc = spawnpoint.transform.position;
            Quaternion rot = gameObject.transform.rotation;
            spawn = Instantiate(prefab, loc, rot);
            spawn.name = prefab.name+"_"+counter.ToString();
            Debug.Log("plop");

            // recover Langevin GOS gameobject list and append the spawned gameobjects
            Langevin_v2 Lange = GameObject.FindGameObjectWithTag("Physics_Sim").GetComponent<Langevin_v2>();

            // add line for mol counter
            //List<Transform> MolCount = GameObject.Find("Simulation").GetComponent<molcounter>().molecules;

            Debug.Log("Before " + Lange.GOS.Count);
            //Lange.GOS.Add(
            Debug.Log(spawn.transform.name);
            Rigidbody[] GOarray = spawn.gameObject.GetComponentsInChildren<Rigidbody>();
            Lange.GOS.AddRange(GOarray);
            MolCount.Add(spawn);
            Debug.Log("After  " + Lange.GOS.Count);
            Debug.Log("counter " + counter);
        }
	}




	// uncomment below if using the function for non UI purpose (testing debugs etc)
//	void Update () {
//		Vector3 loc = gameObject.transform.position;
//		Quaternion rot = gameObject.transform.rotation;
//
//		if (Input.GetKeyDown ("space")) {
//			spawn = Instantiate (prefab, loc, rot);
//			Vector3 currScale = spawn.transform.localScale;
//
//			children = spawn.GetComponentsInChildren<Rigidbody>();
//		}
//	}
}
