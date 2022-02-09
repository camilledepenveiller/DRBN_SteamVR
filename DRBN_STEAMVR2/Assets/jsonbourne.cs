using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Demonstration of generic List serialization using JsonUtility
/// </summary>
public class jsonbourne : MonoBehaviour
{
	/// <summary>
	/// Initialization
	/// </summary>
	void Start ()
	{
		string json = SerializeToJson();
		DeserializeFromJson(json);
	}


	/// <summary>
	/// Creates the list and serializes it to a Json string
	/// </summary>
	/// <returns>Serialized Json string</returns>
	string SerializeToJson()
	{
		List<SomeDataModel> dataList = new List<SomeDataModel>();

		dataList.Add(new SomeDataModel(0, "Entry 1"));
		dataList.Add(new SomeDataModel(1, "Entry 2"));
		dataList.Add(new SomeDataModel(2, "Entry 3"));

		ListContainer container = new ListContainer(dataList);

		// TODO: Wrap this in try/catch to handle serialization exceptions
		string json = JsonUtility.ToJson(container,true);

		// Outputs: {"dataList":[{"ID":0,"Name":"Entry 1"},{"ID":1,"Name":"Entry 2"},{"ID":2,"Name":"Entry 3"}]}
		Debug.Log(json);
		File.WriteAllText(Application.persistentDataPath + "/gamesave_list_test.jsonbrn",json);

		return json;
	}


	/// <summary>
	/// Deserializes the Json string into the list object
	/// </summary>
	/// <param name="json">Json string</param>
	void DeserializeFromJson(string json)
	{
		// TODO: Wrap this in try/catch to handle deserialization exceptions
		ListContainer container = JsonUtility.FromJson<ListContainer>(json);

		Debug.Log(container.dataList.Count);
		for (int i = 0; i < container.dataList.Count; i++)
		{
			Debug.Log("ID: " + container.dataList[i].ID + ", Name: " + container.dataList[i].Name);
		}
	}
}


/// <summary>
/// Container struct for the List
/// </summary>
public struct ListContainer
{
	public List<SomeDataModel> dataList;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="_dataList">Data list value</param>
	public ListContainer(List<SomeDataModel> _dataList)
	{
		dataList = _dataList;
	}
}


/// <summary>
/// Some generic data model, used for List entries
/// </summary>
[Serializable]
public struct SomeDataModel
{
	public int ID;
	public string Name;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="_ID">ID value</param>
	/// <param name="_Name">Name value</param>
	public SomeDataModel(int _ID, string _Name)
	{
		ID = _ID;
		Name = _Name;
	}
}