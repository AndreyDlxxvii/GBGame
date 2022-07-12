using System;
using System.Threading;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _btnConnect;
    [SerializeField] private Button _btnDisconnect;
    [SerializeField] private TMP_Text _textField;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _btnConnect.onClick.AddListener(Connect);
        _btnDisconnect.onClick.AddListener(Disconnect);
    }
    
    private void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnected");
        ShowResult(Color.green, "Connected");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"OnJoinLobby: {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions {MaxPlayers = 2, IsVisible = true}, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinRoom: {PhotonNetwork.InRoom}");
    }
    
    
    private void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ShowResult(Color.red, $"Disconnect {cause}");
        Debug.Log($"Disconnect {cause}");
    }
    
    private void ShowResult(Color color, string mess)
    {
        _textField.text = mess;
        _textField.color = color;
    }

    private void OnDestroy()
    {
        _btnConnect.onClick.RemoveAllListeners();
        _btnDisconnect.onClick.RemoveAllListeners();
    }
}
