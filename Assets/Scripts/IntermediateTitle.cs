using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using Facebook.Unity;

public class IntermediateTitle : MonoBehaviour {
    public Mesh GameOverMesh;
    public Mesh NextLevelMesh;
    public Text livesRemainingLabel;
    public GameObject adText;
    public Text levelText;
    public Text scoreText;
    public Text highScoreText;

    public bool rewardedVideo = true;

    public bool EnableAds;

    public GameObject PrestigeMessage;

    Vector3 NextLevelPosition = new Vector3(-0.38f, 2.13f, -0.02539063f);
    Vector3 GameOverPosition = new Vector3(0.02f, 2.13f, -0.02539063f);

    private void Awake()
    {
        InitFB();
    }

    // Use this for initialization
    void Start() {
        levelText.text = "LEVEL " + PlayerPrefs.GetInt("Level").ToString();
        scoreText.text = "SCORE: " + PlayerPrefs.GetFloat("Score").ToString();

        if (PlayerPrefs.HasKey("Highscore"))
        {
            highScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetFloat("Highscore");
        }

        if (PlayerPrefs.GetInt("Finished") == 0)
        {
            //They Died last
            GetComponent<MeshFilter>().mesh = GameOverMesh;
            transform.parent.position = GameOverPosition;
            StartCoroutine(AnalyticLevelFail(PlayerPrefs.GetInt("Level")));
        }
        else
        {
            //They finished last Level
            GetComponent<MeshFilter>().mesh = NextLevelMesh;
            transform.parent.position = NextLevelPosition;
            if (PlayerPrefs.GetInt("Level") == 19)
            {
                PrestigeMessage.SetActive(true);
            }
            StartCoroutine(AnalyticLevelComplete(PlayerPrefs.GetInt("Level")));
        }
        livesRemainingLabel.text = "REMAINING LIVES: " + PlayerPrefs.GetInt("Lives").ToString();

        //Advertisement crap
        if (EnableAds)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Advertisement.Initialize("2865675", true);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Advertisement.Initialize("2865674", true);
            }

            StartCoroutine(ShowAdTextWhenReady());
        }
    }

    public void InitFB()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
    }

    public void PrestigeButton()
    {
        PrestigeMessage.SetActive(false);
    }

    IEnumerator AnalyticLevelComplete(int level)
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
                        { AppEventParameterName.Description, "Completed Level: " + level }
                    });
                Debug.Log(
                    "You may see results showing up at https://www.facebook.com/analytics/"
                    + FB.AppId);
    }

    IEnumerator AnalyticLevelFail(int level)
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
                        { AppEventParameterName.Description, "Failed Level: " + level }
                    });
        Debug.Log(
            "You may see results showing up at https://www.facebook.com/analytics/"
            + FB.AppId);
    }



    IEnumerator ShowAdTextWhenReady()
    {
        float initialTime = Time.time + 2f;
        float timer = Time.time;

        while (!Advertisement.IsReady("rewardedVideo"))
        {
            timer = Time.time;
            if (timer > initialTime)
            {
                if (Advertisement.IsReady("video"))
                {
                    rewardedVideo = false;
                    adText.SetActive(true);
                    yield break;
                }
            }
            yield return null;
        }

        adText.SetActive(true);
    }

    void AdCallbackhandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                int currentLives = PlayerPrefs.GetInt("Lives");
                PlayerPrefs.SetInt("Lives", currentLives + 1);
                livesRemainingLabel.text = "REMAINING LIVES: " + PlayerPrefs.GetInt("Lives").ToString();
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad skipped. Son, I am dissapointed in you");
                break;
            case ShowResult.Failed:
                Debug.Log("I swear this has never happened to me before");
                break;
        }
    }

    public void ShowAd()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallbackhandler;

        if (rewardedVideo)
        {
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Advertisement.Show("video", options);
        }
    }
}
