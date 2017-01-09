using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour {
  
    // Use this for initialization
    void Start () {
     //   Debug.Log("Background Move begin");
    }
	
	// Update is called once per frame
	void Update () {
        if(PipeScript.isCollide == true)
        {
            return;
        }
        this.transform.position += new Vector3(1.0f, 0, 0) * PipeScript.speed / 100;
        if(this.transform.position.x >= 6.67f)
        {
            this.transform.position = new Vector3(-23.88f, this.transform.position.y, 0.0f);
        }
    }
}
