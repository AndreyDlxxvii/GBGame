using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private Button _clearPlayerPrefs;
    [SerializeField] private RectTransform _scrollViewCatalog;
    [SerializeField] private TMP_Text _fieldForCataloItem;
    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);
        _clearPlayerPrefs.onClick.AddListener(ClearSaves);
    }

    private void OnGetCatalog(GetCatalogItemsResult result)
    {
        ShowCatalog(result.Catalog);
        Debug.Log("GetCatalog complete!");
    }

    private void ShowCatalog(List<CatalogItem> items)
    {
        foreach (var item in items)
        {
            var t = Instantiate(_fieldForCataloItem, _scrollViewCatalog);
            t.text = $"ItemID: {item.ItemId}";
            
        }
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
