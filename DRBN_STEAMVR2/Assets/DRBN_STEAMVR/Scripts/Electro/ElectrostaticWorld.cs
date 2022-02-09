using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ElectrostaticWorld : MonoBehaviour
{
	//public float Permeability = 0.05f;
	public float k = 7.08f; // *80 for water dielectric constant *10e9 (pour passer en nm)
	public float MaxForce = 10000.0f;

	public bool UseScaleForDebugDraw;

	void Start()
	{

	}

	Vector3 Calculateelectrostatic(Electrostatic charge1,Electrostatic charge2){
		var m1 = charge1.transform.position;
		var m2 = charge2.transform.position;
		var r = m2 - m1;
		var dist = r.magnitude;
		//var part0 = Permeability * charge1.ChargeForce * charge2.ChargeForce;
		var part0 = k*(charge1.ChargeForce * charge2.ChargeForce);
		var part1 = dist*dist;

		var f = (part0 / part1);
		Debug.Log ("k "+ k);
		Debug.Log ("part0 " + part0 + " part1 " + part1);
		Debug.Log (charge1.ChargeForce + " " + charge2.ChargeForce + " " + f);

		if (charge1.Pole == charge2.Pole)
			f = -f;

		return f * r.normalized;
	}

	//	Vector3 CalculateGilbertForce(electrostatic charge1, electrostatic charge2)
	//	{
	//		var c1 = charge1.transform.position;
	//		var c2 = charge2.transform.position;
	//		var r = c2 - c1;
	//		var dist = r.magnitude;
	//		var part0 = Permeability * charge1.electroForce * charge2.electroForce;
	//		var part1 = 4 * Mathf.PI * dist;
	//
	//		var f = (part0 / part1);
	//
	//		if (charge1.electroPole == charge2.electroPole)
	//			f = -f;
	//
	//		return f * r.normalized;
	//	}

	void FixedUpdate()
	{
		var electros = FindObjectsOfType<Electrostatic>();
		var electroCount = electros.Length;

		for (int i = 0; i < electroCount; i++)
		{
			var c1 = electros[i];
			if (c1.RigidBody == null)
				continue;

			var rb1 = c1.RigidBody;
			var accF1 = Vector3.zero;
			var accF2 = Vector3.zero;
			for (int j = 0; j < electroCount; j++)
			{
				if (i == j)
					continue;

				var c2 = electros[j];

				//				if (c2.electroForce < 5.0f)
				//					continue;

				if (c1.transform.parent == c2.transform.parent)
					continue;

				//var f = CalculateGilbertForce(c1, c2);
				var f = Calculateelectrostatic(c1, c2);
				var electroForce = c1.ChargeForce * c2.ChargeForce;

				accF1 += f * electroForce;
			}

			if (accF1.magnitude > MaxForce)
			{
				accF1 = accF1.normalized * MaxForce;
			}
			rb1.AddForceAtPosition(accF1, c1.transform.position);
		}
	}

	void OnDrawGizmos()
	{
		var electros = FindObjectsOfType<Electrostatic>();
		var electroCount = electros.Length;
		var randPts = new List<Vector3>();

		for (int i = 0; i < 100; i++)
		{
			var unitPt = Random.insideUnitSphere;

		}

		//if (Selection.activeTransform == null)
		//	return;
		//var selectedelectros = Selection.activeTransform.gameObject.GetComponentsInChildren<Electrostatic>();
		//if (selectedelectros.Length == 0 || selectedelectros.Length > 20)
		//	return;
//		for (int i = 0; i < selectedelectros.Length; i++)
//		{
//			var c1 = selectedelectros[i];
//			var scale1 = 0.35f / 0.5f;
//			if (UseScaleForDebugDraw)
//				scale1 *= c1.transform.parent.lossyScale.x * (c1.ChargeForce / 50.0f);
//			if (c1.Pole == Electrostatic.Charge.plus)
//			{
//				Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.25f);
//				Gizmos.DrawSphere(c1.transform.position, scale1);

//			}
//			else
//			{
//				Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.25f);
//				Gizmos.DrawSphere(c1.transform.position, scale1);

//			}

//			for (int j = 0; j < electroCount; j++)
//			{
//				var c2 = electros[j];

//				if (c1 == c2)
//					continue;

////				if (c2.ChargeForce < 5.0f)
////					continue;

//				if (c1.transform.parent == c2.transform.parent)
//					continue;

//				//var f = CalculateGilbertForce(c1, c2);
//				var f = Calculateelectrostatic(c1, c2);

//				if (c2.Pole == Electrostatic.Charge.plus)
//				{
//					Gizmos.color = Color.cyan;
//				}
//				else
//				{
//					Gizmos.color = Color.red;
//				}

//				Gizmos.DrawLine(c1.transform.position, c1.transform.position + f);
//			}
//		}
	}
}
