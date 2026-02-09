using Fusion;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private Button _enterBtn;

    public void Init(SessionInfo info, UnityAction onEnterBtnPressed)
    {
        _roomNameText.text = info.Name;
        _enterBtn.onClick.AddListener(onEnterBtnPressed);
    }
}
