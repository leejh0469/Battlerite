using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : MonoBehaviour
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Color _dimColor = new Color(0,0,0,0f);

    private GameObject _dimGO;

    protected virtual void OnEnable()
    {
        _closeBtn.onClick.RemoveAllListeners();
        _closeBtn.onClick.AddListener(OnCloseBtnPressed);

        AddDim();
    }

    private void OnCloseBtnPressed()
    {
        UI_Manager.Instance.ClosePopup();
    }

    private void AddDim()
    {
        if (_dimGO != null) return;

        _dimGO = new GameObject("UI_Dim");
        _dimGO.transform.SetParent(transform, false);

        RectTransform rect = _dimGO.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero; // (0, 0)
        rect.anchorMax = Vector2.one;  // (1, 1)
        rect.offsetMin = Vector2.zero; // Left, Bottom 0
        rect.offsetMax = Vector2.one;  // Right, Top 0

        Image img = _dimGO.AddComponent<Image>();
        img.color = _dimColor;
        img.raycastTarget = true;

        _dimGO.transform.SetAsFirstSibling();
    }
}
