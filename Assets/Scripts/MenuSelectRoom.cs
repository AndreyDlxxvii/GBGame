using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelectRoom : MonoBehaviour
{
    [SerializeField] private Button _joinRoom;
    [SerializeField] private TMP_Text _text;

    public event Action<String> Join;
    public void SetRoomName(RoomInfo roomInfo)
    {
        _text.text = $"Room name: {roomInfo.Name} People: {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        _joinRoom.onClick.AddListener(() => Join(roomInfo.Name));
    }

    private void OnDestroy()
    {
        _joinRoom.onClick.RemoveAllListeners();
    }
}
