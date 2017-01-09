using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    public GameObject bird;
    public const float radius = 70; //point 6.4 , 84
    public  float PI = Mathf.Acos(1.0f);
    // Use this for initialization
	void Start () {
      //  StartCoroutine(birdCircleFly());
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startGame()
    {
        //  Application.LoadLevel("test1");
      //  connectServer();
        SceneManager.LoadScene("get_ready");
        
    }

    
}
