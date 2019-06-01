using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    public float speed;
    public float scrollSpeed = 1F;
    public string playerColor;
    public GameObject explosion;
    Rigidbody2D thisRigidbody;
    Vector2 worldStartPoint = new Vector2();
    Vector2 screenResolution = new Vector2();
    float screenSize;

    // Use this for initialization
    void Start () {
        speed = 400f;

        if (PlayerPrefs.HasKey("PrestigeSpeed"))
        {
            scrollSpeed = PlayerPrefs.GetFloat("PrestigeSpeed");
        }

        if (!PlayerPrefs.HasKey("Lives"))
        {
            PlayerPrefs.SetInt("Lives", 3);
        }

        screenSize = Mathf.Max(Screen.width, Screen.height);
    }

    private void FixedUpdate()
    {
        //Scroll the game
        ScrollMovement();

        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
            //Update Touch Input
            TouchInput();
        //}
        //else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{
            //MouseInput();
        //}
    }

    void ScrollMovement()
    {
        //Scroll the player forward
        transform.Translate(Vector3.up * scrollSpeed * Time.fixedDeltaTime);
        //thisRigidbody.MovePosition(Vector2.up * scrollSpeed * Time.deltaTime);

        //Scroll the camera forward
        Camera.main.transform.Translate(Vector3.up * scrollSpeed * Time.fixedDeltaTime);
    }

    void MouseInput()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        transform.position = mousePos;
    }

    void TouchInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //Resolution SHITT
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            Vector2 touchDeltaPercentage = touchDeltaPosition / screenSize;

            // Move object across XY plane
            if (transform.position.x >= 2.75f && touchDeltaPosition.x > 0)
            {
                touchDeltaPercentage.x = 0f;
                transform.Translate(touchDeltaPercentage * speed * Time.fixedDeltaTime);
            }
            else if (transform.position.x <= -2.75f && touchDeltaPosition.x < 0)
            {
                touchDeltaPercentage.x = 0f;
                transform.Translate(touchDeltaPercentage * speed * Time.fixedDeltaTime);
            }
            else
            {
                transform.Translate(touchDeltaPercentage * speed * Time.fixedDeltaTime);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
        if (collision.collider.CompareTag("Gamepiece"))
        {
            if(collision.collider.GetComponent<Gamepiece>().GamepieceColor != playerColor)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        //Start our Explosion
        Instantiate(explosion, transform.position, Quaternion.identity);
        StartCoroutine(EndGame());

        //Stop player from moving
        scrollSpeed = 0;
        speed = 0;

        //Score
        PlayerPrefs.SetFloat("Score", GameObject.FindGameObjectWithTag("FinishLine").GetComponent<FinishLine>().score);
        StopCoroutine(GameObject.FindGameObjectWithTag("FinishLine").GetComponent<FinishLine>().dTimer);

        //Destroy Children
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            Destroy(transform.GetChild(childIndex).gameObject);
        }

        //Clean up
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());

        //Kill TrailRenderer
        Destroy(GetComponent<TrailRenderer>());

        //Set Level and Lives
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1);

        //They didnt finish the level
        PlayerPrefs.SetInt("Finished", 0);
    }

    IEnumerator EndGame()
    {
        //Wait 2 seconds and load intermediate scene
        yield return new WaitForSeconds(2F);
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings -1);
    }
}
