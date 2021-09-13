using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class StartScreen : MonoBehaviour
{
    public Controls controls;
    [SerializeField]
    Text email, password, username;
    [SerializeField]
    Text errorText;
    [SerializeField]
    GameObject onlinePanel, offlinePanel, diffSelect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        gameObject.SetActive(false);
        controls.StartGame(1, false);
    }

    public void Login() {
        Debug.Log("Login");
        
        var request = new LoginWithEmailAddressRequest{Email = email.text, Password = password.text};
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFailed);
    }

    public void Signup() {
        var registerRequest = new RegisterPlayFabUserRequest{Email = email.text, Password =  password.text, Username = username.text};
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFailed );
    }



    void RegisterSuccess(RegisterPlayFabUserResult result){
        Debug.Log("Success: " + result);
        errorText.text = "";
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest {DisplayName = username.text}, result=>Debug.Log("a"), RegisterFailed);
        SelectDifficulty();
    }

    void UpdateSuccess() {

    }

    void RegisterFailed(PlayFabError error){
        Debug.Log("Failed: " + error);
        errorText.text = error.GenerateErrorReport();
    }

    void LoginSuccess(LoginResult result){
        Debug.Log("Sucess: " + result);
        errorText.text = "";
        SelectDifficulty();
    }

    void LoginFailed(PlayFabError error){
        Debug.Log("Failed: " + error);
        errorText.text = error.GenerateErrorReport();
    }

    void SelectDifficulty() {
        diffSelect.SetActive(true);
        onlinePanel.SetActive(false);
        offlinePanel.SetActive(false);
    }

    public void StartEasy() {
        gameObject.SetActive(false);
        controls.StartGame(0, true);
    }

    public void StartNormal() {
        gameObject.SetActive(false);
        controls.StartGame(1, true);
        
    }

    public void StartHard() {
        gameObject.SetActive(false);
        controls.StartGame(2, true);
        
    }
}
