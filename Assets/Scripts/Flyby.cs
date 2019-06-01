using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyby : MonoBehaviour {
    public float speed = 5f;
    public Vector2 endLocation;
    Vector2 startLocation;
    Vector2 direction;
    Transform playerTransform;

	// Use this for initialization
	void Start () {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        startLocation = transform.position;
        direction = startLocation - endLocation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < 8)
        {
            transform.Translate(-direction * speed * Time.fixedDeltaTime);
        }
    }

    private void OnBecameInvisible()
    {
        transform.position = startLocation;
    }
}
