using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour {

	// Use this for initialization
	public void GameStart () {
		SceneManager.LoadScene("GameScene");
	}
}
