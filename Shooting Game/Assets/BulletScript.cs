using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class BulletScript : MonoBehaviour {

	public GameObject BulletPrefab;
	//private Ray ray;
	private float power = 100f;
	private bool shoot;
	private float timeLimit = 3;
	GameObject Bullet;
	public float score = 0;
	public float totalScore = 0;
	public float bulletCount = 0;
	public float totalTime = 30;
	GameObject timeText;
	Text time_text;
	bool gameOverFlag;
	GameObject ResultPanel;
	GameObject ClearPanel;
	Text clear_text;
	public bool clearFlag;
	public GameObject obj;
	public GameObject monster;

	static UdpClient udp;
	IPEndPoint remoteEP = null;
	int i = 0;
	private float clear_time;

	void Awake() {
		ResultPanel = GameObject.Find("ResultPanel");
		ResultPanel.SetActive(false);
		ClearPanel = GameObject.Find("ClearPanel");
		ClearPanel.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		timeText = GameObject.Find("Time");
		time_text = timeText.GetComponent<Text>();

		monster = GameObject.FindWithTag("gomi");

		gameOverFlag = false;
		clearFlag = false;

		int LOCA_LPORT = 5000;
		udp = new UdpClient(LOCA_LPORT);
		udp.Client.ReceiveTimeout = 10;
	}

	// Update is called once per frame
	void Update () {
		IPEndPoint remoteEP = null;
		try{
			byte[] data = udp.Receive(ref remoteEP);
			string text = Encoding.UTF8.GetString(data);
			Debug.Log(text);
			if(int.Parse(text) == 0){
				Bullet = GameObject.Instantiate(BulletPrefab);
				shoot = true;
			}else if(int.Parse(text) == 1){
				obj = GameObject.FindWithTag("Red");
				Debug.Log(obj.name);
				Destroy(obj);
				shoot = true;
			}else if(int.Parse(text) == 2){
				obj = GameObject.FindWithTag("Blue");
				Debug.Log(obj.name);
				Destroy(obj);
				shoot = true;
			}else if(int.Parse(text) == 3){
				obj = GameObject.FindWithTag("Yellow");
				Debug.Log(obj.name);
				Destroy(obj);
				shoot = true;
			}
		}catch{
			shoot = false;
		}
		Destroy(Bullet, timeLimit);

		if(clearFlag == true){
			clear_time = totalTime;
			gameOverFlag = true;
			ClearPanel.SetActive(true);
			clear_text = GameObject.Find("TimeText").GetComponent<Text>();
		}else if(totalTime > 0){
			totalTime -= Time.deltaTime;
			time_text.text = "TIME " + totalTime.ToString("f2");
		}else if(gameOverFlag == false){
			gameOverFlag = true;
			totalTime = 0;
			time_text.text = "TIME 0";
			ResultPanel.SetActive(true);

			if(bulletCount > 0){
				float bonus = score * (score / bulletCount) * 300;
				totalScore = (score * 300 + bonus);
			}
		}
	}

	void FixedUpdate () {
		if(shoot){
			if(Bullet != null){
				Bullet.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);

				bulletCount ++;
				shoot = false;
			}
		}
	}

}
