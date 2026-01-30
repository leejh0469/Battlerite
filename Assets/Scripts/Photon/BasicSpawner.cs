using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkObject PlayerPrefab;

    public void OnConnectedToServer(NetworkRunner runner)
    {}

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {}

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("oninput");

        var data = new NetworkInputData();

        data.direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        data.mousePosition = GetMouseWorldPosition();

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer) // 내가 방장(Host)일 때만 소환 명령을 내림
        {
            Debug.Log($"{player} 접속! 캐릭터 소환 중...");
            // 캐릭터를 (0, 1, 0) 위치에 소환하고, 조종권(player)을 넘겨줌
            runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    private Vector3 GetMouseWorldPosition()
    {
        // 1. 카메라에서 마우스 위치를 통과하는 광선(Ray)을 생성합니다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 2. 광선이 무엇인가(바닥 등)에 부딪혔는지 확인합니다.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 3. 부딪힌 지점의 3D 좌표를 반환합니다.
            return hit.point;
        }

        // 부딪힌 곳이 없다면 기본값 반환
        return Vector3.zero;
    }
}
