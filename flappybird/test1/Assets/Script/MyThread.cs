using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Script
{
    public class MyThread
    {
        public static Thread mythread = null;

        public MyThread()
        {
            
        }

        public static void initThread()
        {
            mythread = new Thread(uploadPosY);
            mythread.Start();
        }
        //制作一个线程上传当前鸟的pos y的message
        public static void uploadPosY()
        {
            while (PipeScript.isEnd == false)
            {
                Debug.Log("we are in the new thread ");
                try
                {
                    if (ServerConnect.instance == null)
                    {
                        ServerConnect.instance = new ServerConnect();
                        ServerConnect.instance.connect();
                    }
                    int value = (int)(PipeScript.bird.transform.position.y*100);
                    string message = value.ToString();
                    message = "4" + message;
                    int len = message.Length;
                    if (len >= 10) message = len.ToString() + message;
                    else message = "0" + len.ToString() + message;
                    ServerConnect.instance.sendMessage(message);
                    string mess = ServerConnect.instance.receiveMessage();
                }
                catch(Exception ex)
                {
                    Debug.LogWarning("Cur bird's pos y exception: " + ex.ToString());
                }
                Thread.Sleep(2000);
            }
        }

        public static void close()
        {
            mythread.Abort();
        }
    }
}
