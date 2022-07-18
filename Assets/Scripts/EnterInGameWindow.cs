using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInBtn;
    [SerializeField] private Button _createAccountBtn;

    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _createAccountCanvas;
    [SerializeField] private Canvas _signInCanvas;

    private void Start()
    {
        _signInBtn.onClick.AddListener(OpenSighInWindow);
        _createAccountBtn.onClick.AddListener(CreateAccountWindow);
    }
    
    private void OpenSighInWindow()
    {
        _enterInGameCanvas.enabled = false;
        _signInCanvas.enabled = true;
    }
    private void CreateAccountWindow()
    {
        _enterInGameCanvas.enabled = false;
        _createAccountCanvas.enabled = true;
    }

    private void OnDestroy()
    {
        _signInBtn.onClick.RemoveAllListeners();
        _createAccountBtn.onClick.RemoveAllListeners();
    }
}
