using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour {

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
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer)
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
            if (PlayerPrefs.GetInt("Lives") > 0)
            {
                if (PlayerPrefs.GetInt("Level") == 19)
                {
                    //Prestige and restart going faster.
                    if (!PlayerPrefs.HasKey("PrestigeSpeed"))
                    {
                        PlayerPrefs.SetFloat("PrestigeSpeed", 1.5f);
                    }
                    else if (PlayerPrefs.HasKey("PrestigeSpeed"))
                    {
                        PlayerPrefs.SetFloat("PrestigeSpeed", PlayerPrefs.GetFloat("PrestigeSpeed") + 0.5f);
                    }
                    PlayerPrefs.SetInt("Level", 0);
                    StartCoroutine(AnalyticPrestiged());
                }
                SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
                //SceneManager.LoadScene(8);
            }
            else
            {
                PlayerPrefs.SetInt("Lives", 3);
                StartCoroutine(AnalyticRestarted());
                PlayerPrefs.SetFloat("PrestigeSpeed", 1f);
                //Highscore
                if (PlayerPrefs.HasKey("Highscore"))
                {
                    if (PlayerPrefs.GetFloat("Highscore") < PlayerPrefs.GetFloat("Score"))
                    {
                        PlayerPrefs.SetFloat("Highscore", PlayerPrefs.GetFloat("Score"));
                    }
                }
                else if (!PlayerPrefs.HasKey("Highscore"))
                {
                    PlayerPrefs.SetFloat("Highscore", PlayerPrefs.GetFloat("Score"));
                }
                PlayerPrefs.SetFloat("Score", 0f);
                SceneManager.LoadScene(0);
            }
        }
    }

    public void InitFB()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
    }

    IEnumerator AnalyticPrestiged()
    {
        InitFB();
        while (!FB.IsInitialized)
        {
            yield return null;
        }

        //this.Status = "Logged FB.AppEvent";
        FB.LogAppEvent(
            AppEventName.UnlockedAchievement,
                    null,
                    new Dictionary<string, object>()
                    {
                        { AppEventParameterName.Description, "Prestige. Beat Level 20" }
                    });
        Debug.Log(
            "You may see results showing up at https://www.facebook.com/analytics/"
            + FB.AppId);
    }

    IEnumerator AnalyticRestarted()
    {
        InitFB();
        while (!FB.IsInitialized)
        {
            yield return null;
        }

        //this.Status = "Logged FB.AppEvent";
        FB.LogAppEvent(
            AppEventName.AchievedLevel,
                    null,
                    new Dictionary<string, object>()
                    {
                        { AppEventParameterName.Description, "Ran out of Lives, Restarted at Level 0" }
                    });
        Debug.Log(
            "You may see results showing up at https://www.facebook.com/analytics/"
            + FB.AppId);
    }
}
