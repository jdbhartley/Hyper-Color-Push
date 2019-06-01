using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class PlayAd : MonoBehaviour {
    public IntermediateTitle intermediateTitle;

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0 && Input.touchCount < 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    checkTouch(Input.GetTouch(0).position);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                checkTouch(Input.mousePosition);
            }
        }

    }

    private void checkTouch(Vector3 pos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        Collider2D hit = Physics2D.OverlapPoint(touchPos);

        if (hit && hit == gameObject.GetComponent<Collider2D>())
        {
            intermediateTitle.ShowAd();
            
        }
    }
}

