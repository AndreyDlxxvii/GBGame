using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SignInWindow : AccountDataWindowsBase
{
    [SerializeField] private Button _signInBtn;
    [SerializeField] private RectTransform _progressBar;
    [SerializeField] private Canvas _canvas;

    private float _millesecound;
    private bool _isPressLogin;

    protected override void SubscribeElementsUI()
    {
        base.SubscribeElementsUI();
        _signInBtn.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        _canvas.enabled = false;
        _isPressLogin = true;
        
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        },
            result =>
            {
                _progressBar.gameObject.SetActive(false);
                EnterInGameScene();
            }, 
            error => { Debug.LogError(error.Error); });
    }

    private void Update()
    {
        if (_isPressLogin)
        {
            var time = Time.deltaTime * 500f;
            _progressBar.Rotate(Vector3.back * time);
        }
    }
}
