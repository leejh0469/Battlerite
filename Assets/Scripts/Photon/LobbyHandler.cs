using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHandler : FusionCallbackBase
{
    [SerializeField] private NetworkObject _playerStatusPrefab;

    private void Start()
    {
        NetworkHandler.Instance.Runner.AddCallbacks(this);
    }

    public override void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            var pObj = runner.Spawn(_playerStatusPrefab, inputAuthority: player);
            pObj.GetComponent<PlayerStatus>().SetPlayerInfo($"Guest {player.PlayerId}");

            runner.SetPlayerObject(player, pObj);
        }
    }
}
