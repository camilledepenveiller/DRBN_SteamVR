using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Valve.VR.InteractionSystem.Sample
{
	public class OnClick_SaveSnapShot : MonoBehaviour
	{
		public GameObject savecube; 

		void Awake()
		{
			savecube = GameObject.Find("Saveing_Cube");
		}

		public void SaveMolecule()
		{
//			SaveLoad save = SaveSnapShot();
//
//			BinaryFormatter bf = new BinaryFormatter ();
//
//			FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
//			bf.Serialize(file, save);
//			file.Close();
//
//			Debug.Log("Game Saved");
		}
	}	
}

