using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletAction : MonoBehaviour {

	private float point = 1;
	GameObject BulletScript;
	BulletScript bulletScript;

	// Use this for initialization
	void Start () {

		BulletScript = GameObject.Find("BulletScript");
		bulletScript = BulletScript.GetComponent<BulletScript>();
	}

	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "gomi")
		{
			Destroy(collision.gameObject);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.GetComponent<ParticleSystem>().Play();
			this.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
			Destroy(this.gameObject, 1);

			bulletScript.score += point;
			bulletScript.clearFlag = true;
			bulletScript.totalScore = bulletScript.score * 300;
		}
	}
}
