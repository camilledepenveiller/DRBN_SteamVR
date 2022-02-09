using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keep_orientation : MonoBehaviour
{
	Transform follow;

	// Start is called before the first frame update
    void Start()
    {
		//find parent
		follow = transform.parent;
		//unparent
		transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
		//instead of canceling rotations (headache) just make the cube follow the position of the (ex)parent (works yeah!)
		transform.position = follow.transform.position;

		//transform.rotation = Quaternion.Euler(0f,0f,0f);//Vector3 (0f,0f,0f);
		//transform.LookAt(new Vector3 (0,100000,0));//Vector3 (0f,0f,0f);
		//transform.LookAt(new Vector3 (0,0,100000));//Vector3 (0f,0f,0f);
    }
}
