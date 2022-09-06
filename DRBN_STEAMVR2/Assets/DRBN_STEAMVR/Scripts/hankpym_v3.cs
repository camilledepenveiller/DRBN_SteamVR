using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* The problem seems to be that the physics system calculates the anchors and connected anchors at the start, but does not update them later, which is fine, as long as the scale is the same.

But when you scale it, the joint system stays at the original size thus the model falls apart.

What you need to do is save the anchor and connected anchor positions, and update it whenever you scale it. (It is in local space, so no need to scale it).

Don't forget to disable Auto Configure Connected Anchor!!  */

/* This version tries to adress instability problems with some models by disabling the rigid body colliders during the scaling and enabling them afterwards
 
 */

public class hankpym_v3 : MonoBehaviour {

	public Transform[] children;
    public Collider[] childrenColliders;

    public List<Vector3> _connectedAnchor;
	public List<Vector3> _anchor;
    public List<int> _index;

    public Dictionary<int, Joint[]> _joints_dic = new Dictionary<int, Joint[]>();
    public Dictionary<int, Vector3[]> _connectedAnchor_dic = new Dictionary<int, Vector3[]>();
    public Dictionary<int, Vector3[]> _anchor_dic = new Dictionary<int, Vector3[]>();

    

    float maxscale;

    void Start()
    {
        children = transform.GetComponentsInChildren<Transform>();

        childrenColliders = transform.GetComponentsInChildren<Collider>();

        //_connectedAnchor = new Vector3[children.Length];
        //_anchor = new Vector3[children.Length];
        _connectedAnchor = new List<Vector3>();
        _anchor = new List<Vector3>();
        _index = new List<int>();

        //disable all colliders on start
        for (int i = 0; i < childrenColliders.Length; i++)
        { 
            if (childrenColliders[i].enabled == true)
            {
                childrenColliders[i].enabled = !childrenColliders[i].enabled;
            }
        }


        for (int i = 0; i < children.Length; i++)
        {
            
            
            if (children[i].GetComponents<Joint>().Count() != 0)
            {
                //children[i].GetComponent<Joint>().autoConfigureConnectedAnchor = false; // /!\ script HankPym.cs will give bad results if Connected Anchor is auto configured
                _joints_dic.Add(i, children[i].GetComponents<Joint>());
            }
            else
            {
                //Debug.Log("i " + i + " nothing to add, adding nothing");
                _joints_dic.Add(i, null);
            }
        }
        
        foreach(var item in _joints_dic)
        {
            //Debug.Log("Key " + item.Key);
            //Debug.Log("Value " + item.Value);
            if (item.Value == null)
            {
                _connectedAnchor.Add(Vector3.zero);
                _anchor.Add(Vector3.zero);
                _index.Add(item.Key);
            }

            if (item.Value != null)
            {
                foreach (var elt in item.Value)
                {
                    //Debug.Log(elt);
                    _connectedAnchor.Add(elt.connectedAnchor);
                    _anchor.Add(elt.anchor);
                    _index.Add(item.Key);
                }
            }
        }
        //Debug.Log("_connectedAnchor.Count "+ _connectedAnchor.Count);
        //Debug.Log("_index.Count "+ _index.Count);
    }

    private void Update()
    {
        //check gameobject size and disable hankpym script if gameobject size is superior or equal to final size in 
        //

        MolScale thisGOscale = this.GetComponent<MolScale>();
        maxscale = thisGOscale.maxScale.x;
        if (this.transform.localScale.x >= maxscale) {
            enabled = false;

            //enable all colliders on scaling end
            for (int i = 0; i < childrenColliders.Length; i++)
            {
                if (childrenColliders[i].enabled == false)
                {
                    childrenColliders[i].enabled = true;
                }
            }

        }

        for (int index = 1; index < _index.Count; index++)
        {
            //Debug.Log("_index[index] " + _index[index] + " || " + "index" + index);

            Joint[] RBJoint = children[_index[index]].GetComponents<Joint>();
                        
            if (RBJoint!=null)
            {
                for (int component = 0; component < RBJoint.Length; component++)
                {
                    RBJoint[component].connectedAnchor = _connectedAnchor[index];
                    RBJoint[component].anchor = _anchor[index];
                }
            }
        }
    }
}
