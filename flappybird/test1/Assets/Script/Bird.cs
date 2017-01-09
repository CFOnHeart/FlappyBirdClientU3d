using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    // public AudioClip clipHit;
    //  public AudioClip clipJump;
   // public AudioClip[] clip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //加了is trigger勾选的话就碰撞会进入这个函数
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("小鸟受到了管子的collider");
        PipeScript.isEnd = true;
    }
    //没加is trigger勾选的话就碰撞会进入这个函数
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "ceil")
        {
            Debug.Log("小鸟已经撞到了天花板，无法在往上飞");
        }
        else
        {
            this.GetComponent<AudioSource>().clip = Resources.Load("music/hit") as AudioClip;
            this.GetComponent<AudioSource>().Play();
            Debug.Log("小鸟受到了管子或者地面的collision");
            PipeScript.isCollide = true;
        }
    }
}
