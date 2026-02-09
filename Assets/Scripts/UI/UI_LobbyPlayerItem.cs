using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyPlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;

    [Header("Player Ready Reference")]
    [SerializeField] private GameObject _playerReadyGO;
    [SerializeField] private GameObject _playerNotReadyGO;

    public void SetPlayerInfo(string name, bool isReady)
    {
        _nameText.text = name;

        _playerReadyGO.SetActive(isReady);
        _playerNotReadyGO.SetActive(!isReady);
    }
}
