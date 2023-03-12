using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private GameObject signUpButton;
    [SerializeField] private GameObject signInButton;
    [SerializeField] private GameObject signInForm;
    
    private void Start()
    {
        signUpButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(Registration()); });
        signInButton.GetComponent<Button>().onClick.AddListener(ChangeForm);
    }

    private IEnumerator Registration()
    {
        var json = JsonUtility.ToJson(new RegisterData(email.text, username.text, password.text));
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        
        var uwr = new UnityWebRequest("http://localhost/auth/sign-up", "POST");
        uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        
        uwr.SetRequestHeader("Content-Type", "application/json");
        
        // check if the request is done
        yield return uwr.SendWebRequest();
        
        // check if the request is successful
        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Registration successful");
        }
        else
        {
            // show the error message
            Debug.Log(uwr.error);
        }
    }
    
    private void ChangeForm()
    {
        gameObject.SetActive(false);
        signInForm.SetActive(true);
    }
}

class RegisterData
{
    public string email;
    public string username;
    public string password;

    public RegisterData(string email, string username, string password)
    {
        this.email = email;
        this.username = username;
        this.password = password;
    }
}