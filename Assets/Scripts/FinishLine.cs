using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour {
    public Text remainingDistance;
    Transform playerTransform;
    public Transform distanceMarker;
    Vector2 playerDist;
    Vector2 markerDist;
    public float score;
    public Coroutine dTimer;
    float prestige;

	// Use this for initialization
	void Start () {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        markerDist = new Vector2(0, distanceMarker.position.y);

        dTimer = StartCoroutine("DistanceTimer");

        if (PlayerPrefs.HasKey("PrestigeSpeed"))
        {
            prestige = PlayerPrefs.GetFloat("PrestigeSpeed");
        }
        else
        {
            prestige = 1;
        }

        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetFloat("Score");
        }
        else
        {
            score = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
    }

    IEnumerator DistanceTimer()
    {
        while (true)
        {
            score += (10 * prestige);
            remainingDistance.text = score.ToString();
            yield return new WaitForSeconds(0.1F);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //They finished, set PlayerPref to finished
            PlayerPrefs.SetInt("Finished", 1);
            Finished(collision.GetComponent<PlayerMovement>());
        }
    }

    private void Finished(PlayerMovement player)
    {
        //Explosion
        Instantiate(player.explosion, player.transform.position, Quaternion.identity);

        //Stop player from moving
        player.scrollSpeed = 0;
        player.speed = 0;

        //Stop Coroutine 
        StopCoroutine(dTimer);

        //Save score
        Debug.Log("Saving Score: " + score);
        PlayerPrefs.SetFloat("Score", score);

        //Destroy Children
        for (int childIndex = 0; childIndex < player.transform.childCount; childIndex++)
        {
            Destroy(player.transform.GetChild(childIndex).gameObject);
        }

        //Clean up
        Destroy(player.GetComponent<Rigidbody2D>());
        Destroy(player.GetComponent<Collider2D>());

        //Kill TrailRenderer
        Destroy(player.GetComponent<TrailRenderer>());

        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2F);
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings -1);
    }
}
