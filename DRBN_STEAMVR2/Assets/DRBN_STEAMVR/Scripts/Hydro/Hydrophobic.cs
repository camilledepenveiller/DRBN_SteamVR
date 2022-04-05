using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrophobic : MonoBehaviour
{
	public enum Hydro
	{
		phobic,
		philic
	}

	public float HydroForce;
	public Hydro HydroPole;
	public Rigidbody RigidBody; //maybe try to automate the process by taking THIS game object? //done in line 23
	

	// Use this for initialization
	void Start ()
	{
		Hydrophobic HP = new Hydrophobic();
		Debug.Log(gameObject.transform.parent.name);
		RigidBody = gameObject.transform.parent.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnDrawGizmos()
	{

	}
}
