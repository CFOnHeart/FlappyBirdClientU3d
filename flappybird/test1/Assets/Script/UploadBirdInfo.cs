using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class UploadBirdInfo
    {
        public static IEnumerator uploadBirdPosY()
        {
           
            while (PipeScript.isEnd == false)
            {
                yield return new WaitForSeconds(2);
                
                try
                {
                    if (ServerConnect.instance == null)
                    {
                        ServerConnect.instance = new ServerConnect();
                        ServerConnect.instance.connect();
                    }
                    int value = (int)(PipeScript.bird.transform.position.y * 100);
                    string message = value.ToString();
                    message = "4" + message;
                    int len = message.Length;
                    if (len >= 10) message = len.ToString() + message;
                    else message = "0" + len.ToString() + message;
                    ServerConnect.instance.sendMessage(message);
                    string mess = ServerConnect.instance.receiveMessage();
                    Debug.Log("we are in the new thread "+mess+" cur y: "+ PipeScript.bird.transform.position.y.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Cur bird's pos y exception: " + ex.ToString());
                }
            }
        }
    
    }
}
