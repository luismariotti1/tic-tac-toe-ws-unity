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
    [SerializeField] private GameObject signInButton;
    [SerializeField] private GameObject signUpButton;
    [SerializeField] private GameObject singUpForm;

    void Start()
    {
        signInButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(SignInRequest()); });
        signUpButton.GetComponent<Button>().onClick.AddListener(ChangeForm);
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
    
    private void ChangeForm()
    {
        gameObject.SetActive(false);
        singUpForm.SetActive(true);
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