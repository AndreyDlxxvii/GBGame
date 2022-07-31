using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private Button _clearPlayerPrefs;
    [SerializeField] private RectTransform _scrollViewCatalog;
    [SerializeField] private TMP_Text _fieldForCataloItem;

    [SerializeField] private GameObject _newCharacteCreatePanel;
    [SerializeField] private Button _createCharacterButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private List<CharacterWidget> _slots;

    private string _characterName;
    private const string _level = "Level";
    private const string _gold = "Gold";
    private const string _hp = "HP";
    private const string _damage = "Damage";
    private const string _experience = "Experience";
    
    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);
        GetCharacters();
        _clearPlayerPrefs.onClick.AddListener(ClearSaves);
        foreach (var slot in _slots) 
            slot.Button.onClick.AddListener(OpenCreateNewCharacter);

        _createCharacterButton.onClick.AddListener(CreateCharacterWithItem);
        _inputField.onValueChanged.AddListener(OnNameChanged);
    }

    private void CreateCharacterWithItem()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "start_item"
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        }, OnError);
    }

    private void UpdateCharacterStatistics(string resultCharacterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = resultCharacterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {_level, 1},
                {_gold, 0},
                {_hp, 100},
                {_damage, 10},
                {_experience, 0}
            }
        },
            result =>
            {
                Debug.Log("UpdateCharacterStatistic complete!!!"); 
                CloseCreateNewCharacter();
                GetCharacters();
            }, OnError);
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                Debug.Log($"Character count: {result.Characters.Count}");
                ShowCharacterInSlots(result.Characters);
            }, OnError);
    }

    private void ShowCharacterInSlots(List<CharacterResult> resultCharacters)
    {
        if (resultCharacters.Count == 0)
        {
            foreach (var slot in _slots)
            {
                slot.ShowEmptySlot();
            }
        }
        else if (resultCharacters.Count > 0 && resultCharacters.Count <= _slots.Count )
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = resultCharacters.First().CharacterId
            }, result =>
            {
                var level = result.CharacterStatistics[_level].ToString();
                var gold = result.CharacterStatistics[_gold].ToString();
                var hp = result.CharacterStatistics[_hp].ToString();
                var damage = result.CharacterStatistics[_damage].ToString();
                var experience = result.CharacterStatistics[_experience].ToString();
                
                _slots.First().ShowInfoCharacterSlot(resultCharacters.First().CharacterName, level, gold, hp, damage, experience);
            },OnError);
        }
        else
        {
           Debug.LogError("Added slots of characters"); 
        }
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacteCreatePanel.SetActive(true);
    }
    
    private void CloseCreateNewCharacter()
    {
        _newCharacteCreatePanel.SetActive(false);
    }

    private void OnNameChanged(string name)
    {
        _characterName = name;
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
        _createCharacterButton.onClick.RemoveAllListeners();
        _inputField.onValueChanged.RemoveAllListeners();
    }
}