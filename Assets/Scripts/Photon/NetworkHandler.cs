using CoreSystem.Common;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkHandler : MonoSingleton<NetworkHandler>
{
    private NetworkRunner _runner;

    public NetworkRunner Runner { get { return _runner; } }

    protected override void Awake()
    {
        base.Awake();

        JoinLobby();
    }

    private async void JoinLobby()
    {
        if(_runner == null) _runner = gameObject.AddComponent<NetworkRunner>();

        var result = await _runner.JoinSessionLobby(SessionLobby.ClientServer);

        if (result.Ok) Debug.Log("포톤 로비 접속 완료");
        else Debug.LogError($"로비 접속 실패: {result.ShutdownReason}");
    }

    public async void StartNetworkGame(GameMode mode, string roomName, int sceneIndex)
    {
        if(_runner == null) _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var sceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>();
        if (sceneManager == null) sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();

        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneRef.FromIndex(sceneIndex), // 이동할 씬 번호 (예: 대기실 씬)
            SceneManager = sceneManager
        });

        if (result.Ok)
        {
            Debug.Log($"네트워크 시작 성공: {mode} 모드 / 방이름: {roomName}");
        }
        else
        {
            Debug.LogError($"네트워크 시작 실패: {result.ShutdownReason}");
        }
    }
}
