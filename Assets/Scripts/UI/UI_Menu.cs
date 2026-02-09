using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private Button _roomCreateBtn;

    [SerializeField] private GameObject _roomItemPrefab;
    [SerializeField] private Transform _itemContainer;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _roomCreateBtn.onClick.RemoveAllListeners();
        _roomCreateBtn.onClick.AddListener(OnRoomCreateBtnPressed);
    }

    private void OnRoomCreateBtnPressed()
    {
        UI_Manager.Instance.ShowPopup<UI_RoomCreatePopup>();
    }

    public void RefreshList(List<SessionInfo> sessionList)
    {
        foreach (Transform child in _itemContainer) Destroy(child.gameObject);

        foreach (var session in sessionList)
        {
            // 방이 가득 차지 않았고, 공개된 방만 표시
            if (session.IsVisible && session.IsOpen)
            {
                var go = Instantiate(_roomItemPrefab, _itemContainer);
                var item = go.GetComponent<UI_RoomItem>();

                // UI 세팅 및 클릭 시 해당 방으로 Join 명령
                item.Init(session, () =>
                {
                    NetworkHandler.Instance.StartNetworkGame(GameMode.Client, session.Name, 1);
                });
            }
        }
    }
}
