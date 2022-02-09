using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hankpym : MonoBehaviour {

	public Transform[] children;
	public Vector3[] _connectedAnchor;
	public Vector3[] _anchor;
	float maxscale ;
	void Start()
	{
		children = transform.GetComponentsInChildren<Transform>();
		_connectedAnchor = new Vector3[children.Length];
		_anchor = new Vector3[children.Length];
		for (int i = 1; i < children.Length; i++)
		{
			if (children[i].GetComponent<Joint>() != null)
			{
				children[i].GetComponent<Joint>().autoConfigureConnectedAnchor = false; // /!\ script HankPym.cs will give bad results if Connected Anchor is auto configured
				_connectedAnchor[i] = children[i].GetComponent<Joint>().connectedAnchor;
				_anchor[i] = children[i].GetComponent<Joint>().anchor;
			}
		}
	}
	private void Update()
	{
		//check gameobject size and disable hankpym script if gameobject size is superior or equal to final size in 
		//

		MolScale thisGOscale = this.GetComponent<MolScale> ();
		maxscale = thisGOscale.maxScale.x;
		if (this.transform.localScale.x>=maxscale) {
			enabled = false;
		}

		for (int i = 1; i < children.Length; i++)
		{
            //Debug.Log("index " + i + " name " + children[i].name);
            if (children[i].GetComponent<Joint>() != null)
			{
                //Debug.Log("Doing something");
                children[i].GetComponent<Joint>().connectedAnchor = _connectedAnchor[i];
				children[i].GetComponent<Joint>().anchor = _anchor[i];
			}
		}
	}
}
