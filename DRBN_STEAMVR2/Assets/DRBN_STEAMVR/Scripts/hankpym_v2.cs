using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* The problem seems to be that the physics system calculates the anchors and connected anchors at the start, but does not update them later, which is fine, as long as the scale is the same.

But when you scale it, the joint system stays at the original size thus the model falls apart.

What you need to do is save the anchor and connected anchor positions, and update it whenever you scale it. (It is in local space, so no need to scale it).

Don't forget to disable Auto Configure Connected Anchor!!  */

public class hankpym_v2 : MonoBehaviour {

	public Transform[] children;

    public List<Vector3> _connectedAnchor;
	public List<Vector3> _anchor;
    public List<int> _index;

    public Dictionary<int, Joint[]> _joints_dic = new Dictionary<int, Joint[]>();
    public Dictionary<int, Vector3[]> _connectedAnchor_dic = new Dictionary<int, Vector3[]>();
    public Dictionary<int, Vector3[]> _anchor_dic = new Dictionary<int, Vector3[]>();

    float maxscale;

	//void Start()
	//{
	//	children = transform.GetComponentsInChildren<Transform>();
 //       //_connectedAnchor = new Vector3[children.Length];
 //       //_anchor = new Vector3[children.Length];
 //       int childrenlen = 0;
 //       for (int i = 1; i < children.Length; i++)
	//	{
 //           if (children[i].GetComponents<Joint>().Count() != 0)
 //           {
 //               Debug.Log("i " + i + " children name " + children[i].name + " children joints " + children[i].GetComponents<Joint>().Count());
 //               _joints_dic.Add(i, children[i].GetComponents<Joint>());
 //               childrenlen += children[i].GetComponents<Joint>().Count();
 //           }
 //           else
 //           {
 //               Debug.Log("i " + i + " nothing to add, adding nothing");
 //               _joints_dic.Add(i, null);
 //               childrenlen += 1;
 //           }
 //       }
 //       Debug.Log("childrenlen" + childrenlen);
 //       _connectedAnchor = new Vector3[childrenlen-1];
 //       _anchor = new Vector3[childrenlen-1];
 //       _index = new int[childrenlen-1];

 //       int j = 1;
 //       int index = 1;
 //       while (j < _joints_dic.Count())
 //       {
 //           Debug.Log("j " + j);
 //           Debug.Log("joint" + _joints_dic[j]);

 //           int num_of_joints;
            

 //           if (_joints_dic[j] == null)
 //           {
 //               num_of_joints = 1;
 //               _index[j] = index;
 //               j++;
 //           }
 //           if (_joints_dic[j] != null)
 //           {
 //               num_of_joints = _joints_dic[j].Count();

 //               foreach (Joint Jo in _joints_dic[j])
 //               {
 //                   _index[j] = index;
 //                   _connectedAnchor[j] = Jo.connectedAnchor;
 //                   _anchor[j] = Jo.anchor;
 //                   j++;
 //               }
 //           }
 //           index++;
 //       }
	//}

    void Start()
    {
        children = transform.GetComponentsInChildren<Transform>();
        //_connectedAnchor = new Vector3[children.Length];
        //_anchor = new Vector3[children.Length];
        _connectedAnchor = new List<Vector3>();
        _anchor = new List<Vector3>();
        _index = new List<int>();

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
            //children[13].GetComponents<Joint>()[0].connectedAnchor = _connectedAnchor[13];
            //children[13].GetComponents<Joint>()[1].connectedAnchor = _connectedAnchor[14];
            //children[13].GetComponents<Joint>()[2].connectedAnchor = _connectedAnchor[15];

            //children[17].GetComponents<Joint>()[0].connectedAnchor = _connectedAnchor[19];

            //children[13].GetComponents<Joint>()[0].anchor = _anchor[13];
            //children[13].GetComponents<Joint>()[1].anchor = _anchor[14];
            //children[13].GetComponents<Joint>()[2].anchor = _anchor[15];

            //children[17].GetComponents<Joint>()[0].anchor = _anchor[19];
        }
    }
}
