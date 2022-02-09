using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glue : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void JoinGO(Rigidbody A,Rigidbody B){
		GameObject parentA = A.transform.parent.gameObject;
		GameObject parentB = B.transform.parent.gameObject;
		FixedJoint Gojoint = parentA.AddComponent<FixedJoint> ();

	}
}
