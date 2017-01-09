using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void login()
    {
        string username = GameObject.Find("UI Root/user_input").GetComponent<UIInput>().value;
        string password = GameObject.Find("UI Root/pwd_input").GetComponent<UIInput>().value;
        int len = username.Length + password.Length + 2;
        string message = "1" + username + "&" + password;
        if (len >= 10) message = len.ToString() + message;
        else message = "0" + len.ToString() + message;
        try
        {
            //ServerConnect serverConnect = new ServerConnect();
            //serverConnect.connect();
            //serverConnect.sendMessage(message);
            //string mess = serverConnect.receiveMessage();
            if (ServerConnect.instance == null)
            {
                ServerConnect.instance = new ServerConnect();
                ServerConnect.instance.connect();
            }
            
            ServerConnect.instance.sendMessage(message);
            string mess = ServerConnect.instance.receiveMessage();
            Debug.Log("loging here: " + mess+" " + message);
            int score;
            if((score = int.Parse(mess)) >= 0) {
                PipeScript.best_score = score;
                SceneManager.LoadScene("start_scene");
            }
          //  serverConnect.close();
        }
        catch (Exception ex)
        {
            Debug.Log("login errot : "+ex.ToString());
        }
        finally
        {

        }
    }
    //这里实现的是直接注册成功后就用当前注册的账号登录游戏
    public void register()
    {
        string username = GameObject.Find("UI Root/user_input").GetComponent<UIInput>().value;
        string password = GameObject.Find("UI Root/pwd_input").GetComponent<UIInput>().value;
        int len = username.Length + password.Length + 2;
        string message = "3" + username + "&" + password;
        if (len >= 10) message = len.ToString() + message;
        else message = "0" + len.ToString() + message;
        try
        {
            //ServerConnect serverConnect = new ServerConnect();
            //serverConnect.connect();
            //serverConnect.sendMessage(message);
            //string mess = serverConnect.receiveMessage();
            if (ServerConnect.instance == null)
            {
                ServerConnect.instance = new ServerConnect();
                ServerConnect.instance.connect();
            }
            
            ServerConnect.instance.sendMessage(message);
            string mess = ServerConnect.instance.receiveMessage();
            string b = "success";
            
            Debug.Log("Register: get the information-----" + mess+" "+ b);
            if (mess.CompareTo(b) == 0)
            {
                SceneManager.LoadScene("start_scene");
            }
            PipeScript.best_score = 0;
            //  serverConnect.close();
        }
        catch (Exception ex)
        {
            Debug.Log("register Exception: "+ex.ToString());
        }
        finally
        {

        }
    }
}
