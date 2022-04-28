using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HydrophobicWorld : MonoBehaviour
{
	public float Permeability = 0.05f;
	public float MaxForce = 10000.0f;
	public float floor =0;

	public bool UseScaleForDebugDraw;

	void Start()
	{

	}

	public Vector3 CalculateHydrophobic(Hydrophobic hydro1,Hydrophobic hydro2){
		var h1 = hydro1.transform.position;
		var h2 = hydro2.transform.position;
		//Debug.DrawLine (h1,h2,Color.yellow);
		var r = h2 - h1;
		var dist = r.magnitude;
		var D_0 = 1f;
		var radius = 2f; //2cm(?!!) in nature 300341a0.pdf
		var f = (0.14f * Mathf.Exp (dist / D_0)) * radius;

		return f*r.normalized;
	}

	public Vector3 CalculateHydrophobicPlan(Hydrophobic hydro1,float floor){
		var h1 = hydro1.transform.position;
		var h2 = new Vector3(h1[0],floor,h1[2]);
		//Debug.DrawLine (h1,h2,Color.cyan);
		var r = h2 - h1;
		var dist = r.magnitude;
		var D_0 = 1f;
		var radius = 2f; //2cm(?!!) in nature 300341a0.pdf
		var f = (0.14f * Mathf.Exp (dist / D_0)) * radius;

		return f*r.normalized;
	}

//	Vector3 CalculateGilbertForce(Hydrophobic hydro1, Hydrophobic hydro2)
//	{
//		var h1 = hydro1.transform.position;
//		var h2 = hydro2.transform.position;
//		var r = h2 - h1;
//		var dist = r.magnitude;
//		var part0 = Permeability * hydro1.HydroForce * hydro2.HydroForce;
//		var part1 = 4 * Mathf.PI * dist;
//
//		var f = (part0 / part1);
//
//		if (hydro1.HydroPole == hydro2.HydroPole)
//			f = -f;
//
//		return f * r.normalized;
//	}

	void FixedUpdate()
	{
		var hydros = FindObjectsOfType<Hydrophobic>();
		var hydroCount = hydros.Length;

		for (int i = 0; i < hydroCount; i++)
		{
			var h1 = hydros[i];
			if (h1.RigidBody == null)
				continue;

			var rb1 = h1.RigidBody;
			var accF1 = Vector3.zero;
			var accF2 = Vector3.zero;
			for (int j = 0; j < hydroCount; j++)
			{
				if (i == j)
					continue;

				var h2 = hydros[j];

//				if (h2.HydroForce < 5.0f)
//					continue;

				if (h1.transform.parent == h2.transform.parent)
					continue;

				//var f = CalculateGilbertForce(h1, h2);
				var f = CalculateHydrophobic(h1, h2);
				//var f= new Vector3(0f,0f,0f);
				var fp = CalculateHydrophobicPlan(h1, floor);
				var hydroForce = h1.HydroForce * h2.HydroForce;

				accF1 += f * hydroForce;
			}

			if (accF1.magnitude > MaxForce)
			{
				accF1 = accF1.normalized * MaxForce;
			}
			rb1.AddForceAtPosition(accF1, h1.transform.position);
		}
	}

	//void OnDrawGizmos()
	//{
	//	var hydros = FindObjectsOfType<Hydrophobic>();
	//	var hydroCount = hydros.Length;
	//	var randPts = new List<Vector3>();

	//	for (int i = 0; i < 100; i++)
	//	{
	//		var unitPt = Random.insideUnitSphere;

	//	}

 //       if (Selection.activeTransform == null)
 //           return;
 //       var selectedhydros = Selection.activeTransform.gameObject.GetComponentsInChildren<Hydrophobic>();
 //       if (selectedhydros.Length == 0 || selectedhydros.Length > 20)
 //           return;
 //       for (int i = 0; i < selectedhydros.Length; i++)
 //       {
 //           var h1 = selectedhydros[i];
 //           var scale1 = 0.35f / 0.5f;
 //           if (UseScaleForDebugDraw)
 //               scale1 *= h1.transform.parent.lossyScale.x * (h1.HydroForce / 50.0f);
 //           if (h1.HydroPole == Hydrophobic.Hydro.phobic)
 //           {
 //               Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.25f);
 //               Gizmos.DrawSphere(h1.transform.position, scale1);

 //           }
 //           else
 //           {
 //               Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.25f);
 //               Gizmos.DrawSphere(h1.transform.position, scale1);

 //           }

 //           for (int j = 0; j < hydroCount; j++)
 //           {
 //               var h2 = hydros[j];

 //               if (h1 == h2)
 //                   continue;

 //               if (h2.HydroForce < 5.0f)
 //                   continue;

 //               if (h1.transform.parent == h2.transform.parent)
 //                   continue;

 //               //var f = CalculateGilbertForce(h1, h2);
 //               var f = CalculateHydrophobic(h1, h2);
 //               var fp = CalculateHydrophobicPlan(h1, floor);

 //               if (h2.HydroPole == Hydrophobic.Hydro.phobic)
 //               {
 //                   Gizmos.color = Color.cyan;
 //               }
 //               else
 //               {
 //                   Gizmos.color = Color.red;
 //               }

 //               Gizmos.DrawLine(h1.transform.position, h1.transform.position + f);
 //           }
 //       }
 //   }
}
