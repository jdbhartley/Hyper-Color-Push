using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimNearPlayer : MonoBehaviour {

    private void Awake()
    {
        GetComponent<Animator>().StopPlayback();

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnBecameVisible()
    {
        GetComponent<Animator>().StartPlayback();
    }
}
