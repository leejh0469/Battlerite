using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _playerItem;
    [SerializeField] private Button _readyBtn;
    [SerializeField] private Button _startBtn;

    public void UpdatePlayerList()
    {
        foreach(Transform child in _container) Destroy(child.gameObject);

        var players = Object.FindObjectsByType<PlayerStatus>(FindObjectsSortMode.None);

        foreach(var p in players)
        {
            var item = Instantiate(_playerItem, _container).GetComponent<UI_LobbyPlayerItem>();
            item.SetPlayerInfo(p.Nickname.ToString(), p.IsReady);
        }

        CheckStartCondition(players);
    }

    private void CheckStartCondition(PlayerStatus[] players)
    {
        _startBtn.gameObject.SetActive(NetworkHandler.Instance.Runner.IsServer);

        bool allReady = true;
        foreach( PlayerStatus p in players)
        {
            if (!p.IsReady)
            {
                allReady = false; 
                break;
            }
        }

        _startBtn.interactable = allReady && players.Length >= 2;
    }

    public void OnReadyBtnPressed()
    {
        var localPlayer = NetworkHandler.Instance.Runner.GetPlayerObject(NetworkHandler.Instance.Runner.LocalPlayer);
        localPlayer.GetComponent<PlayerStatus>().RPC_ToggleReady();
    }

    public void OnStartBtnPressed()
    {
        NetworkHandler.Instance.ChangeScene(2);
    }
}
