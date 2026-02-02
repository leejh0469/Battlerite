using CoreSystem.Common;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoSingleton<UI_Manager>
{
    private Dictionary<System.Type, MonoBehaviour> _uiPanels = new Dictionary<System.Type, MonoBehaviour>();

    private Stack<GameObject> _popupStack = new Stack<GameObject>();

    private const string UI_PATH = "UI/";

    public T GetPanel<T>() where T : MonoBehaviour
    {
        System.Type type = typeof(T);

        if (_uiPanels.TryGetValue(type, out var panel))
        {
            return (T)panel;
        }

        T existingPanel = FindFirstObjectByType<T>(FindObjectsInactive.Include);
        if (existingPanel != null)
        {
            _uiPanels[type] = existingPanel;
            return existingPanel;
        }

        GameObject uiPrefab = Resources.Load<GameObject>($"{UI_PATH}{type.Name}");

        if (uiPrefab == null)
        {
            Debug.LogError($"{type.Name} 프리팹을 {UI_PATH}에서 찾을 수 없습니다!");
            return null;
        }

        GameObject go = Instantiate(uiPrefab, transform);
        T instance = go.GetComponent<T>();

        _uiPanels[type] = instance;
        return instance;
    }

    public void ShowPanel<T>() where T : MonoBehaviour
    {
        var panel = GetPanel<T>();
        if(panel != null) 
            panel.gameObject.SetActive(true);
    }

    public void HidePanel<T>() where T : MonoBehaviour
    {
        var panel = GetPanel<T>();
        if (panel != null) panel.gameObject.SetActive(false);
    }

    public T ShowPopup<T>() where T : MonoBehaviour
    {
        T popup = GetPanel<T>();
        if (popup == null) return null;

        _popupStack.Push(popup.gameObject);
        popup.gameObject.SetActive(true);

        popup.transform.SetAsLastSibling();

        return popup;
    }

    public void ClosePopup()
    {
        if( _popupStack.Count > 0 )
        {
            GameObject popup = _popupStack.Pop();
            popup.SetActive(false);
        }
    }

    public void CloseAllPopups()
    {
        while( _popupStack.Count > 0)
        {
            ClosePopup();
        }
    }
}
