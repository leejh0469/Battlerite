using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnDataChange))] public NetworkString<_16> Nickname { get; set; }
    [Networked, OnChangedRender(nameof(OnDataChange))] public NetworkBool IsReady { get; set; }

    private void OnDataChange()
    {
        var uiLobby = UI_Manager.Instance.GetPanel<UI_Lobby>();
        uiLobby.UpdatePlayerList();
    }

    public override void Spawned()
    {
        var lobbyUI = UI_Manager.Instance.GetPanel<UI_Lobby>();
        if (lobbyUI != null) lobbyUI.UpdatePlayerList();

        Runner.MakeDontDestroyOnLoad(gameObject);
    }

    public void SetPlayerInfo(string name)
    {
        if (Object.HasStateAuthority)
        {
            Nickname = name;
            IsReady = false;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ToggleReady()
    {
        IsReady = !IsReady;
    }
}
