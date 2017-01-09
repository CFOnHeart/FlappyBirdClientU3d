using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToGame : MonoBehaviour
{
    public GameObject getReady;
    public GameObject tap;
    public GameObject waitForOther;
    private int isFinishLoadFrame;
    private const float DOWNSPEED = 10.0f;
    private Queue<int> que;
    bool flag = false;
    Thread thread;
    //ready start posy 1.392187 , end to 0.5416667
    // Use this for initialization
    void Start()
    {
        //init
        flag = false;
        que = new Queue<int>();
        que.Clear();
        getReady = GameObject.Find("UI Root/getready");
        tap = GameObject.Find("UI Root/tap");

        waitForOther = GameObject.Find("UI Root/waitOtherGamer");
        waitForOther.SetActive(false);
        tap.SetActive(false);
        if (getReady == null || tap == null)
        {
            Debug.LogError("Load Failure");
        }
        isFinishLoadFrame = 0;
      //  Debug.Log("current y is at " + getReady.transform.position.y.ToString());
        //   StartCoroutine(loadFrame());
        thread = new Thread(listenReceiveMessage);
        thread.Start();
        //  tap.SetActive(true);

    }

    // Update is called once per frame

    void Update()
    {
        if (que.Count > 0 || flag)
        {
            int value = que.Dequeue();
            thread.Abort();
            Debug.Log("Thread abort now game in next scene");
            SceneManager.LoadScene("game_scene");

        }
        if (isFinishLoadFrame == 0)
        {
            getReady.transform.position = new Vector3(getReady.transform.position.x, getReady.transform.position.y - DOWNSPEED / 1000, getReady.transform.position.z);
            // Debug.Log("current y is at " + getReady.transform.position.y.ToString());
            if (getReady.transform.position.y <= 0.5416667f)
            {
                isFinishLoadFrame = 1;
                tap.SetActive(true);
            }
        }
        else if (Input.GetMouseButtonDown(0) && isFinishLoadFrame == 1)
        {
            
            
         //   SceneManager.LoadScene("game_scene");
            isFinishLoadFrame = 2;
            ////点击了tap说明这个客户端进入了游戏
            string message = "014";
            //Debug.Log("");
            try
            {
                if (ServerConnect.instance == null)
                {
                    ServerConnect.instance = new ServerConnect();
                    ServerConnect.instance.connect();
                }

                ServerConnect.instance.sendMessage(message);
                //string mess = ServerConnect.instance.receiveMessage();

                //  Debug.Log("Tap to game's get server's message : " + mess);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.ToString());
            }
            waitForOther.SetActive(true);

          //  SceneManage.LoadScene("game scene");
        }


    }

    public void listenReceiveMessage()
    {
        while (true)
        {
            string mess = ServerConnect.instance.receiveMessage();
            Debug.Log("listenReceiveMessage is: " + mess);
            if (mess.CompareTo("game_start") == 0)
            {
                Debug.Log("I'm in QUEUE: " + mess);
                flag = true;
                que.Enqueue(1);
                break;
            }
        }
    }
}
