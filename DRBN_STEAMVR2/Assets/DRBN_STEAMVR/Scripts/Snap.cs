using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	//when interact stop : create a joint


	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.GetComponent<Collider>().name == "osph" | other.gameObject.GetComponent<Collider>().name == "isph")
		{
			Debug.Log("oy!");
			Vector3 snaptopos = other.gameObject.GetComponent<Collider>().transform.position;
			Vector3 parentfrom = this.gameObject.transform.parent.transform.position;
			Vector3 snapfrom = this.gameObject.transform.position;//this.GetComponent<Collider>().transform.position;
			Vector3 d = (snapfrom - parentfrom);
			this.gameObject.transform.parent.GetComponent<Collider>().enabled = false;
			this.gameObject.transform.parent.transform.position = Vector3.MoveTowards(snapfrom-d, snaptopos-d, 10000f);
//			other.gameObject.transform.parent.gameObject.AddComponent<ConfigurableJoint>();
//			other.gameObject.GetComponent<ConfigurableJoint>().connectedBody=this.GetComponent<Rigidbody>(); //commented the two lines because it does not work. Check glue.cs instead.
		}
	}

	//	private void OnTriggerStay(Collider other)
//	{
//		if (other.gameObject.GetComponent<Collider>().name == "osph" | other.gameObject.GetComponent<Collider>().name == "isph")
//		{
//			Debug.Log("oy!");
//			Vector3 snaptopos = other.gameObject.GetComponent<Collider>().transform.position;
//			Vector3 parentfrom = this.gameObject.transform.parent.transform.position;
//			Vector3 snapfrom = this.gameObject.transform.position;//this.GetComponent<Collider>().transform.position;
//			Vector3 d = (snapfrom - parentfrom);
//			//Debug.Log("snap to " + snaptopos + " snap from " + snapfrom + " distance " + Vector3.Distance(snaptopos,snapfrom));
//			//Debug.Log("parent position " + parentfrom);
//			//this.gameObject.transform.position = Vector3.MoveTowards(snapfrom,snaptopos,0.1f);
//			this.gameObject.transform.parent.GetComponent<Collider>().enabled = false;
//			this.gameObject.transform.parent.transform.position = Vector3.MoveTowards(snapfrom-d, snaptopos-d, 10000f);
//			//this.gameObject.transform.parent.SetParent(other.gameObject.transform.parent);
//			//if (this.gameObject.GetComponent<ConfigurableJoint>() == null)
//			//{
//			//    this.gameObject.AddComponent<ConfigurableJoint>();
//			//    var otherBody = other.gameObject.GetComponent<Rigidbody>();
//			//}
//		}
//	}
}