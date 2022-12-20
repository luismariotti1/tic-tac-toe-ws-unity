using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject authButton;

    void Start()
    {
        button.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(SignInRequest()); });
        authButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(AuthRequest()); });
    }

    private IEnumerator SignInRequest()
    {
        var json = JsonUtility.ToJson(new SignInData(username.text, password.text));
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        var uwr = new UnityWebRequest("http://localhost/auth/sign-in", "POST");
        uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        using (uwr)
        {
            // check if the request is done
            yield return uwr.SendWebRequest();

            // check if the request is successful
            if (uwr.result == UnityWebRequest.Result.Success)
            {
                // get the access token from the response
                var response = JsonUtility.FromJson<SignInResponse>(uwr.downloadHandler.text);
                PlayerPrefs.SetString("access_token", response.access_token);
                Debug.Log("success");
                Debug.Log(response.access_token);
            }
            else
            {
                Debug.Log("Request Failed");
                Debug.Log(uwr.error);
            }
        }
    }

    private IEnumerator AuthRequest()
    {
        var uwr = new UnityWebRequest("http://localhost", "GET");

        uwr.downloadHandler = new DownloadHandlerBuffer();

        // get the access token from the player prefs
        var token = PlayerPrefs.GetString("access_token");
        Debug.Log(token);

        // add as bearer token
        token = "Bearer " + token;
        uwr.SetRequestHeader("Authorization", token);

        using (uwr)
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.downloadHandler.text);
            }
            else
            {
                Debug.Log(uwr.error);
            }
        }
    }
}

internal class SignInData
{
    public string username;
    public string password;
    
    // constructor
    public SignInData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

internal class SignInResponse
{
    public string access_token;
}