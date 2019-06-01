using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeMessage : MonoBehaviour {
    public GameObject MessageBox;
    float timeScale;

    private void Awake()
    {
            InitFB();
    }

    // Use this for initialization
    void Start () {

		if (!PlayerPrefs.HasKey("FirstTime"))
        {
            MessageBox.SetActive(true);
            timeScale = Time.timeScale;
            Time.timeScale = 0f;
            StartCoroutine(AnalyticAppFirstTime());
        }
	}

    public void InitFB()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
    }

    public void OkayButton()
    {
        MessageBox.SetActive(false);
        PlayerPrefs.SetString("FirstTime", "false");
        Time.timeScale = timeScale;
    }

    IEnumerator AnalyticAppFirstTime()
    {
        InitFB();
        while (!FB.IsInitialized)
        {
            yield return null;
        }

        //this.Status = "Logged FB.AppEvent";
        FB.LogAppEvent(
            AppEventName.CompletedTutorial,
                    null,
                    new Dictionary<string, object>()
                    {
                        { AppEventParameterName.Description, "Played for the first time." }
                    });
        Debug.Log(
            "You may see results showing up at https://www.facebook.com/analytics/"
            + FB.AppId);
    }
}
