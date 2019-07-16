using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueScript : MonoBehaviour {

	// Use this for initialization
	public void GameContinue () {
		SceneManager.LoadScene("TitleScene");
	}
}
