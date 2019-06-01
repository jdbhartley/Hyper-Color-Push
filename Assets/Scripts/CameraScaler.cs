using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour {

    //Half of the Units you want displayed (actually half of half since camera is centered)
    float halfWidth = 3f;

    // Use this for initialization
    void Start () {
        Camera.main.orthographicSize = halfWidth / Camera.main.aspect;
        Debug.Log(halfWidth / Camera.main.aspect);
    }
}
