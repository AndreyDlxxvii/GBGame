using System;
using System.Collections.Generic;
using System.Linq;
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

    private const string AuthGuidKey = "auth_guids";
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
        SetUserData(obj.PlayFabId);
        //MakePurchase();
        GetInventory();
        //SetUserHP(100);
        GetUserHP(obj.PlayFabId);
    }

    private void SetUserHP(int HP)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"HP_user", HP.ToString()}
            }
        }, result=> Debug.Log($"Update User HP {HP}"),
            OnLoginError);
    }

    private void GetUserHP(string playFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        }, result =>
        {
            Debug.Log("Get user HP complete");
            Debug.Log($"User HP:{result.Data["HP_user"].Value}");
        },
            OnLoginError);
    }

    private void SetUserData(string objPlayFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"time_receive_daily_reward", DateTime.UtcNow.ToString()}
            }
        }, result =>
        {
            Debug.Log("Complete update user date");
            GetUserData(objPlayFabId, "time_receive_daily_reward");
        }, OnLoginError);
    }

    private void GetUserData(string playfabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playfabId
        }, 
            result =>
        {
            Debug.Log($"{keyData}: {result.Data[keyData].Value}");
        }, OnLoginError);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "MyCatalog",
            ItemId = "health_potion",
            Price = 3,
            VirtualCurrency = "MY"
        },
            result =>
            {
                Debug.Log("Complete purchase");
            }, OnLoginError);
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result=>ShowInventory(result.Inventory), OnLoginError );
    }

    private void ShowInventory(List<ItemInstance> items)
    {
        var firstItem = items.First();
        Debug.Log(firstItem.ItemId);
        ConsumePotion(firstItem.ItemInstanceId);
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        }, result=>Debug.Log("Consume complete"), OnLoginError );
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
