using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolScale : MonoBehaviour {
	//scaling part of the code 
	Vector3 minScale;
	public Vector3 maxScale;
	public float speed = 2f;
	public float duration = 50f;

	// Use this for initialization
	IEnumerator Start () {
		minScale = transform.localScale;
		maxScale = new Vector3(0.001f,0.001f,0.001f);
		yield return ScaleLerp (minScale, maxScale, duration);
	}

	public IEnumerator ScaleLerp(Vector3 a, Vector3 b, float time){
		float i = 0.0f;
		//float rate = (1.0f / time) * speed; //trying a slower rate 
        float rate = (0.5f / time) * speed;
        while (i < 1.0f){
			i += Time.deltaTime * rate;
			transform.localScale = Vector3.Lerp (a, b, i);
			yield return null;
		}
	}
}
