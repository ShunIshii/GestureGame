using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAction : MonoBehaviour {

  private int cnt = 0;
	// Use this for initialization
	void Start () {
		this.gameObject.transform.position = new Vector3(-12, 4, 120);//start position
	}

	// Update is called once per frame
	void Update () {
    if(cnt < 25){
		  this.gameObject.transform.Translate(-1, 0, 0);
    }else{
      this.gameObject.transform.Translate(1, 0, 0);
    }

    if(cnt == 49){
      cnt = 0;
    }else{
      cnt++;
    }

    if(this.gameObject.transform.position.z < 50){
      Destroy(this.gameObject);
    }
	}
}
