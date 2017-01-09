using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopWindows : MonoBehaviour {
    public GameObject win;
    public UIButton button;
	// Use this for initialization
	void Start () {
        win = GameObject.Find("3D UI/Window");
        win.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClick()
    {
        if(win == null)
        {
            Debug.Log("can't find");
            return;
        }
        win.SetActive(true);
        button.enabled = false;
    }
}
