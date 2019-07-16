using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour {

	public GameObject slimeGreenPrefab;
	public GameObject slimeGreen;
	public GameObject redPrefab;
	public GameObject red;
	public GameObject bluePrefab;
	public GameObject blue;
	public GameObject yellowPrefab;
	public GameObject yellow;
	GameObject BulletScript;
	BulletScript bulletScript;

	private float counter;
	private float countLimit = 4;

	// Use this for initialization
	void Start () {
		BulletScript = GameObject.Find("BulletScript");
		bulletScript = BulletScript.GetComponent<BulletScript>();
		counter = countLimit - 1;
	}

	// Update is called once per frame
	void Update () {
		if(bulletScript.totalTime > 0 && slimeGreen == null){
			slimeGreen = Instantiate(slimeGreenPrefab) as GameObject;
		}

		counter += Time.deltaTime;

		if(counter >= countLimit){
			counter = 0;
			int n = Random.Range(1, 4);
			if(n == 1){
				red = Instantiate(redPrefab) as GameObject;
			}
			if(n == 2){
				blue = Instantiate(bluePrefab) as GameObject;
			}
			if(n == 3){
				yellow = Instantiate(yellowPrefab) as GameObject;
			}
		}
	}
}
