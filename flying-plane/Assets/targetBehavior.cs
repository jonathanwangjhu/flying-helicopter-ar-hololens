using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetBehavior : MonoBehaviour {

    GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas = GameObject.FindWithTag("Respawn");
        if(canvas == null)
        {
            Debug.Log("couldn't find");
        } else
        {
            Debug.Log("found");
        }
        canvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
    }
}
