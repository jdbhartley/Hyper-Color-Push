using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPiece : MonoBehaviour {
    public float speed = 45f;
    public bool reverse = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (!reverse)
        {
            transform.Rotate(Vector3.forward * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Rotate(Vector3.back * speed * Time.fixedDeltaTime);
        }
    }
}
