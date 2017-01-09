using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMove : MonoBehaviour {
    
    private bool isCount = false; //判断当前管子是否已经被计算分数，但因为有上下两根管子计数会翻倍
	// Use this for initialization
	void Start () {
      //  Debug.Log("Pipe Move begin");
        this.isCount = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (PipeScript.isCollide == true || PipeScript.isEnd == true)
        {
            return;
        }
        if (PipeScript.isEnd == false)
        {
            this.transform.position += new Vector3(-1.0f, 0, 0) * PipeScript.speed / 100;
        }
        if(this.isCount == false && this.transform.position.x < PipeScript.BIRD_START_POS)
        {
            PipeScript.score++;
          //  Debug.Log("cur time score is : "+PipeScript.score);
            PipeScript.setRealtimeScore((PipeScript.score + 1) / 2);
            this.isCount = true;
        }
	}
}
