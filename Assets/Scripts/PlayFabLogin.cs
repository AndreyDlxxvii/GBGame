using System;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private Button _connectPlayFab;
    [SerializeField] private TMP_Text _textField;
    private void Start()
    {
        _connectPlayFab.onClick.AddListener(Connect);
    }

    private void Connect()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "767B9";

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Player2",
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucces, OnLoginError);
    }

    private void OnLoginError(PlayFabError obj)
    {
        var errorMessage = obj.GenerateErrorReport();
        ShowResult(Color.red, obj.ErrorMessage);
        Debug.LogError($"Error: {errorMessage}");
    }

    private void OnLoginSucces(LoginResult obj)
    {
        ShowResult(Color.green, "Connect complete!");
        Debug.Log("Complete");
    }

    private void ShowResult(Color color, string mess)
    {
        _connectPlayFab.GetComponent<Graphic>().color = color;
        _textField.text = mess;
    }

    private void OnDestroy()
    {
        _connectPlayFab.onClick.RemoveAllListeners();
    }
}
