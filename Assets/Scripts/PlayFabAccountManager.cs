using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private Button _clearPlayerPrefs;
    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        _clearPlayerPrefs.onClick.AddListener(ClearSaves);
    }

    private void OnError(PlayFabError playFabError)
    {
        var errorMessage = playFabError.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void OnGetAccount(GetAccountInfoResult getAccountInfoResult)
    {
        _titleLabel.text = $"Welcome back, Player ID {getAccountInfoResult.AccountInfo.PlayFabId} " +
                           $"Login: {getAccountInfoResult.AccountInfo.Username}" +
                           $"The player's profile Created date is: {getAccountInfoResult.AccountInfo.Created}";
    }
    
    private void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        _clearPlayerPrefs.onClick.RemoveAllListeners();
    }
}
