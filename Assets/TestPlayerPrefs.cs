using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerPrefs : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        PlayerPrefs.SetInt("Finished", 1);
        PlayerPrefs.SetInt("Level", 19);
	}

}
