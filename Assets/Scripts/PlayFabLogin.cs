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

    private const string AuthGuidKey = "auth_guid";
    private void Start()
    {
        _connectPlayFab.onClick.AddListener(Connect);
    }

    private void Connect()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "767B9";

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation
        };
        PlayFabClientAPI.LoginWithCustomID(request, 
            succes =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSucces(succes);
            }, OnLoginError);
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
