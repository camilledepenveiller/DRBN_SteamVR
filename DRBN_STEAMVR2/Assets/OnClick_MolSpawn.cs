using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
	public class OnClick_MolSpawn : MonoBehaviour
	{
		public Transform prefab;
		private Transform spawn;

		public void SpawnMolecule()
		{
			var rot = gameObject.transform.rotation;
			var loc = gameObject.transform.position;
			spawn = Instantiate (prefab, loc, rot);
			Debug.Log ("plop");
		}
	}
}