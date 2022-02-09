using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrostatic : MonoBehaviour
{
	public enum Charge
	{
		plus,
		minus
	}

	public float ChargeForce;
	public Charge Pole;
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
