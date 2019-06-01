using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class Countdown : MonoBehaviour {
    public List<Mesh> Numbers;
    public GameObject continueText;

    // Use this for initialization
    void Start () {
        StartCoroutine(DoCountdown());
    }

    IEnumerator DoCountdown()
    {
        foreach (Mesh mesh in Numbers)
        {
            GetComponent<MeshFilter>().mesh = mesh;
            yield return new WaitForSecondsRealtime(1);
        }

        //Load Scene
        continueText.SetActive(true);

        Destroy(this.gameObject);
    }
}
