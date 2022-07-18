using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountWindow : AccountDataWindowsBase
{
    [SerializeField] private InputField _emailInputField;
    [SerializeField] private Button _createAccountBtn;

    private string _email;

    protected override void SubscribeElementsUI()
    {
        base.SubscribeElementsUI();
        _emailInputField.onValueChanged.AddListener(UpdateEmail);
        _createAccountBtn.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username, 
            Email = _email, 
            Password = _password,
        }, result =>
            {
                Debug.Log($"Success {_username}");
            }, 
            error => Debug.LogError($"Error {error.Error}"));
    }

    private void UpdateEmail(string email)
    {
        _email = email;
    }
}
