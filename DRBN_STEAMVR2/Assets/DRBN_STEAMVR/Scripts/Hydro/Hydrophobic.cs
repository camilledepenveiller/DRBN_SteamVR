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
	public Rigidbody RigidBody;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnDrawGizmos()
	{

	}
}
