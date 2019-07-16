using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAction : MonoBehaviour {

	GameObject BulletScript;
	BulletScript bulletScript;

	// Use this for initialization
	void Start () {
		this.gameObject.transform.position = new Vector3(0, 5, 120);//start position

		BulletScript = GameObject.Find("BulletScript");
		bulletScript = BulletScript.GetComponent<BulletScript>();
	}

	// Update is called once per frame
	void Update () {
    this.gameObject.transform.Translate(0, 0, 1);
    if(this.gameObject.transform.position.z < 50){
      bulletScript.totalTime = 0;
    }
	}
}
