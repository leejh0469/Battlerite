using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _playerItem;
    [SerializeField] private Button _readyBtn;

    public void UpdatePlayerList()
    {
        foreach(Transform child in _container) Destroy(child.gameObject);

        var players = Object.FindObjectsByType<PlayerStatus>(FindObjectsSortMode.None);

        foreach(var p in players)
        {
            var item = Instantiate(_playerItem, _container).GetComponent<UI_LobbyPlayerItem>();
            item.SetPlayerInfo(p.Nickname.ToString(), p.IsReady);
        }
    }

    public void OnReadyBtnPressed()
    {
        var localPlayer = NetworkHandler.Instance.Runner.GetPlayerObject(NetworkHandler.Instance.Runner.LocalPlayer);
        localPlayer.GetComponent<PlayerStatus>().RPC_ToggleReady();
    }

    public void OnStartBtnPressed()
    {

    }
}
