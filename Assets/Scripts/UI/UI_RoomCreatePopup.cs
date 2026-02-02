using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomCreatePopup : UI_Popup
{
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private Button _submitBtn;

    protected override void OnEnable()
    {
        base.OnEnable();

        _submitBtn.onClick.RemoveAllListeners();
        _submitBtn.onClick.AddListener(OnSubmitBtnPressed);
    }

    private void OnSubmitBtnPressed()
    {
        if(_roomNameInput.text == "")
        {
            return;
        }

        NetworkHandler.Instance.StartNetworkGame(Fusion.GameMode.Host, _roomNameInput.text, 1);
    }
}
